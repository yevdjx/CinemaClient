using System;
using System.IO;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using System.Windows.Forms;
using iTextSharp.text;
using iTextSharp.text.pdf;
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

        public byte[] GeneratePdfTicket(UserTicketDto t)
        {
            using var ms = new MemoryStream();

            // Горизонтальный A5 (как реальный билет)
            var doc = new Document(PageSize.A5.Rotate(), 25, 25, 20, 20);
            var writer = PdfWriter.GetInstance(doc, ms);
            doc.Open();
            // Путь к шрифту
            var fontPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Resources", "DejaVuSans.ttf");

            // Создаём BaseFont
            var baseF = BaseFont.CreateFont(fontPath, BaseFont.IDENTITY_H, BaseFont.EMBEDDED);

            // Создаём Font'ы
            var titleF = new iTextFont(baseF, 28, iTextFont.BOLD, BaseColor.White);
            var boldF = new iTextFont(baseF, 12, iTextFont.BOLD);
            var regF = new iTextFont(baseF, 12, iTextFont.NORMAL);
            var smallF = new iTextFont(baseF, 10, iTextFont.NORMAL);
            var headerBg = new BaseColor(220, 20, 60);   // красный (Crimson)
            var headYel = new BaseColor(255, 215, 0);   // ярко-жёлтый (шапка таблицы)
            var cellYel = new BaseColor(255, 239, 153); // бледно-жёлтый (данные)

            /* ---------- Шапка «БИЛЕТ» ------------------------------------ */
            var headerTbl = new PdfPTable(1) { WidthPercentage = 100, SpacingAfter = 8 };
            headerTbl.AddCell(new PdfPCell(new Phrase("БИЛЕТ", titleF))
            {
                BackgroundColor = headerBg,
                HorizontalAlignment = Element.ALIGN_CENTER,
                PaddingTop = 10,
                PaddingBottom = 10,
                Border = iTextRectangle.NO_BORDER
            });

            /* ---------- Таблица с данными -------------------------------- */
            var dataTbl = new PdfPTable(7)
            {
                WidthPercentage = 100,
                SpacingBefore = 5,
                SpacingAfter = 10
            };
            dataTbl.SetWidths(new float[] { 3f, 2f, 2f, 1.2f, 1.2f, 1.2f, 2f });

            void AddHead(string txt) => dataTbl.AddCell(new PdfPCell(new Phrase(txt, boldF))
            {
                BackgroundColor = headYel,
                HorizontalAlignment = Element.ALIGN_CENTER,
                VerticalAlignment = Element.ALIGN_MIDDLE,
                Padding = 4
            });

            void AddCell(string txt) => dataTbl.AddCell(new PdfPCell(new Phrase(txt, regF))
            {
                BackgroundColor = cellYel,
                HorizontalAlignment = Element.ALIGN_CENTER,
                VerticalAlignment = Element.ALIGN_MIDDLE,
                Padding = 4
            });

            // строка шапки
            foreach (var h in new[] { "ФИЛЬМ", "ДАТА", "ВРЕМЯ", "ЗАЛ", "РЯД", "МЕСТО", "ЦЕНА" })
                AddHead(h);

            // строка с данными
            AddCell(t.MovieTitle);
            AddCell(t.SessionDateTime.ToString("dd.MM.yyyy"));
            AddCell(t.SessionDateTime.ToString("HH:mm"));
            AddCell(t.HallNumber.ToString());
            AddCell(t.Row.ToString());
            AddCell(t.Seat.ToString());
            AddCell($"{t.Price} руб.");

            /* ---------- Лого при наличии (уменьшаем до 60px) ------------- */
            PdfPCell logoCell = null;
            if (t.MovieImage?.Length > 0)
            {
                try
                {
                    var img = iTextImage.GetInstance(t.MovieImage);
                    img.ScaleToFit(60, 60);
                    logoCell = new PdfPCell(img)
                    {
                        Border = iTextRectangle.NO_BORDER,
                        HorizontalAlignment = Element.ALIGN_LEFT,
                        PaddingRight = 5
                    };
                }
                catch { /* игнорируем */ }
            }

            /* ---------- Сайд-бар «КОНТРОЛЬ» ------------------------------ */
            var redColor = new BaseColor(255, 0, 0);
            var controlCell = new PdfPCell(new Phrase("КОНТРОЛЬ",
                                new iTextFont(baseF, 14, iTextFont.BOLD, redColor)))
            {
                Rotation = 90,
                BackgroundColor = cellYel,
                HorizontalAlignment = Element.ALIGN_CENTER,
                VerticalAlignment = Element.ALIGN_MIDDLE,
                Border = iTextRectangle.NO_BORDER
            };

            /* ---------- Компоновка в одну широкую таблицу ---------------- */
            var outer = new PdfPTable(2) { WidthPercentage = 100 };
            outer.SetWidths(new float[] { 12f, 1.2f });

            // первая колонка (внутри — лого, шапка, таблица, футер)
            var inner = new PdfPTable(1) { WidthPercentage = 100 };
            if (logoCell != null) inner.AddCell(logoCell);                // лого
            inner.AddCell(headerTbl);                                     // БИЛЕТ
            inner.AddCell(dataTbl);                                       // данные
            inner.AddCell(new PdfPCell(new Phrase("Кинотеатр AgroKino  •  www.agrocinema.ru", smallF))
            {
                Border = iTextRectangle.NO_BORDER,
                HorizontalAlignment = Element.ALIGN_CENTER,
                PaddingTop = 6
            });

            outer.AddCell(new PdfPCell(inner) { Border = iTextRectangle.NO_BORDER });
            outer.AddCell(controlCell);                                   // полоска КОНТРОЛЬ

            doc.Add(outer);
            doc.Close();
            return ms.ToArray();
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
                using (var smtpClient = new SmtpClient("smtp.mail.ru", 587))
                {
                    smtpClient.Credentials = new NetworkCredential("cate_cate_cate_cate_cate@mail.ru", "eQhMGgjFBYCUBzbUFpCR");
                    smtpClient.EnableSsl = true;
                    smtpClient.UseDefaultCredentials = false;
                    smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;

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
                               (ticketInfo.Status == "booked"
                                    ? "Внимание! Бронь действительна до 15 минут до начала сеанса.\n"
                                    : "") +
                               "Приятного просмотра!",
                        IsBodyHtml = false,
                    };

                    mailMessage.To.Add(email);

                    using (var stream = new MemoryStream(ticketPdf))
                    {
                        string attachmentName = ticketInfo.Status == "sold"
                            ? $"Билет_{ticketInfo.MovieTitle}_{ticketInfo.SessionDateTime:yyyyMMdd}.pdf"
                            : $"Бронь_{ticketInfo.MovieTitle}_{ticketInfo.SessionDateTime:yyyyMMdd}.pdf";

                        mailMessage.Attachments.Add(new Attachment(
                            stream,
                            attachmentName,
                            "application/pdf"));

                        // Асинхронная отправка
                        await smtpClient.SendMailAsync(mailMessage);
                    }
                }

                // Показываем MessageBox
                MessageBox.Show("Письмо успешно отправлено!", "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                // Показываем MessageBox при ошибке
                MessageBox.Show($"Ошибка отправки email: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                throw;
            }
        }
    }
}
