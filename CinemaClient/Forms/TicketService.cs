using System;
using System.IO;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using System.Windows.Forms;
using iTextSharp.text;
using iTextSharp.text.pdf;
using CinemaClient.Services;

// второе название для длинных имен классов iTextSharp
using iTextFont = iTextSharp.text.Font;
using iTextImage = iTextSharp.text.Image;
using iTextRectangle = iTextSharp.text.Rectangle;

namespace CinemaClient.Services
{
    public class TicketService
    {
        private readonly ApiService _apiService; // Сервис для работы с API

        // Конструктор, принимающий ApiService для взаимодействия с API
        public TicketService(ApiService apiService)
        {
            _apiService = apiService;
        }

        // Генерация PDF-билета на основе данных UserTicketDto
        public byte[] GeneratePdfTicket(UserTicketDto t)
        {
            using var ms = new MemoryStream(); // Поток для хранения PDF в памяти

            // Создаем документ формата A5 в горизонтальной ориентации с отступами по 25/20
            var doc = new Document(PageSize.A5.Rotate(), 25, 25, 20, 20);
            var writer = PdfWriter.GetInstance(doc, ms); // Связываем документ с потоком
            doc.Open(); // Открываем документ для записи

            // Путь к файлу шрифта
            var fontPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Resources", "DejaVuSans.ttf");

            // Создаем базовый шрифт с поддержкой кириллицы
            var baseF = BaseFont.CreateFont(fontPath, BaseFont.IDENTITY_H, BaseFont.EMBEDDED);

            // Создаем набор шрифтов разных стилей:
            var titleF = new iTextFont(baseF, 28, iTextFont.BOLD, BaseColor.White); // Заголовок
            var boldF = new iTextFont(baseF, 12, iTextFont.BOLD); // Жирный 
            var regF = new iTextFont(baseF, 12, iTextFont.NORMAL); // Обычный
            var smallF = new iTextFont(baseF, 10, iTextFont.NORMAL); // Мелкий

            // Цвета для оформления 
            var headerBg = new BaseColor(220, 20, 60);   // красный (Crimson)
            var headYel = new BaseColor(255, 215, 0);   // ярко-жёлтый (шапка таблицы)
            var cellYel = new BaseColor(255, 239, 153); // бледно-жёлтый (данные)

            // Шапка БИЛЕТ
            var headerTbl = new PdfPTable(1) { WidthPercentage = 100, SpacingAfter = 8 };
            headerTbl.AddCell(new PdfPCell(new Phrase("БИЛЕТ", titleF))
            {
                BackgroundColor = headerBg, // Красный фон
                HorizontalAlignment = Element.ALIGN_CENTER, // Вырывания по центру
                PaddingTop = 10, // Отступы сверху/снизу
                PaddingBottom = 10, 
                Border = iTextRectangle.NO_BORDER // Без рамки
            });

            // Таблица с данными 
            var dataTbl = new PdfPTable(7) // 7 кнопок 
            {
                WidthPercentage = 100, // Ширина
                SpacingBefore = 5, // ОТступы
                SpacingAfter = 10
            };
            // Устанавливаем относительные ширины колонок
            dataTbl.SetWidths(new float[] { 3f, 2f, 2f, 1.2f, 1.2f, 1.2f, 2f });

            // Локальная функция для добавления заголовков таблицы
            void AddHead(string txt) => dataTbl.AddCell(new PdfPCell(new Phrase(txt, boldF))
            {
                BackgroundColor = headYel, // Желтый фон
                HorizontalAlignment = Element.ALIGN_CENTER,
                VerticalAlignment = Element.ALIGN_MIDDLE,
                Padding = 4
            });

            // Локальная функция для добавления ячеек с данными
            void AddCell(string txt) => dataTbl.AddCell(new PdfPCell(new Phrase(txt, regF))
            {
                BackgroundColor = cellYel, // СВетло-желтый фон
                HorizontalAlignment = Element.ALIGN_CENTER,
                VerticalAlignment = Element.ALIGN_MIDDLE,
                Padding = 4
            });

            // Заголовки столбцов
            foreach (var h in new[] { "ФИЛЬМ", "ДАТА", "ВРЕМЯ", "ЗАЛ", "РЯД", "МЕСТО", "ЦЕНА" })
                AddHead(h);

            // Добавляем данные билета
            AddCell(t.MovieTitle);
            AddCell(t.SessionDateTime.ToString("dd.MM.yyyy"));
            AddCell(t.SessionDateTime.ToString("HH:mm"));
            AddCell(t.HallNumber.ToString());
            AddCell(t.Row.ToString());
            AddCell(t.Seat.ToString());
            AddCell($"{t.Price} руб.");

            // Лого при наличии (уменьшаем до 60px) 
            PdfPCell logoCell = null;
            if (t.MovieImage?.Length > 0) // Если есть изображение фильма
            {
                try
                {
                    var img = iTextImage.GetInstance(t.MovieImage); // Создаем изображение
                    img.ScaleToFit(60, 60); // Масштабируем
                    logoCell = new PdfPCell(img)
                    {
                        Border = iTextRectangle.NO_BORDER,
                        HorizontalAlignment = Element.ALIGN_LEFT,
                        PaddingRight = 5
                    };
                }
                catch { /* Игнорируем ошибки */ }
            }

            // Сайд-бар «КОНТРОЛЬ» 
            var redColor = new BaseColor(255, 0, 0);
            var controlCell = new PdfPCell(new Phrase("КОНТРОЛЬ",
                                new iTextFont(baseF, 14, iTextFont.BOLD, redColor)))
            {
                Rotation = 90,  // Поворачиваем текст на 90 градусов
                BackgroundColor = cellYel,
                HorizontalAlignment = Element.ALIGN_CENTER,
                VerticalAlignment = Element.ALIGN_MIDDLE,
                Border = iTextRectangle.NO_BORDER
            };

            // Компоновка в одну широкую таблицу 
            var outer = new PdfPTable(2) { WidthPercentage = 100 };
            outer.SetWidths(new float[] { 12f, 1.2f }); // Основное содержимое + узкая колонка "КОНТРОЛЬ"

            // Внутренняя таблица для основного содержимого
            var inner = new PdfPTable(1) { WidthPercentage = 100 };
            if (logoCell != null) inner.AddCell(logoCell); // Лого
            inner.AddCell(headerTbl); // Шапка "БИЛЕТ"
            inner.AddCell(dataTbl); // Таблица с даннымм
            // Футер с информацией о кинотеатре
            inner.AddCell(new PdfPCell(new Phrase("Кинотеатр AgroKino  •  www.agrocinema.ru", smallF))
            {
                Border = iTextRectangle.NO_BORDER,
                HorizontalAlignment = Element.ALIGN_CENTER,
                PaddingTop = 6
            });

            // Добавляем внутреннюю таблицу и колонку "КОНТРОЛЬ" во внешнюю таблицу
            outer.AddCell(new PdfPCell(inner) { Border = iTextRectangle.NO_BORDER });
            outer.AddCell(controlCell); // полоска "КОНТРОЛЬ"

            // Добавляем внешнюю таблицу в документ
            doc.Add(outer);
            doc.Close(); // Закрываем документ
            return ms.ToArray(); // Возвращаем PDF как массив байтов
        }

