using CinemaClient.Services;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace CinemaClient.Forms
{
    public partial class KorzinaForm : Form
    {
        private readonly ApiService _api;
        private readonly TicketService _ticketService;
        private List<UserTicketDto> _userTickets;

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

            // Основной контейнер с прокруткой
            var mainPanel = new Panel
            {
                Dock = DockStyle.Fill,
                AutoScroll = true,
                AutoSize = false // Важно отключить AutoSize
            };
            this.Controls.Add(mainPanel);

            // Внутренний контейнер для содержимого
            var contentPanel = new Panel
            {
                Width = mainPanel.ClientSize.Width - 20, // Оставляем место для скроллбара
                AutoSize = true // Включаем AutoSize для автоматического определения высоты
            };
            mainPanel.Controls.Add(contentPanel);

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
            contentPanel.Controls.Add(titleLabel);

            // Группировка билетов
            var groupPanel = new Panel
            {
                Dock = DockStyle.Fill,
                AutoSize = true // Включаем AutoSize
            };
            contentPanel.Controls.Add(groupPanel);
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
            var mainPanel = (Panel)this.Controls[0];
            var contentPanel = (Panel)mainPanel.Controls[0];
            var groupPanel = (Panel)contentPanel.Controls[1]; // Теперь groupPanel - второй элемент в contentPanel
            groupPanel.Controls.Clear();

            if (!_userTickets.Any())
            {
                var emptyLabel = new Label
                {
                    Text = "У вас пока нет билетов",
                    Dock = DockStyle.Fill,
                    TextAlign = ContentAlignment.MiddleCenter,
                    Font = new Font("Bahnschrift", 14),
                    ForeColor = Color.Gray
                };
                groupPanel.Controls.Add(emptyLabel);
                return;
            }

            // Группируем по статусу (будущие/прошедшие)
            var now = DateTime.Now;
            var futureTickets = _userTickets.Where(t => t.Flag == 1).OrderBy(t => t.SessionDateTime).ToList();
            var pastTickets = _userTickets.Where(t => t.Flag == 0).OrderByDescending(t => t.SessionDateTime).ToList();

            int yPos = 10;

            // Будущие сеансы
            if (futureTickets.Any())
            {
                var futureLabel = CreateGroupLabel("Будущие сеансы", yPos);
                groupPanel.Controls.Add(futureLabel);
                yPos += 40;

                foreach (var ticket in futureTickets)
                {
                    var card = CreateTicketCard(ticket, true);
                    card.Location = new Point(20, yPos);
                    groupPanel.Controls.Add(card);
                    yPos += card.Height + 10;
                }
                yPos += 20;
            }

            // Прошедшие сеансы
            if (pastTickets.Any())
            {
                var pastLabel = CreateGroupLabel("Прошедшие сеансы", yPos);
                groupPanel.Controls.Add(pastLabel);
                yPos += 40;

                foreach (var ticket in pastTickets)
                {
                    var card = CreateTicketCard(ticket, false);
                    card.Location = new Point(20, yPos);
                    groupPanel.Controls.Add(card);
                    yPos += card.Height + 10;
                }
            }
        }

        private Label CreateGroupLabel(string text, int yPos)
        {
            return new Label
            {
                Text = text,
                Location = new Point(20, yPos),
                AutoSize = true,
                Font = new Font("Bahnschrift", 12, FontStyle.Bold),
                ForeColor = Color.Maroon
            };
        }

        private Panel CreateTicketCard(UserTicketDto ticket, bool isFuture)
        {
            var card = new Panel
            {
                Width = 730,
                Height = 150,
                BackColor = isFuture ? Color.MistyRose : Color.Lavender,
                BorderStyle = BorderStyle.FixedSingle,
                Padding = new Padding(10)
            };

            // Изображение фильма
            if (ticket.MovieImage != null && ticket.MovieImage.Length > 0)
            {
                try
                {
                    using (var ms = new MemoryStream(ticket.MovieImage))
                    {
                        var pictureBox = new PictureBox
                        {
                            Image = Image.FromStream(ms),
                            SizeMode = PictureBoxSizeMode.Zoom,
                            Size = new Size(100, 130),
                            Location = new Point(10, 10)
                        };
                        card.Controls.Add(pictureBox);
                    }
                }
                catch { /* Игнорируем ошибки загрузки изображения */ }
            }

            // Основная информация
            var infoLabel = new Label
            {
                Text = $"{ticket.MovieTitle}\n" +
                      $"Дата: {ticket.SessionDateTime:dd.MM.yyyy}\n" +
                      $"Время: {ticket.SessionDateTime:HH:mm}\n" +
                      $"Зал: {ticket.HallNumber}\n" +
                      $"Место: Ряд {ticket.Row}, Место {ticket.Seat}",
                Location = new Point(120, 10),
                AutoSize = true,
                Font = new Font("Bahnschrift", 10)
            };
            card.Controls.Add(infoLabel);

            // Статус и цена
            var statusLabel = new Label
            {
                Text = ticket.Status == "booked" ? "Бронь" : "Оплачено",
                Location = new Point(400, 10),
                AutoSize = true,
                Font = new Font("Bahnschrift", 10, FontStyle.Bold),
                ForeColor = ticket.Status == "booked" ? Color.Blue : Color.Green
            };
            card.Controls.Add(statusLabel);

            var priceLabel = new Label
            {
                Text = $"{ticket.Price} руб.",
                Location = new Point(400, 40),
                AutoSize = true,
                Font = new Font("Bahnschrift", 10, FontStyle.Bold)
            };
            card.Controls.Add(priceLabel);

            // Кнопка отправки только для будущих сеансов
            if (isFuture)
            {
                var sendButton = new Button
                {
                    Text = "Отправить билет",
                    Location = new Point(550, 80),
                    Size = new Size(150, 30),
                    BackColor = Color.DeepPink,
                    ForeColor = Color.White,
                    FlatStyle = FlatStyle.Flat,
                    Tag = ticket
                };
                sendButton.FlatStyle = FlatStyle.Standard;
                sendButton.FlatAppearance.BorderSize = 0;
                sendButton.Click += SendButton_Click;
                card.Controls.Add(sendButton);
            }

            return card;
        }

        private async void SendButton_Click(object sender, EventArgs e)
        {
            var button = (Button)sender;
            var ticket = (UserTicketDto)button.Tag;

            using (var inputForm = new Form())
            {
                inputForm.Text = "Отправить билет на email";
                inputForm.Size = new Size(350, 180);
                inputForm.StartPosition = FormStartPosition.CenterParent;
                inputForm.BackColor = Color.AntiqueWhite;
                inputForm.FormBorderStyle = FormBorderStyle.FixedDialog;

                var emailLabel = new Label
                {
                    Text = "Введите email:",
                    Location = new Point(20, 20),
                    AutoSize = true
                };
                inputForm.Controls.Add(emailLabel);

                var emailTextBox = new TextBox
                {
                    Location = new Point(20, 50),
                    Width = 300
                };
                inputForm.Controls.Add(emailTextBox);

                var sendButton = new Button
                {
                    Text = "Отправить",
                    Location = new Point(120, 100),
                    Size = new Size(100, 30),
                    BackColor = Color.DeepPink,
                    ForeColor = Color.White,
                    FlatStyle = FlatStyle.Flat,
                    DialogResult = DialogResult.OK
                };
                inputForm.Controls.Add(sendButton);
                inputForm.AcceptButton = sendButton;

                if (inputForm.ShowDialog() == DialogResult.OK && !string.IsNullOrWhiteSpace(emailTextBox.Text))
                {
                    try
                    {
                        var pdfBytes = _ticketService.GeneratePdfTicket(ticket);
                        await _ticketService.SendEmailWithTicketAsync(emailTextBox.Text, pdfBytes, ticket);

                        MessageBox.Show($"Билет на {ticket.MovieTitle} успешно отправлен!", "Успех",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Ошибка отправки: {ex.Message}", "Ошибка",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }
    }
}