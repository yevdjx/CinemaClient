using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Mail;
using iTextSharp.text;
using iTextSharp.text.pdf;

namespace CinemaClient.Services
{
    public class TicketService
    {
        private readonly ApiService _apiService;

        public TicketService(ApiService apiService)
        {
            _apiService = apiService;
        }

        /// <summary>
        /// Генерирует PDF-билет с полной информацией о фильме и сеансе.
        /// </summary>
        /// <param name="movieTitle">Название фильма.</param>
        /// <param name="sessionStartTime">Время начала сеанса (например, "14:30").</param>
        /// <param name="sessionEndTime">Время окончания сеанса (например, "16:45").</param>
        /// <param name="hallName">Название зала.</param>
        /// <param name="seats">Коллекция строк с информацией о местах (например, "Ряд 5 Место 12").</param>
        /// <param name="totalPrice">Общая стоимость билетов.</param>
        /// <param name="customerName">Имя покупателя.</param>
        /// <param name="movieImageBytes">Массив байтов изображения фильма (может быть null).</param>
        /// <returns>Массив байтов PDF-файла.</returns>
        public byte[] GeneratePdfTicket(string movieTitle, string sessionStartTime, string sessionEndTime,
                                      string hallName, IEnumerable<string> seats,
                                      decimal totalPrice, string customerName, byte[] movieImageBytes)
        {
            using (var memoryStream = new MemoryStream())
            {
                // Устанавливаем размер страницы A5 в альбомной ориентации
                var pageSize = PageSize.A5.Rotate(); // A5 повернут на 90 градусов (ширина > высота)
                var document = new Document(pageSize, 25, 25, 30, 30); // Добавляем отступы
                PdfWriter.GetInstance(document, memoryStream);
                document.Open();

                try
                {
                    // Создаем базовый шрифт для поддержки кириллицы (если нужна, иначе HELVETICA)
                    // Для кириллицы лучше использовать шрифты типа "Arial", "Times New Roman" или другие,
                    // которые поддерживают кодировку CP1251 или Unicode.
                    // Для примера я использую HELVETICA, так как она стандартна и не требует embedding шрифта,
                    // но помните о проблемах с кириллицей.
                    // Если кириллица отображается некорректно, замените BaseFont на что-то вроде:
                    // string fontPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Fonts), "arial.ttf");
                    // var baseFont = iTextSharp.text.pdf.BaseFont.CreateFont(fontPath, iTextSharp.text.pdf.BaseFont.IDENTITY_H, iTextSharp.text.pdf.BaseFont.NOT_EMBEDDED);
                    var baseFont = iTextSharp.text.pdf.BaseFont.CreateFont(
                        iTextSharp.text.pdf.BaseFont.HELVETICA,
                        iTextSharp.text.pdf.BaseFont.CP1252, // Или BaseFont.IDENTITY_H для Unicode
                        iTextSharp.text.pdf.BaseFont.NOT_EMBEDDED);

                    // Определяем шрифты
                    var titleFont = new iTextSharp.text.Font(baseFont, 20, iTextSharp.text.Font.BOLD);
                    var movieTitleFont = new iTextSharp.text.Font(baseFont, 16, iTextSharp.text.Font.BOLD);
                    var sectionHeaderFont = new iTextSharp.text.Font(baseFont, 14, iTextSharp.text.Font.BOLD);
                    var regularFont = new iTextSharp.text.Font(baseFont, 12, iTextSharp.text.Font.NORMAL);
                    var boldFont = new iTextSharp.text.Font(baseFont, 12, iTextSharp.text.Font.BOLD);
                    var smallFont = new iTextSharp.text.Font(baseFont, 10, iTextSharp.text.Font.NORMAL);

                    // 1. Заголовок билета
                    var mainTitle = new iTextSharp.text.Paragraph("Электронный билет", titleFont)
                    {
                        Alignment = iTextSharp.text.Element.ALIGN_CENTER
                    };
                    document.Add(mainTitle);
                    document.Add(new iTextSharp.text.Paragraph(" ", regularFont)); // Отступ

                    // 2. Картинка фильма
                    if (movieImageBytes != null && movieImageBytes.Length > 0)
                    {
                        try
                        {
                            var movieImage = iTextSharp.text.Image.GetInstance(movieImageBytes);
                            // Масштабируем изображение, чтобы оно помещалось на страницу
                            movieImage.ScaleToFit(document.PageSize.Width / 3, document.PageSize.Height / 2); // Примерно 1/3 ширины и 1/2 высоты страницы
                            movieImage.Alignment = iTextSharp.text.Element.ALIGN_CENTER;
                            document.Add(movieImage);
                            document.Add(new iTextSharp.text.Paragraph(" ", regularFont)); // Отступ после картинки
                        }
                        catch (Exception ex)
                        {
                            // Если произошла ошибка при добавлении изображения, добавить текст-заменитель
                            document.Add(new iTextSharp.text.Paragraph("Изображение фильма недоступно.", smallFont));
                            Console.WriteLine($"Ошибка при добавлении изображения в PDF: {ex.Message}");
                        }
                    }

                    // 3. Название фильма
                    var movieTitleParagraph = new iTextSharp.text.Paragraph(movieTitle, movieTitleFont)
                    {
                        Alignment = iTextSharp.text.Element.ALIGN_CENTER
                    };
                    document.Add(movieTitleParagraph);
                    document.Add(new iTextSharp.text.Paragraph(" ", regularFont)); // Отступ

                    // 4. Информация о сеансе (время начала, время конца, зал)
                    document.Add(new iTextSharp.text.Paragraph("Детали сеанса:", sectionHeaderFont));
                    document.Add(new iTextSharp.text.Paragraph($"Дата и время: {sessionStartTime} - {sessionEndTime}", regularFont));
                    document.Add(new iTextSharp.text.Paragraph($"Зал: {hallName}", regularFont));
                    document.Add(new iTextSharp.text.Paragraph(" ", regularFont)); // Отступ

                    // 5. Места и ряды
                    var seatsParagraph = new iTextSharp.text.Paragraph("Места: ", regularFont);
                    int seatCount = 0;
                    foreach (var seat in seats)
                    {
                        seatsParagraph.Add(new iTextSharp.text.Chunk($"{seat} ", boldFont)); // Выделяем каждое место жирным
                        seatCount++;
                        // Добавляем перенос строки каждые 3 места для лучшего форматирования
                        if (seatCount % 3 == 0)
                        {
                            seatsParagraph.Add(new iTextSharp.text.Chunk("\n", regularFont));
                        }
                    }
                    document.Add(seatsParagraph);
                    document.Add(new iTextSharp.text.Paragraph(" ", regularFont)); // Отступ

                    // 6. Общая стоимость
                    document.Add(new iTextSharp.text.Paragraph($"Общая стоимость: {totalPrice:C2} руб.", boldFont)); // Форматируем цену как валюту
                    document.Add(new iTextSharp.text.Paragraph(" ", regularFont)); // Отступ

                    // 7. Информация о покупателе (раскомментировано)
                    //document.Add(new iTextSharp.text.Paragraph($"Покупатель: {customerName}", regularFont));
                    //document.Add(new iTextSharp.text.Paragraph(" ", regularFont)); // Отступ

                    // 8. Уникальный номер билета (ID билета)
                    var ticketId = Guid.NewGuid().ToString().ToUpper(); // Генерируем полный GUID для уникальности
                    var ticketIdParagraph = new iTextSharp.text.Paragraph($"ID Билета: {ticketId}", smallFont)
                    {
                        Alignment = iTextSharp.text.Element.ALIGN_CENTER
                    };
                    document.Add(ticketIdParagraph);

                    // Дополнительная информация/дисклеймер
                    document.Add(new iTextSharp.text.Paragraph(" ", regularFont)); // Отступ
                    var disclaimer = new iTextSharp.text.Paragraph(
                        "Пожалуйста, предъявите этот билет на входе. Приятного просмотра!", smallFont)
                    {
                        Alignment = iTextSharp.text.Element.ALIGN_CENTER
                    };
                    document.Add(disclaimer);
                }
                finally
                {
                    // Важно: закрывать документ в блоке finally, чтобы гарантировать его закрытие
                    if (document.IsOpen())
                    {
                        document.Close();
                    }
                }
                return memoryStream.ToArray();
            }
        }

