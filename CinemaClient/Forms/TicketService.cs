using System;
using System.IO;
using System.Net;
using System.Net.Mail;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.Configuration;
using System.Collections.Generic;
using System.Linq;
using CinemaClient.Services;

// Убедитесь, что есть явные ссылки на пространства имен
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

        public byte[] GeneratePdfTicket(TicketInfo ticketInfo)
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

                    // Шрифты с явным указанием пространства имен
                    var titleFont = new iTextFont(baseFont, 20, iTextFont.BOLD);
                    var headerFont = new iTextFont(baseFont, 16, iTextFont.BOLD);
                    var regularFont = new iTextFont(baseFont, 12, iTextFont.NORMAL);
                    var boldFont = new iTextFont(baseFont, 12, iTextFont.BOLD);
                    var smallFont = new iTextFont(baseFont, 10, iTextFont.NORMAL);

                    // Заголовок
                    document.Add(new Paragraph("КИНОБИЛЕТ", titleFont)
                    {
                        Alignment = Element.ALIGN_CENTER,
                        SpacingAfter = 20
                    });

                    // Картинка фильма
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

                    // Информация о фильме
                    document.Add(new Paragraph(ticketInfo.MovieTitle, headerFont)
                    {
                        Alignment = Element.ALIGN_CENTER,
                        SpacingAfter = 15
                    });

                    // Таблица с деталями
                    var table = new PdfPTable(2)
                    {
                        WidthPercentage = 100,
                        SpacingBefore = 10f, // Явное указание float
                        SpacingAfter = 20f     // Явное указание float
                    };

                    AddTableRow(table, "Дата сеанса:", ticketInfo.SessionDate.ToString("dd.MM.yyyy"), boldFont, regularFont);
                    AddTableRow(table, "Время сеанса:", ticketInfo.SessionTime, boldFont, regularFont);
                    AddTableRow(table, "Зал:", ticketInfo.HallNumber, boldFont, regularFont);

                    foreach (var seat in ticketInfo.Seats)
                    {
                        AddTableRow(table, "Место:", $"Ряд {seat.Row}, Место {seat.Number}", boldFont, regularFont);
                    }

                    AddTableRow(table, "Цена:", $"{ticketInfo.TotalPrice} руб.", boldFont, regularFont);
                    AddTableRow(table, "ID билета:", ticketInfo.TicketId.ToString(), boldFont, smallFont);

                    document.Add(table);

                    // Подпись
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

        public void SendEmailWithTicket(string email, byte[] ticketPdf, TicketInfo ticketInfo)
        {
            try
            {
                using (var smtpClient = new SmtpClient("smtp.gmail.com")
                {
                    Port = 587,
                    Credentials = new NetworkCredential("your-cinema-email@gmail.com", "your-password"),
                    EnableSsl = true,
                })
                {
                    var mailMessage = new MailMessage
                    {
                        From = new MailAddress("your-cinema-email@gmail.com", "Кинотеатр"),
                        Subject = $"Ваш билет на фильм {ticketInfo.MovieTitle}",
                        Body = $"Уважаемый зритель!\n\n" +
                               $"Ваш билет на фильм «{ticketInfo.MovieTitle}» прикреплен к этому письму.\n\n" +
                               $"Дата: {ticketInfo.SessionDate:dd.MM.yyyy}\n" +
                               $"Время: {ticketInfo.SessionTime}\n" +
                               $"Зал: {ticketInfo.HallNumber}\n" +
                               $"Места: {string.Join(", ", ticketInfo.Seats.Select(s => $"Ряд {s.Row}, Место {s.Number}"))}\n\n" +
                               "Приятного просмотра!",
                        IsBodyHtml = false,
                    };

                    mailMessage.To.Add(email);

                    using (var stream = new MemoryStream(ticketPdf))
                    {
                        mailMessage.Attachments.Add(new Attachment(
                            stream,
                            $"Билет_{ticketInfo.MovieTitle}_{DateTime.Now:yyyyMMdd}.pdf",
                            "application/pdf"));

                        smtpClient.Send(mailMessage);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка отправки email: {ex.Message}");
                throw;
            }
        }
    }

    public class TicketInfo
    {
        public Guid TicketId { get; set; } = Guid.NewGuid();
        public string MovieTitle { get; set; }
        public byte[] MovieImage { get; set; }
        public DateTime SessionDate { get; set; }
        public string SessionTime { get; set; }
        public string HallNumber { get; set; }
        public List<SeatDto> Seats { get; set; }
        public decimal TotalPrice { get; set; }
    }
}