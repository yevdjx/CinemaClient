using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using CinemaClient.Services;

namespace CinemaClient.Forms
{
    public partial class HallForm : Form
    {
        private readonly ApiService _api;
        private readonly int _sessionId;
        private readonly List<Button> _selectedSeats = new List<Button>();
        private TableLayoutPanel seatsTable;
        private Panel seatsContainer;
        private bool _isProcessing = false;

        public HallForm(ApiService api, int sessionId, string movieTitle)
        {
            InitializeComponent();
            _api = api;
            _sessionId = sessionId;
            Text = $"Бронирование билетов на фильм «{movieTitle}»";
            this.BackColor = Color.AntiqueWhite;
            this.ForeColor = Color.DeepPink;
            this.Font = new Font("Bahnschrift", 10, FontStyle.Bold);
            this.Size = new Size(800, 600);
            this.StartPosition = FormStartPosition.CenterScreen;
        }

        private async void HallForm_Load(object sender, EventArgs e)
        {
            var mainPanel = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = Color.AntiqueWhite
            };
            this.Controls.Add(mainPanel);

            var screenPanel = new Panel
            {
                Dock = DockStyle.Top,
                Height = 60,
                BackColor = Color.AntiqueWhite,
                Padding = new Padding(0, 10, 0, 0)
            };
            mainPanel.Controls.Add(screenPanel);

            var screenLabel = new Label
            {
                Text = "ЭКРАН",
                Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.BottomCenter,
                Font = new Font("Bahnschrift", 14, FontStyle.Bold),
                ForeColor = Color.DeepPink,
                BackColor = Color.AntiqueWhite
            };
            screenPanel.Controls.Add(screenLabel);

            var scrollPanel = new Panel
            {
                Dock = DockStyle.Fill,
                AutoScroll = true,
                BackColor = Color.AntiqueWhite,
                Padding = new Padding(20, 0, 20, 0)
            };
            mainPanel.Controls.Add(scrollPanel);

            seatsContainer = new Panel
            {
                AutoSize = true,
                AutoSizeMode = AutoSizeMode.GrowAndShrink,
                BackColor = Color.AntiqueWhite,
                MinimumSize = new Size(0, 300)
            };
            scrollPanel.Controls.Add(seatsContainer);

            var seats = (await _api.GetSeatsAsync(_sessionId)).ToList();
            int rows = seats.Max(s => s.Number);
            int cols = seats.Max(s => s.Row);

            seatsTable = new TableLayoutPanel
            {
                AutoSize = true,
                AutoSizeMode = AutoSizeMode.GrowAndShrink,
                ColumnCount = cols,
                RowCount = rows,
                BackColor = Color.AntiqueWhite,
                Margin = new Padding(0, 30, 0, 20)
            };