        // Ваш метод SendEmailWithTicket остается без изменений,
        // так как он уже корректно принимает byte[] ticketPdf
        public void SendEmailWithTicket(string recipientEmail, byte[] ticketPdf,
                                      string movieTitle, string customerName)
        {
            try
            {
                // Настройки SMTP (замените на реальные)
                using (var smtpClient = new SmtpClient("smtp.yandex.ru")
                {
                    Port = 587,
                    Credentials = new NetworkCredential("your_email@yandex.ru", "your_password"), // Замените на ваши учетные данные
                    EnableSsl = true,
                })
                {
                    var mailMessage = new MailMessage
                    {
                        From = new MailAddress("your_email@yandex.ru"), // Замените на ваш email отправителя
                        Subject = $"Ваши билеты на фильм {movieTitle}",
                        Body = $"Уважаемый(ая) {customerName},\n\nВаши билеты прикреплены к этому письму.\n\nПриятного просмотра!",
                        IsBodyHtml = false,
                    };

                    mailMessage.To.Add(recipientEmail);

                    using (var pdfStream = new MemoryStream(ticketPdf))
                    {
                        mailMessage.Attachments.Add(new Attachment(pdfStream, $"Билет_{movieTitle}.pdf"));
                        smtpClient.Send(mailMessage);
                    }
                }
            }
            catch (Exception ex)
            {
                // Для отладки: выведите сообщение об ошибке в консоль или лог
                Console.WriteLine($"Ошибка при отправке email: {ex.Message}");
                throw new Exception("Ошибка при отправке email: " + ex.Message);
            }
        }
    }
}
