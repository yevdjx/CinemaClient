using CinemaClient.Services;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CinemaClient.Forms
{
    public partial class KorzinaForm : Form
    {
        private readonly ApiService _api;
        private readonly TicketService _ticketService;
        private List<TicketDto> _userTickets;

        public KorzinaForm(ApiService api)
        {
            InitializeComponent();
            _api = api;
            _ticketService = new TicketService(api);
            SetupUI();
            LoadTickets();
        }

        private void SetupUI()
        {
            this.Text = "Мои билеты";
            this.BackColor = Color.AntiqueWhite;
            this.Size = new Size(800, 600);
            this.StartPosition = FormStartPosition.CenterScreen;

            // Основные элементы
            var mainPanel = new Panel { Dock = DockStyle.Fill };
            this.Controls.Add(mainPanel);

            // Заголовок
            var titleLabel = new Label
            {
                Text = "Мои билеты",
                Dock = DockStyle.Top,
                Font = new Font("Bahnschrift", 16, FontStyle.Bold),
                TextAlign = ContentAlignment.MiddleCenter,
                Height = 60,
                BackColor = Color.PaleVioletRed,
                ForeColor = Color.White
            };
            mainPanel.Controls.Add(titleLabel);

            // Список билетов
            var ticketsPanel = new FlowLayoutPanel
            {
                Dock = DockStyle.Fill,
                AutoScroll = true,
                BackColor = Color.AntiqueWhite,
                Padding = new Padding(20)
            };
            mainPanel.Controls.Add(ticketsPanel);

            // Панель для ввода email
            var emailPanel = new Panel
            {
                Dock = DockStyle.Bottom,
                Height = 100,
                BackColor = Color.MistyRose
            };
            mainPanel.Controls.Add(emailPanel);

            var emailLabel = new Label
            {
                Text = "Email для отправки билетов:",
                Location = new Point(20, 20),
                AutoSize = true
            };
            emailPanel.Controls.Add(emailLabel);

            var emailTextBox = new TextBox
            {
                Location = new Point(20, 50),
                Width = 300
            };
            emailPanel.Controls.Add(emailTextBox);

            var sendButton = new Button
            {
                Text = "Отправить билеты",
                Location = new Point(330, 50),
                BackColor = Color.DeepPink,
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };
            sendButton.Click += async (s, e) => await SendTickets(emailTextBox.Text);
            emailPanel.Controls.Add(sendButton);
        }

        private async void LoadTickets()
        {
            try
            {
                _userTickets = (await _api.GetUserTicketsAsync()).ToList();
                DisplayTickets();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки билетов: {ex.Message}", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void DisplayTickets()
        {
            var ticketsPanel = this.Controls[0].Controls.OfType<FlowLayoutPanel>().First();
            ticketsPanel.Controls.Clear();

            var now = DateTime.Now;

            foreach (var ticket in _userTickets.OrderBy(t => t.SessionDateTime))
            {
                var isExpired = ticket.SessionDateTime < now;
                var isBooked = ticket.Status == "booked";
                var isSold = ticket.Status == "sold";

                var card = new Panel
                {
                    Width = 700,
                    Height = 150,
                    Margin = new Padding(10),
                    BackColor = isExpired ? Color.LightGray :
                                isBooked ? Color.LightBlue :
                                Color.LightGreen,
                    BorderStyle = BorderStyle.FixedSingle
                };

                var movieLabel = new Label
                {
                    Text = ticket.MovieTitle,
                    Font = new Font("Bahnschrift", 12, FontStyle.Bold),
                    Location = new Point(10, 10),
                    AutoSize = true
                };
                card.Controls.Add(movieLabel);

                var sessionLabel = new Label
                {
                    Text = $"Дата: {ticket.SessionDateTime:dd.MM.yyyy}\n" +
                           $"Время: {ticket.SessionDateTime:HH:mm}\n" +
                           $"Зал: {ticket.HallNumber}",
                    Location = new Point(10, 40),
                    AutoSize = true
                };
                card.Controls.Add(sessionLabel);

                var seatsLabel = new Label
                {
                    Text = $"Места: {string.Join(", ", ticket.Seats.Select(s => $"{s.Row}/{s.Number}"))}",
                    Location = new Point(10, 80),
                    AutoSize = true
                };
                card.Controls.Add(seatsLabel);

                var statusLabel = new Label
                {
                    Text = isExpired ? "Просрочен" :
                          isBooked ? "Бронь (не оплачен)" : "Оплачен",
                    Location = new Point(500, 10),
                    AutoSize = true,
                    ForeColor = isExpired ? Color.Red : Color.Black
                };
                card.Controls.Add(statusLabel);

                var priceLabel = new Label
                {
                    Text = $"{ticket.Price} руб.",
                    Location = new Point(500, 40),
                    AutoSize = true,
                    Font = new Font("Bahnschrift", 10, FontStyle.Bold)
                };
                card.Controls.Add(priceLabel);

                if (isBooked && !isExpired)
                {
                    var payButton = new Button
                    {
                        Text = "Оплатить",
                        Location = new Point(500, 80),
                        Size = new Size(100, 30),
                        Tag = ticket.TicketId
                    };
                    payButton.Click += PayButton_Click;
                    card.Controls.Add(payButton);
                }

                ticketsPanel.Controls.Add(card);
            }
        }

        private async void PayButton_Click(object sender, EventArgs e)
        {
            var button = (Button)sender;
            var ticketId = (int)button.Tag;

            try
            {
                var result = await _api.ConfirmBookingAsync(ticketId);
                if (result)
                {
                    MessageBox.Show("Билет успешно оплачен!", "Успех",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadTickets(); // Обновляем список
                }
                else
                {
                    MessageBox.Show("Не удалось оплатить билет", "Ошибка",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка оплаты: {ex.Message}", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async Task SendTickets(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
            {
                MessageBox.Show("Введите email адрес", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                var now = DateTime.Now;
                var ticketsToSend = _userTickets
                    .Where(t => t.Status == "sold" && t.SessionDateTime > now)
                    .ToList();

                if (!ticketsToSend.Any())
                {
                    MessageBox.Show("Нет билетов для отправки", "Информация",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                foreach (var ticket in ticketsToSend)
                {
                    var ticketInfo = new TicketInfo
                    {
                        MovieTitle = ticket.MovieTitle,
                        MovieImage = ticket.MovieImage,
                        SessionDate = ticket.SessionDateTime.Date,
                        SessionTime = ticket.SessionDateTime.ToString("HH:mm"),
                        HallNumber = ticket.HallNumber,
                        Seats = ticket.Seats.ToList(),
                        TotalPrice = ticket.Price
                    };

                    var pdfBytes = _ticketService.GeneratePdfTicket(ticketInfo);
                    _ticketService.SendEmailWithTicket(email, pdfBytes, ticketInfo);
                }

                MessageBox.Show($"Билеты успешно отправлены на {email}", "Успех",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка отправки билетов: {ex.Message}", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}

public class TicketDto
{
    public int TicketId { get; set; }
    public string MovieTitle { get; set; }
    public byte[] MovieImage { get; set; }
    public DateTime SessionDateTime { get; set; }
    public string HallNumber { get; set; }
    public IEnumerable<SeatDto> Seats { get; set; }
    public decimal Price { get; set; }
    public string Status { get; set; } // "booked" или "sold"
}