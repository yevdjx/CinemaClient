using CinemaClient.Services;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace CinemaClient.Forms
{
    public partial class KorzinaForm : Form
    {
        private readonly ApiService _api;
        private readonly List<SeatDto> _selectedSeats;
        private readonly int _sessionId;
        private readonly string _movieTitle;

        // List<SeatDto> selectedSeats, int sessionId, string movieTitle
        public KorzinaForm(ApiService api)
        {
            InitializeComponent();
            _api = api;
            //_selectedSeats = selectedSeats;
            //_sessionId = sessionId;
            //_movieTitle = movieTitle;
        }

        private void KorzinaForm_Load(object sender, EventArgs e)
        {
            // Настройка внешнего вида формы
            this.Text = $"Корзина - {_movieTitle}";
            this.BackColor = Color.AntiqueWhite;
            this.Font = new Font("Bahnschrift", 10, FontStyle.Regular);

            // Создаем основной контейнер
            var mainPanel = new Panel { Dock = DockStyle.Fill };
            this.Controls.Add(mainPanel);

            // Заголовок
            var titleLabel = new Label
            {
                Text = $"Ваши выбранные места на фильм «{_movieTitle}»",
                Dock = DockStyle.Top,
                TextAlign = ContentAlignment.MiddleCenter,
                Font = new Font("Bahnschrift", 12, FontStyle.Bold),
                Height = 50
            };
            mainPanel.Controls.Add(titleLabel);

            // Список выбранных мест
            var seatsList = new ListBox
            {
                Dock = DockStyle.Fill,
                Font = new Font("Bahnschrift", 10),
                BorderStyle = BorderStyle.None,
                ItemHeight = 25
            };

            foreach (var seat in _selectedSeats)
            {
                seatsList.Items.Add($"Ряд {seat.Row}, Место {seat.Number} - 280 руб.");
            }
            mainPanel.Controls.Add(seatsList);

            // Панель с кнопками
            var buttonsPanel = new Panel
            {
                Dock = DockStyle.Bottom,
                Height = 60
            };
            mainPanel.Controls.Add(buttonsPanel);

            // Кнопка "Назад"
            var btnBack = new Button
            {
                Text = "Назад",
                Dock = DockStyle.Left,
                Width = 150,
                BackColor = Color.LightGray
            };
            btnBack.Click += (s, args) => this.Close();
            buttonsPanel.Controls.Add(btnBack);

            // Кнопка "Оплатить"
            var btnPay = new Button
            {
                Text = "Оплатить",
                Dock = DockStyle.Right,
                Width = 150,
                BackColor = Color.DeepPink,
                ForeColor = Color.White
            };
            //btnPay.Click += BtnPay_Click;
            buttonsPanel.Controls.Add(btnPay);

            // Метка с общей суммой
            var totalLabel = new Label
            {
                Text = $"Итого: {_selectedSeats.Count * 280} руб.",
                Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.MiddleRight,
                Font = new Font("Bahnschrift", 10, FontStyle.Bold)
            };
            buttonsPanel.Controls.Add(totalLabel);
        }

        //private async void BtnPay_Click(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        // 1. Получаем email пользователя
        //        string userEmail = await _api.GetUserEmailAsync();
        //        if (string.IsNullOrEmpty(userEmail))
        //        {
        //            MessageBox.Show("Не удалось определить ваш email. Пожалуйста, проверьте данные профиля.",
        //                          "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
        //            return;
        //        }

        //        // 2. Получаем имя пользователя
        //        string userName = await _api.GetUserNameAsync();

        //        // 3. Получаем информацию о сеансе
        //        var session = await _api.GetSessionAsync(_sessionId);
        //        if (session == null)
        //        {
        //            MessageBox.Show("Не удалось получить информацию о сеансе.", "Ошибка");
        //            return;
        //        }

        //        // 4. Получаем изображение фильма
        //        byte[] movieImage = null;
        //        if (session.MovieId > 0)
        //        {
        //            movieImage = await _api.GetMovieImageAsync(session.MovieId);
        //        }

        //        // 4. Подготавливаем данные для билета
        //        var seatsInfo = new List<string>();
        //        foreach (var seat in _selectedSeats)
        //        {
        //            seatsInfo.Add($"Ряд {seat.Row}, Место {seat.Number}");
        //        }

        //        // 5. Создаем PDF билет
        //        var ticketService = new TicketService(_api);
        //        var pdfBytes = ticketService.GeneratePdfTicket(
        //            _movieTitle,
        //            session.StartTime,
        //            session.EndTime,
        //            session.HallName,
        //            seatsInfo,
        //            _selectedSeats.Count * 280,
        //            userName,
        //            await _api.GetMovieImageAsync(session.MovieId));

        //        // 6. Отправляем билет на email
        //        ticketService.SendEmailWithTicket(userEmail, pdfBytes, _movieTitle, userName);

        //        // 7. Подтверждаем бронирование на сервере
        //        bool bookingSuccess = true;
        //        foreach (var seat in _selectedSeats)
        //        {
        //            if (!await _api.ConfirmBookingAsync(seat.TicketId))
        //            {
        //                bookingSuccess = false;
        //                break;
        //            }
        //        }

        //        if (bookingSuccess)
        //        {
        //            MessageBox.Show($"Билеты успешно оплачены и отправлены на {userEmail}!",
        //                          "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);
        //            this.DialogResult = DialogResult.OK;
        //            this.Close();
        //        }
        //        else
        //        {
        //            MessageBox.Show("Произошла ошибка при подтверждении бронирования.",
        //                          "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show($"Ошибка при оформлении заказа: {ex.Message}",
        //                      "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
        //    }
        //}
    }
}