        // Асинхронная отправка билета по email
        public async Task SendEmailWithTicketAsync(string email, byte[] ticketPdf, UserTicketDto ticketInfo)
        {
            try
            {
                using (var smtpClient = new SmtpClient("smtp.mail.ru", 587)) // SMTP-клиент для mail.ru
                {
                    // Учетные данные для отправки
                    smtpClient.Credentials = new NetworkCredential("cate_cate_cate_cate_cate@mail.ru", "eQhMGgjFBYCUBzbUFpCR");
                    smtpClient.EnableSsl = true; // Используем SSL
                    smtpClient.UseDefaultCredentials = false;
                    smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;

                    // Определяем тип билета (билет или бронь)
                    string ticketType = ticketInfo.Status == "sold" ? "билет" : "бронь";

                    // Создаем email сообщение
                    var mailMessage = new MailMessage
                    {
                        From = new MailAddress("cate_cate_cate_cate_cate@mail.ru", "AgroKino"),
                        Subject = $"Ваш {ticketType} на фильм {ticketInfo.MovieTitle}",
                        Body = $"Уважаемый зритель!\n\n" +
                               $"Ваш {ticketType} на фильм «{ticketInfo.MovieTitle}» прикреплен к этому письму.\n\n" +
                               $"Дата: {ticketInfo.SessionDateTime:dd.MM.yyyy}\n" +
                               $"Время: {ticketInfo.SessionDateTime:HH:mm}\n" +
                               $"Зал: {ticketInfo.HallNumber}\n" +
                               $"Место: Ряд {ticketInfo.Row}, Место {ticketInfo.Seat}\n" +
                               $"Статус: {(ticketInfo.Status == "sold" ? "Оплачено" : "Забронировано")}\n\n" +
                               (ticketInfo.Status == "booked"
                                    ? "Внимание! Бронь действительна до 15 минут до начала сеанса.\n"
                                    : "") +
                               "Приятного просмотра!",
                        IsBodyHtml = false, // Текст без HTML
                    };

                    mailMessage.To.Add(email); // Добавляем получателя

                    // Добавляем PDF-вложение
                    using (var stream = new MemoryStream(ticketPdf))
                    {
                        // Формируем имя файла в зависимости от типа билета
                        string attachmentName = ticketInfo.Status == "sold"
                            ? $"Билет_{ticketInfo.MovieTitle}_{ticketInfo.SessionDateTime:yyyyMMdd}.pdf"
                            : $"Бронь_{ticketInfo.MovieTitle}_{ticketInfo.SessionDateTime:yyyyMMdd}.pdf";

                        mailMessage.Attachments.Add(new Attachment(
                            stream,
                            attachmentName,
                            "application/pdf")); // MIME-тип PDF

                        // Асинхронная отправка письма
                        await smtpClient.SendMailAsync(mailMessage);
                    }
                }

                // Показываем сообщение об успешной отправке
                MessageBox.Show("Письмо успешно отправлено!", "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                // Показываем сообщение об ошибке
                MessageBox.Show($"Ошибка отправки email: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                throw; // Пробрасываем исключение дальше
            }
        }
    }
}
