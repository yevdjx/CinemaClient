using System;
using System.IO;
using System.Net;
using System.Net.Mail;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.Configuration;
using System.Linq;
using System.Drawing;

namespace CinemaClient.Services
{
    public class TicketService
    {
//        private readonly ApiService _apiService;

//        public TicketService(ApiService apiService)
//        {
//            _apiService = apiService;
//        }

//        public byte[] GeneratePdfTicket(TicketInfo ticketInfo)
//        {
//            using (var memoryStream = new MemoryStream())
//            {
//                // Размер страницы A6 (подходит для билета)
//                var pageSize = new iTextSharp.text.Rectangle(288f, 432f); // 4x6 inches в points (1 inch = 72 points)
//                var document = new Document(pageSize, 15, 15, 15, 15);
//                PdfWriter.GetInstance(document, memoryStream);
//                document.Open();

//                try
//                {
//                    // Шрифты (поддержка кириллицы)
//                    var baseFont = BaseFont.CreateFont(
//                        Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Fonts), "arial.ttf",
//                        BaseFont.IDENTITY_H,
//                        BaseFont.EMBEDDED);

//                    var titleFont = new iTextSharp.text.Font(baseFont, 18, iTextSharp.text.Font.BOLD, new BaseColor(70, 70, 70));
//                    var headerFont = new iTextSharp.text.Font(baseFont, 14, iTextSharp.text.Font.BOLD, BaseColor.BLACK);
//                    var normalFont = new iTextSharp.text.Font(baseFont, 12, iTextSharp.text.Font.NORMAL, BaseColor.BLACK);
//                    var smallFont = new iTextSharp.text.Font(baseFont, 10, iTextSharp.text.Font.NORMAL, new BaseColor(128, 128, 128));

//                    // Логотип кинотеатра (опционально)
//                    string logoPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "logo.png");
//                    if (File.Exists(logoPath))
//                    {
//                        var logo = iTextSharp.text.Image.GetInstance(logoPath);
//                        logo.ScaleToFit(100, 50);
//                        logo.Alignment = Element.ALIGN_CENTER;
//                        document.Add(logo);
//                    }

//                    // Заголовок
//                    var title = new Paragraph("ЭЛЕКТРОННЫЙ БИЛЕТ", titleFont)
//                    {
//                        Alignment = Element.ALIGN_CENTER,
//                        SpacingAfter = 15
//                    };
//                    document.Add(title);

//                    // Информация о фильме
//                    var movieSection = new Paragraph("ФИЛЬМ", headerFont)
//                    {
//                        SpacingBefore = 10,
//                        SpacingAfter = 5
//                    };
//                    document.Add(movieSection);

//                    // Постер фильма
//                    if (ticketInfo.MovieImage != null && ticketInfo.MovieImage.Length > 0)
//                    {
//                        var movieImage = iTextSharp.text.Image.GetInstance(ticketInfo.MovieImage);
//                        movieImage.ScaleToFit(150, 200);
//                        movieImage.Alignment = Element.ALIGN_CENTER;
//                        document.Add(movieImage);
//                    }

//                    var movieTitle = new Paragraph(ticketInfo.MovieTitle, normalFont)
//                    {
//                        Alignment = Element.ALIGN_CENTER,
//                        SpacingAfter = 10
//                    };
//                    document.Add(movieTitle);

//                    // Информация о сеансе
//                    var sessionSection = new Paragraph("СЕАНС", headerFont)
//                    {
//                        SpacingBefore = 10,
//                        SpacingAfter = 5
//                    };
//                    document.Add(sessionSection);

//                    var sessionTable = new PdfPTable(2)
//                    {
//                        WidthPercentage = 100,
//                        SpacingAfter = 10
//                    };
//                    sessionTable.DefaultCell.Border = Rectangle.NO_BORDER;

//                    sessionTable.AddCell(new Phrase("Дата:", normalFont));
//                    sessionTable.AddCell(new Phrase(ticketInfo.SessionDate.ToString("dd.MM.yyyy"), normalFont));
//                    sessionTable.AddCell(new Phrase("Время:", normalFont));
//                    sessionTable.AddCell(new Phrase(ticketInfo.SessionTime.ToString("HH:mm"), normalFont));
//                    sessionTable.AddCell(new Phrase("Зал:", normalFont));
//                    sessionTable.AddCell(new Phrase(ticketInfo.HallNumber.ToString(), normalFont));
//                    document.Add(sessionTable);

//                    // Информация о местах
//                    var seatsSection = new Paragraph("МЕСТА", headerFont)
//                    {
//                        SpacingBefore = 10,
//                        SpacingAfter = 5
//                    };
//                    document.Add(seatsSection);

//                    var seatsTable = new PdfPTable(2)
//                    {
//                        WidthPercentage = 100,
//                        SpacingAfter = 10
//                    };
//                    seatsTable.DefaultCell.Border = Rectangle.NO_BORDER;

//                    foreach (var seat in ticketInfo.Seats)
//                    {
//                        seatsTable.AddCell(new Phrase($"Ряд {seat.Row}, Место {seat.Number}:", normalFont));
//                        seatsTable.AddCell(new Phrase($"{ticketInfo.PricePerSeat} руб.", normalFont));
//                    }
//                    document.Add(seatsTable);

//                    // Итого
//                    var total = new Paragraph($"ИТОГО: {ticketInfo.TotalPrice} руб.",
//                        new iTextSharp.text.Font(baseFont, 14, iTextSharp.text.Font.BOLD, new BaseColor(255, 0, 0)))
//                    {
//                        Alignment = Element.ALIGN_RIGHT,
//                        SpacingBefore = 10
//                    };
//                    document.Add(total);

//                    // ID билета (скрыто внизу)
//                    var ticketId = new Paragraph($"ID: {ticketInfo.TicketId}", smallFont)
//                    {
//                        Alignment = Element.ALIGN_CENTER,
//                        SpacingBefore = 20
//                    };
//                    document.Add(ticketId);

//                    // Предупреждение
//                    var warning = new Paragraph("* Предъявите этот билет на входе. Приятного просмотра!", smallFont)
//                    {
//                        Alignment = Element.ALIGN_CENTER,
//                        SpacingBefore = 10
//                    };
//                    document.Add(warning);
//                }
//                finally
//                {
//                    document.Close();
//                }
//                return memoryStream.ToArray();
//            }
//        }

//        public void SendEmailWithTicket(string recipientEmail, byte[] ticketPdf, TicketInfo ticketInfo)
//        {
//            SmtpClient smtpClient = null;
//            MailMessage mailMessage = null;

//            try
//            {
//                // Настройки SMTP из конфига
//                var smtpHost = ConfigurationManager.AppSettings["SmtpHost"];
//                var smtpPort = int.Parse(ConfigurationManager.AppSettings["SmtpPort"]);
//                var smtpUsername = ConfigurationManager.AppSettings["SmtpUsername"];
//                var smtpPassword = ConfigurationManager.AppSettings["SmtpPassword"];
//                var smtpFromEmail = ConfigurationManager.AppSettings["SmtpFromEmail"];
//                var smtpFromName = ConfigurationManager.AppSettings["SmtpFromName"];
//                var smtpEnableSsl = bool.Parse(ConfigurationManager.AppSettings["SmtpEnableSsl"]);

//                // Создаем SMTP клиент
//                smtpClient = new SmtpClient(smtpHost)
//                {
//                    Port = smtpPort,
//                    Credentials = new NetworkCredential(smtpUsername, smtpPassword),
//                    EnableSsl = smtpEnableSsl
//                };

//                // Создаем сообщение
//                mailMessage = new MailMessage
//                {
//                    From = new MailAddress(smtpFromEmail, smtpFromName),
//                    Subject = $"Ваш билет на фильм «{ticketInfo.MovieTitle}»",
//                    Body = $@"Уважаемый зритель!

//                    Ваши билеты на фильм «{ticketInfo.MovieTitle}» прикреплены к этому письму.

//                    Детали сеанса:
//                    Дата: {ticketInfo.SessionDate:dd.MM.yyyy}
//                    Время: {ticketInfo.SessionTime:HH:mm}
//                    Зал: {ticketInfo.HallNumber}
//                    Места: {string.Join(", ", ticketInfo.Seats.Select(s => $"Ряд {s.Row}, Место {s.Number}"))}
//                    Сумма: {ticketInfo.TotalPrice} руб.

//                    Пожалуйста, сохраните это письмо до посещения кинотеатра.
//                    Приятного просмотра!

//                    С уважением,
//                    Кинотеатр «»",
//                    IsBodyHtml = false
//                };

//                mailMessage.To.Add(recipientEmail);

//                // Добавляем PDF вложение
//                using (var pdfStream = new MemoryStream(ticketPdf))
//                {
//                    mailMessage.Attachments.Add(new Attachment(
//                        pdfStream,
//                        $"Билет_{ticketInfo.MovieTitle}_{ticketInfo.SessionDate:yyyyMMdd}.pdf",
//                        "application/pdf"));

//                    smtpClient.Send(mailMessage);
//                }
//            }
//            catch (Exception ex)
//            {
//                throw new ApplicationException("Не удалось отправить билеты на email. Пожалуйста, проверьте ваш email адрес и повторите попытку.", ex);
//            }
//            finally
//            {
//                mailMessage?.Dispose();
//                smtpClient?.Dispose();
//            }
//        }
    }

//    public class TicketInfo
//    {
//        public string TicketId { get; set; } = Guid.NewGuid().ToString("N").ToUpper();
//        public string MovieTitle { get; set; }
//        public byte[] MovieImage { get; set; }
//        public DateTime SessionDate { get; set; }
//        public TimeSpan SessionTime { get; set; }
//        public int HallNumber { get; set; }
//        public SeatInfo[] Seats { get; set; }
//        public decimal PricePerSeat { get; set; }
//        public decimal TotalPrice => Seats.Length * PricePerSeat;
//    }

//    public class SeatInfo
//    {
//        public int Row { get; set; }
//        public int Number { get; set; }
//    }
}