            int buttonSize = Math.Max(30, 400 / cols);
            for (int i = 0; i < cols; i++)
                seatsTable.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, buttonSize));
            for (int i = 0; i < rows; i++)
                seatsTable.RowStyles.Add(new RowStyle(SizeType.Absolute, buttonSize));

            seatsContainer.Controls.Add(seatsTable);
            CenterSeatsTable();

            foreach (var seat in seats)
            {
                var btn = new Button
                {
                    Text = $"{seat.Number}",
                    Size = new Size(buttonSize - 5, buttonSize - 5),
                    FlatStyle = FlatStyle.Flat,
                    Tag = seat,
                    Font = new Font("Bahnschrift", 8, FontStyle.Bold),
                    BackColor = Color.PaleVioletRed,
                    ForeColor = Color.Maroon,
                    FlatAppearance = { BorderColor = Color.DeepPink, BorderSize = 1 }
                };

                UpdateSeatButtonAppearance(btn);
                btn.Click += Seat_Click;
                seatsTable.Controls.Add(btn, seat.Number - 1, seat.Row - 1);
            }

            var legendPanel = new Panel
            {
                Dock = DockStyle.Bottom,
                Height = 60,
                BackColor = Color.AntiqueWhite
            };
            mainPanel.Controls.Add(legendPanel);

            var buyButton = new Button
            {
                Dock = DockStyle.Bottom,
                Height = 50,
                Text = "Купить выбранные места",
                BackColor = Color.DeepPink,
                ForeColor = Color.White,
                Font = new Font("Bahnschrift", 10, FontStyle.Bold),
                FlatStyle = FlatStyle.Flat,
                FlatAppearance = { BorderSize = 0 }
            };
            buyButton.Click += BuySelectedSeats_Click;
            mainPanel.Controls.Add(buyButton);

            var bookButton = new Button
            {
                Dock = DockStyle.Bottom,
                Height = 50,
                Text = "Забронировать выбранные места",
                BackColor = Color.DeepPink,
                ForeColor = Color.White,
                Font = new Font("Bahnschrift", 10, FontStyle.Bold),
                FlatStyle = FlatStyle.Flat,
                FlatAppearance = { BorderSize = 0 }
            };
            bookButton.Click += BookSelectedSeats_Click;
            mainPanel.Controls.Add(bookButton);

            AddLegendItem(legendPanel, "Свободно", Color.White, 20);
            AddLegendItem(legendPanel, "Выбрано", Color.MediumVioletRed, 180);
            AddLegendItem(legendPanel, "Бронь", Color.FromArgb(220, 160, 220), 340);
            AddLegendItem(legendPanel, "Куплено", Color.LightGray, 500);

            this.Resize += (s, ev) => CenterSeatsTable();
            scrollPanel.Resize += (s, ev) => CenterSeatsTable();
        }

        private void CenterSeatsTable()
        {
            if (seatsTable != null && seatsContainer != null)
            {
                seatsTable.Location = new Point(
                    (seatsContainer.ClientSize.Width - seatsTable.Width) / 2,
                    20);
            }
        }

        private void AddLegendItem(Panel parent, string text, Color color, int x)
        {
            var panel = new Panel
            {
                Location = new Point(x, 15),
                Size = new Size(150, 30),
                BackColor = Color.AntiqueWhite
            };
            var colorBox = new Panel
            {
                BackColor = color,
                Size = new Size(20, 20),
                Location = new Point(0, 5),
                BorderStyle = BorderStyle.FixedSingle
            };
            var label = new Label
            {
                Text = text,
                Location = new Point(25, 5),
                AutoSize = true,
                Font = new Font("Bahnschrift", 9),
                ForeColor = Color.DeepPink,
                BackColor = Color.AntiqueWhite
            };

            panel.Controls.Add(colorBox);
            panel.Controls.Add(label);
            parent.Controls.Add(panel);
        }

        private void Seat_Click(object sender, EventArgs e)
        {
            if (_isProcessing) return;

            var btn = (Button)sender;
            var seat = (SeatDto)btn.Tag;

            // Разрешаем взаимодействие только со свободными или забронированными местами
            if (seat.Status.ToLower() != "free" && seat.Status.ToLower() != "booked")
                return;

            if (_selectedSeats.Contains(btn))
            {
                _selectedSeats.Remove(btn);
                btn.BackColor = seat.Status.ToLower() == "booked"
                    ? Color.FromArgb(220, 160, 220)
                    : Color.White;
                btn.ForeColor = seat.Status.ToLower() == "booked"
                    ? Color.White
                    : Color.Maroon;
            }
            else
            {
                _selectedSeats.Add(btn);
                btn.BackColor = Color.MediumVioletRed;
                btn.ForeColor = Color.White;
            }
        }

        // Обновлённый метод бронирования
        private async void BookSelectedSeats_Click(object sender, EventArgs e)
        {
            if (_isProcessing) return;
            _isProcessing = true;

            try
            {
                if (_selectedSeats.Count == 0)
                {
                    MessageBox.Show("Выберите хотя бы одно место!", "Внимание",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                bool allBooked = true;
                foreach (var btn in _selectedSeats.ToList())
                {
                    var oldSeat = (SeatDto)btn.Tag;
                    if (oldSeat.Status == "free") // Бронируем только свободные места
                    {
                        if (await _api.BookAsync(oldSeat.TicketId) != 0)
                        {
                            allBooked = false;
                        }
                        else
                        {
                            var newSeat = oldSeat with { Status = "booked" };
                            btn.Tag = newSeat;
                            UpdateSeatButtonAppearance(btn);
                        }
                    }
                }

                if (allBooked)
                {
                    MessageBox.Show($"Успешно забронировано мест: {_selectedSeats.Count}", "Успех",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    _selectedSeats.Clear();
                }
                else
                {
                    MessageBox.Show("Некоторые места не удалось забронировать.", "Внимание",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    _selectedSeats.Clear();
                }
            }
            finally
            {
                _isProcessing = false;
            }
        }

        private async void BuySelectedSeats_Click(object sender, EventArgs e)
        {
            if (_isProcessing) return;
            _isProcessing = true;

            try
            {
                if (_selectedSeats.Count == 0)
                {
                    MessageBox.Show("Выберите хотя бы одно место!", "Внимание",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                bool allBought = true;
                foreach (var btn in _selectedSeats.ToList())
                {
                    var oldSeat = (SeatDto)btn.Tag;

                    // Пытаемся купить место
                    if (await _api.BuyAsync(oldSeat.TicketId) != 0)
                    {
                        allBought = false;

                        // Если не удалось купить - возвращаем первоначальный вид
                        if (oldSeat.Status == "booked")
                        {
                            btn.BackColor = Color.FromArgb(220, 160, 220); // Цвет брони
                            btn.ForeColor = Color.White;
                        }
                        else
                        {
                            btn.BackColor = Color.White; // Цвет свободного места
                            btn.ForeColor = Color.Maroon;
                        }
                    }
                    else
                    {
                        // Успешная покупка
                        var newSeat = oldSeat with { Status = "sold" };
                        btn.Tag = newSeat;
                        UpdateSeatButtonAppearance(btn);
                        _selectedSeats.Remove(btn);
                    }
                }

                if (allBought)
                {
                    MessageBox.Show($"Успешно куплено {_selectedSeats.Count} мест!", "Успех",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    _selectedSeats.Clear();
                }
                else
                {
                    MessageBox.Show("Некоторые места не удалось купить.", "Внимание",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            finally
            {
                _isProcessing = false;
            }
        }

        private void UpdateSeatButtonAppearance(Button btn)
        {
            var seat = (SeatDto)btn.Tag;

            switch (seat.Status.ToLower())
            {
                case "sold":
                    btn.BackColor = Color.LightGray;
                    btn.ForeColor = Color.DimGray;
                    btn.Enabled = false;
                    break;

                case "booked":
                    btn.BackColor = Color.FromArgb(220, 160, 220);
                    btn.ForeColor = Color.White;
                    btn.Enabled = true;
                    break;

                default: // free
                    btn.BackColor = _selectedSeats.Contains(btn) ? Color.MediumVioletRed : Color.White;
                    btn.ForeColor = _selectedSeats.Contains(btn) ? Color.White : Color.Maroon;
                    btn.Enabled = true;
                    break;
            }
        }
    }
}