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
            // Главный контейнер с правильной структурой
            var mainPanel = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = Color.AntiqueWhite
            };
            this.Controls.Add(mainPanel);

            // 1. Панель экрана (верхняя часть)
            var screenPanel = new Panel
            {
                Dock = DockStyle.Top,
                Height = 60,
                BackColor = Color.AntiqueWhite,
                Padding = new Padding(0, 10, 0, 0)
            };
            mainPanel.Controls.Add(screenPanel);

            // Надпись "ЭКРАН"
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

            // 2. Контейнер для мест с прокруткой
            var scrollPanel = new Panel
            {
                Dock = DockStyle.Fill,
                AutoScroll = true,
                BackColor = Color.AntiqueWhite,
                Padding = new Padding(20, 0, 20, 0)
            };
            mainPanel.Controls.Add(scrollPanel);

            // 3. Панель для таблицы мест
            seatsContainer = new Panel
            {
                AutoSize = true,
                AutoSizeMode = AutoSizeMode.GrowAndShrink,
                BackColor = Color.AntiqueWhite,
                MinimumSize = new Size(0, 300) // Минимальная высота
            };
            scrollPanel.Controls.Add(seatsContainer);

            // Загружаем данные о местах
            var seats = (await _api.GetSeatsAsync(_sessionId)).ToList();
            int rows = seats.Max(s => s.Row);
            int cols = seats.Max(s => s.Number);

            // Создаем таблицу для мест
            seatsTable = new TableLayoutPanel
            {
                AutoSize = true,
                AutoSizeMode = AutoSizeMode.GrowAndShrink,
                ColumnCount = cols,
                RowCount = rows,
                BackColor = Color.AntiqueWhite,
                Margin = new Padding(0, 30, 0, 20) // Отступ сверху и снизу
            };

            // Настраиваем столбцы и строки
            for (int i = 0; i < cols; i++)
                seatsTable.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 40));
            for (int i = 0; i < rows; i++)
                seatsTable.RowStyles.Add(new RowStyle(SizeType.Absolute, 40));

            seatsContainer.Controls.Add(seatsTable);
            CenterSeatsTable();

            // Создаем кнопки мест
            foreach (var seat in seats)
            {
                var btn = new Button
                {
                    Text = seat.Number.ToString(),
                    Size = new Size(35, 35),
                    FlatStyle = FlatStyle.Flat,
                    Tag = seat,
                    Font = new Font("Bahnschrift", 8, FontStyle.Bold),
                    BackColor = Color.PaleVioletRed,
                    ForeColor = Color.White,
                    FlatAppearance = { BorderColor = Color.DeepPink, BorderSize = 1 }
                };

                switch (seat.Status)
                {
                    case "sold":
                        btn.BackColor = Color.LightGray;
                        btn.ForeColor = Color.DimGray;
                        btn.Enabled = false;
                        btn.FlatAppearance.BorderColor = Color.LightGray;
                        break;
                    case "booked":
                        btn.BackColor = Color.FromArgb(220, 160, 220);
                        btn.ForeColor = Color.White;
                        btn.Enabled = false;
                        break;
                    default: // free
                        btn.BackColor = Color.White;
                        btn.Click += Seat_Click;
                        break;
                }

                seatsTable.Controls.Add(btn, seat.Number - 1, seat.Row - 1);
            }

            // 4. Панель легенды (нижняя часть)
            var legendPanel = new Panel
            {
                Dock = DockStyle.Bottom,
                Height = 60,
                BackColor = Color.AntiqueWhite
            };
            mainPanel.Controls.Add(legendPanel);

            // 5. Кнопка бронирования (самая нижняя)
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

            // Добавляем элементы легенды
            AddLegendItem(legendPanel, "Обычные места (280 руб.)", Color.PaleVioletRed, 20);
            AddLegendItem(legendPanel, "Выбранные места", Color.MediumVioletRed, 180);
            AddLegendItem(legendPanel, "Занятые места", Color.LightGray, 340);

            // Обработчики изменения размера
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
            var btn = (Button)sender;
            var seat = (SeatDto)btn.Tag;

            if (_selectedSeats.Contains(btn))
            {
                btn.BackColor = Color.PaleVioletRed;
                _selectedSeats.Remove(btn);
            }
            else
            {
                btn.BackColor = Color.MediumVioletRed;
                _selectedSeats.Add(btn);
            }
        }

        private async void BookSelectedSeats_Click(object sender, EventArgs e)
        {
            if (_selectedSeats.Count == 0)
            {
                MessageBox.Show("Выберите хотя бы одно место!", "Внимание",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            bool allBooked = true;
            foreach (var btn in _selectedSeats)
            {
                var seat = (SeatDto)btn.Tag;
                if (await _api.BookAsync(seat.TicketId) != 0)
                {
                    allBooked = false;
                    btn.BackColor = Color.FromArgb(220, 160, 220);
                    btn.Enabled = false;
                }
                else
                {
                    btn.BackColor = Color.FromArgb(220, 160, 220);
                    btn.Enabled = false;
                    btn.Click -= Seat_Click;
                }
            }

            if (allBooked)
            {
                MessageBox.Show($"Успешно забронировано {_selectedSeats.Count} мест!", "Успех",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                _selectedSeats.Clear();
            }
            else
            {
                MessageBox.Show("Некоторые места не удалось забронировать.", "Внимание",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
    }
}