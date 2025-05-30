using System;
using System.IO;
using System.Net;
using System.Net.Mail;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.Collections.Generic;
using System.Linq;
using CinemaClient.Services;

using iTextFont = iTextSharp.text.Font;
using iTextImage = iTextSharp.text.Image;
using iTextRectangle = iTextSharp.text.Rectangle;

namespace CinemaClient.Services
{
    public class TicketService
    {
        private readonly ApiService _apiService;

        public TicketService(ApiService apiService)
        {
            _apiService = apiService;
        }

        public byte[] GeneratePdfTicket(UserTicketDto ticketInfo)
        {
            using (var memoryStream = new MemoryStream())
            {
                var pageSize = PageSize.A5.Rotate();
                var document = new Document(pageSize, 25, 25, 30, 30);
                PdfWriter.GetInstance(document, memoryStream);
                document.Open();

                try
                {
                    var baseFont = BaseFont.CreateFont(
                        BaseFont.HELVETICA,
                        BaseFont.CP1252,
                        BaseFont.NOT_EMBEDDED);

                    // Шрифты
                    var titleFont = new iTextFont(baseFont, 20, iTextFont.BOLD);
                    var headerFont = new iTextFont(baseFont, 16, iTextFont.BOLD);
                    var regularFont = new iTextFont(baseFont, 12, iTextFont.NORMAL);
                    var boldFont = new iTextFont(baseFont, 12, iTextFont.BOLD);
                    var smallFont = new iTextFont(baseFont, 10, iTextFont.NORMAL);

                    // Заголовок (разный для брони и билета)
                    string ticketType = ticketInfo.Status == "sold" ? "АГРОБИЛЕТ" : "АГРОБРОНЬ";
                    document.Add(new Paragraph(ticketType, titleFont)
                    {
                        Alignment = Element.ALIGN_CENTER,
                        SpacingAfter = 20
                    });

                    // Изображение фильма
                    if (ticketInfo.MovieImage != null && ticketInfo.MovieImage.Length > 0)
                    {
                        try
                        {
                            var movieImage = iTextImage.GetInstance(ticketInfo.MovieImage);
                            movieImage.ScaleToFit(200, 200);
                            movieImage.Alignment = Element.ALIGN_CENTER;
                            document.Add(movieImage);
                            document.Add(new Paragraph(" "));
                        }
                        catch { /* Игнорируем ошибки изображения */ }
                    }

                    // Название фильма
                    document.Add(new Paragraph(ticketInfo.MovieTitle, headerFont)
                    {
                        Alignment = Element.ALIGN_CENTER,
                        SpacingAfter = 15
                    });

                    // Таблица с информацией
                    var table = new PdfPTable(2)
                    {
                        WidthPercentage = 100,
                        SpacingBefore = 10f,
                        SpacingAfter = 20f
                    };

                    AddTableRow(table, "Дата сеанса:", ticketInfo.SessionDateTime.ToString("dd.MM.yyyy"), boldFont, regularFont);
                    AddTableRow(table, "Время сеанса:", ticketInfo.SessionDateTime.ToString("HH:mm"), boldFont, regularFont);
                    AddTableRow(table, "Зал:", ticketInfo.HallNumber, boldFont, regularFont);
                    AddTableRow(table, "Место:", $"Ряд {ticketInfo.Row}, Место {ticketInfo.Seat}", boldFont, regularFont);
                    AddTableRow(table, "Цена:", $"{ticketInfo.Price} руб.", boldFont, regularFont);
                    AddTableRow(table, "Статус:", ticketInfo.Status == "sold" ? "Оплачено" : "Забронировано", boldFont, regularFont);
                    AddTableRow(table, "ID билета:", ticketInfo.TicketId.ToString(), boldFont, smallFont);

                    document.Add(table);

                    // Предупреждение для брони
                    if (ticketInfo.Status == "booked")
                    {
                        document.Add(new Paragraph("Внимание! Бронь действительна до 15 минут до начала сеанса",
                            smallFont)
                        {
                            Alignment = Element.ALIGN_CENTER,
                            SpacingBefore = 10f
                        });
                    }

                    document.Add(new Paragraph("Предъявите этот билет на входе", smallFont)
                    {
                        Alignment = Element.ALIGN_CENTER,
                        SpacingBefore = 20f
                    });
                }
                finally
                {
                    document.Close();
                }
                return memoryStream.ToArray();
            }
        }

        private void AddTableRow(PdfPTable table, string label, string value, iTextFont labelFont, iTextFont valueFont)
        {
            table.AddCell(new PdfPCell(new Phrase(label, labelFont))
            {
                Border = iTextRectangle.NO_BORDER,
                HorizontalAlignment = Element.ALIGN_RIGHT,
                Padding = 5f
            });
            table.AddCell(new PdfPCell(new Phrase(value, valueFont))
            {
                Border = iTextRectangle.NO_BORDER,
                HorizontalAlignment = Element.ALIGN_LEFT,
                Padding = 5f
            });
        }

        public async Task SendEmailWithTicketAsync(string email, byte[] ticketPdf, UserTicketDto ticketInfo)
        {
            try
            {
                using (var smtpClient = new SmtpClient("smtp.mail.ru")
                {
                    Port = 465,
                    Credentials = new NetworkCredential("cate_cate_cate_cate_cate`@mail.ru", "eQhMGgjFBYCUBzbUFpCR"),
                    EnableSsl = true,
                })
                {
                    string ticketType = ticketInfo.Status == "sold" ? "билет" : "бронь";

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
                               (ticketInfo.Status == "booked" ?
                                "Внимание! Бронь действительна до 15 минут до начала сеанса.\n" : "") +
                               "Приятного просмотра!",
                        IsBodyHtml = false,
                    };

                    mailMessage.To.Add(email);

                    using (var stream = new MemoryStream(ticketPdf))
                    {
                        string attachmentName = ticketInfo.Status == "sold" ?
                            $"Билет_{ticketInfo.MovieTitle}_{ticketInfo.SessionDateTime:yyyyMMdd}.pdf" :
                            $"Бронь_{ticketInfo.MovieTitle}_{ticketInfo.SessionDateTime:yyyyMMdd}.pdf";

                        mailMessage.Attachments.Add(new Attachment(
                            stream,
                            attachmentName,
                            "application/pdf"));

                        // Асинхронная отправка!
                        await smtpClient.SendMailAsync(mailMessage);
                    }
                }

                Console.WriteLine("Письмо успешно отправлено.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка отправки email: {ex.Message}");
                throw;
            }
        }

    }
}