using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using CinemaClient.Services;

namespace CinemaClient.Forms
{
    public partial class SessionsForm : Form
    {
        private readonly ApiService _api;
        private List<SessionDto> _allSessions;
        private FlowLayoutPanel _dateMenuPanel;
        private FlowLayoutPanel _moviesPanel;

        public SessionsForm(ApiService api)
        {
            InitializeComponent();
            _api = api;
            SetupUI();
            LoadDataAsync();
        }

        private void SetupUI()
        {
            // Настройка основного контейнера
            this.BackColor = Color.AntiqueWhite;
            this.Text = "Кинотеатр - Расписание сеансов";
            this.WindowState = FormWindowState.Maximized;

            // Основная панель с фильмами
            _moviesPanel = new FlowLayoutPanel
            {
                Dock = DockStyle.Fill,
                AutoScroll = true,
                BackColor = Color.AntiqueWhite,
                Padding = new Padding(20)
            };
            this.Controls.Add(_moviesPanel);

            // Панель с датами (меню)
            _dateMenuPanel = new FlowLayoutPanel
            {
                Dock = DockStyle.Top,
                Height = 60,
                BackColor = Color.PaleVioletRed,
                AutoScroll = true
            };
            this.Controls.Add(_dateMenuPanel);
        }

        private async void LoadDataAsync()
        {
            try
            {
                _allSessions = (await _api.GetSessionsAsync()).ToList();
                CreateDateMenu();
                ShowSessionsForDate(DateTime.Today); // Показываем сеансы на сегодня по умолчанию
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки данных: {ex.Message}", "Ошибка",
                              MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void CreateDateMenu()
        {
            _dateMenuPanel.Controls.Clear();

            // Получаем уникальные даты из сеансов
            var dates = _allSessions
                .Select(s => s.SessionDateTime.Date)
                .Distinct()
                .OrderBy(d => d)
                .ToList();

            foreach (var date in dates)
            {
                var btn = new Button
                {
                    Text = date.ToString("ddd, dd MMM"),
                    Tag = date,
                    Width = 120,
                    Height = 40,
                    Margin = new Padding(5),
                    FlatStyle = FlatStyle.Flat,
                    BackColor = Color.MistyRose,
                    ForeColor = Color.Maroon,
                    Font = new Font("Bahnschrift SemiBold Condensed", 12, FontStyle.Bold)
                };
                btn.FlatAppearance.BorderSize = 1;
                btn.FlatStyle = FlatStyle.Standard;
                btn.Click += DateButton_Click;

                _dateMenuPanel.Controls.Add(btn);
            }
        }

        private void DateButton_Click(object sender, EventArgs e)
        {
            var button = (Button)sender;
            var selectedDate = (DateTime)button.Tag;
            ShowSessionsForDate(selectedDate);
        }

        private void ShowSessionsForDate(DateTime date)
        {
            _moviesPanel.Controls.Clear();

            // Группируем сеансы по фильмам
            var movies = _allSessions
                .Where(s => s.SessionDateTime.Date == date.Date)
                .GroupBy(s => s.MovieTitle)
                .ToList();

            foreach (var movieGroup in movies)
            {
                var movieCard = CreateMovieCard(movieGroup.Key, movieGroup.ToList());
                _moviesPanel.Controls.Add(movieCard);
            }
        }

        private Panel CreateMovieCard(string movieTitle, List<SessionDto> sessions)
        {
            var firstSession = sessions.First();

            var panel = new Panel
            {
                Width = 800,
                Height = 300,
                Margin = new Padding(20),
                BackColor = Color.MistyRose,
                BorderStyle = BorderStyle.FixedSingle
            };

            // Постер фильма
            var pictureBox = new PictureBox
            {
                Width = 180,
                Height = 260,
                Location = new Point(20, 20),
                SizeMode = PictureBoxSizeMode.Zoom,
                BackColor = Color.Black
            };

            if (firstSession.MovieImage != null && firstSession.MovieImage.Length > 0)
            {
                using var ms = new MemoryStream(firstSession.MovieImage);
                pictureBox.Image = Image.FromStream(ms);
            }
            panel.Controls.Add(pictureBox);

            var infoLabel = new Label
            {
                Text = $"{movieTitle}\n\n",
                Location = new Point(220, 20),
                Width = 300,
                Height = 120,
                ForeColor = Color.PaleVioletRed,
                Font = new Font("Bahnschrift Condensed", 20)
            };
            panel.Controls.Add(infoLabel);

            // Панель с временами сеансов
            var timesPanel = new FlowLayoutPanel
            {
                Location = new Point(220, 150),
                Width = 550,
                Height = 120,
                AutoScroll = true
            };

            foreach (var session in sessions.OrderBy(s => s.SessionDateTime))
            {
                var sessionButton = new Button
                {
                    Text = session.SessionDateTime.ToString("HH:mm") + $"\n{session.Price} ₽",
                    Tag = session.SessionId,
                    Width = 80,
                    Height = 50,
                    Margin = new Padding(5),
                    FlatStyle = FlatStyle.Flat,
                    BackColor = Color.AntiqueWhite,
                    ForeColor = Color.Maroon,
                    Font = new Font("Bahnschrift SemiBold Condensed", 12)
                };
                sessionButton.FlatStyle = FlatStyle.Standard;
                sessionButton.FlatAppearance.BorderColor = Color.MistyRose;
                sessionButton.FlatAppearance.BorderSize = 1;
                sessionButton.Click += SessionButton_Click;

                timesPanel.Controls.Add(sessionButton);
            }

            panel.Controls.Add(timesPanel);

            return panel;
        }

        private void SessionButton_Click(object sender, EventArgs e)
        {
            var button = (Button)sender;
            var sessionId = (int)button.Tag;

            var selectedSession = _allSessions.FirstOrDefault(s => s.SessionId == sessionId);
            if (selectedSession != null)
            {
                var hallForm = new HallForm(_api, selectedSession.SessionId, selectedSession.MovieTitle);
                hallForm.ShowDialog();
            }
        }
    }

}
