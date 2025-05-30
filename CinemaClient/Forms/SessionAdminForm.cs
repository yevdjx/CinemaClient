
using CinemaClient.Services;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using static System.ComponentModel.Design.ObjectSelectorEditor;

namespace CinemaClient.Forms
{
    public partial class SessionAdminForm : Form
    {
        private readonly ApiService _api;
        private int? _currentSessionId = null;
        private List<SessionAdminDto> _currentSessions = new List<SessionAdminDto>();
        private List<MovieDto> _movies = new List<MovieDto>();
        private List<HallDto> _halls = new List<HallDto>();

        public SessionAdminForm(ApiService api)
        {
            InitializeComponent();
            _api = api;

            SetupEventHandlers();
            LoadInitialData();

            this.Resize += (sender, e) => AdjustDataGridViewLayout();
            sessionList.SizeChanged += (sender, e) => AdjustDataGridViewLayout();
        }

        private void SetupEventHandlers()
        {
            sessionList.SelectionChanged += OnSessionSelected;
            sohrButton.Click += OnSaveClicked;
            otmenButton.Click += OnCancelClicked;
            delButton.Click += OnDeleteClicked;
            tHours.KeyPress += OnTimeInput;
            tMinutes.KeyPress += OnTimeInput;
            takePrice.KeyPress += OnPriceInput;

            sessionList.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            sessionList.MultiSelect = false;
        }


        private void OnTimeInput(object sender, KeyPressEventArgs e)
        {
            e.Handled = !(char.IsDigit(e.KeyChar) || char.IsControl(e.KeyChar));
        }

        private void OnPriceInput(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void ResetFormFields()
        {
            _currentSessionId = null;
            comboHall.SelectedIndex = -1;
            comboFilm.SelectedIndex = -1;
            takeTime.Value = DateTime.Now;
            tHours.Text = "0";
            tMinutes.Text = "0";
            takePrice.Clear();
            sessionList.ClearSelection();
        }

        private void ShowError(string message) =>
            MessageBox.Show(message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);

        private void ShowWarning(string message) =>
            MessageBox.Show(message, "Предупреждение", MessageBoxButtons.OK, MessageBoxIcon.Warning);

        private void ShowSuccess(string message) =>
            MessageBox.Show(message, "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);

        private bool ConfirmAction(string question) =>
            MessageBox.Show(question, "Подтверждение", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes;

        private async void LoadInitialData()
        {
            try
            {
                // Жестко закодированный список залов
                _halls = new List<HallDto>
                {
                    new HallDto(1, "1"),
                    new HallDto(2, "2"),
                    new HallDto(3, "3"),
                    new HallDto(4, "4")
                };
                comboHall.DataSource = _halls;
                comboHall.DisplayMember = "hallNumber";
                comboHall.ValueMember = "hallId";

                // Загружаем фильмы
                _movies = (await _api.GetMoviesAsync()).ToList();
                comboFilm.DataSource = _movies;
                comboFilm.DisplayMember = "movieTitle";
                comboFilm.ValueMember = "movieId";

                // Загружаем сеансы
                await LoadSessionsList();
            }
            catch (Exception ex)
            {
                ShowError($"Ошибка загрузки данных: {ex.Message}");
            }
        }

        private async Task LoadSessionsList()
        {
            try
            {
                _currentSessions = (await _api.GetSessionsAdminAsync()).ToList();
                sessionList.DataSource = _currentSessions;

                if (sessionList.Columns.Count > 0)
                {
                    sessionList.Columns["sessionId"].Visible = false;
                    sessionList.Columns["hallNumber"].HeaderText = "Зал";
                    sessionList.Columns["movieTitle"].HeaderText = "Фильм";
                    sessionList.Columns["sessionDateTime"].HeaderText = "Дата и время";
                    sessionList.Columns["sessionPrice"].HeaderText = "Цена билета";
                    sessionList.Columns["movieImage"].Visible = false; // Скрываем столбец с изображением
                }

                ResetFormFields();
                AdjustDataGridViewLayout();
            }
            catch (Exception ex)
            {
                ShowError($"Ошибка загрузки сеансов: {ex.Message}");
            }
        }


        async private void OnSessionSelected(object sender, EventArgs e)
        {
            if (sessionList.SelectedRows.Count == 0 ||
                sessionList.SelectedRows[0].Index < 0 ||
                sessionList.SelectedRows[0].DataBoundItem == null)
            {
                ResetFormFields();
                return;
            }

            var selected = (SessionAdminDto)sessionList.SelectedRows[0].DataBoundItem;

            comboHall.SelectedValue = selected.hallId;
            comboFilm.SelectedValue = selected.movieId;
            takeTime.Value = selected.sessionDateTime.Date;
            tHours.Text = selected.sessionDateTime.Hour.ToString();
            tMinutes.Text = selected.sessionDateTime.Minute.ToString();

        }

        private async void OnSaveClicked(object sender, EventArgs e)
        {
            if (!ValidateForm()) return;

            try
            {
                var hallId = (int)comboHall.SelectedValue;
                var movieId = (int)comboFilm.SelectedValue;
                var date = takeTime.Value.Date;
                var hours = int.Parse(tHours.Text);
                var minutes = int.Parse(tMinutes.Text);
                var price = int.Parse(takePrice.Text);

                var sessionDateTime = date.AddHours(hours).AddMinutes(minutes);

                // Проверка что дата в будущем
                if (sessionDateTime <= DateTime.Now)
                {
                    ShowWarning("Дата и время сеанса должны быть в будущем");
                    return;
                }

                // Получаем все сеансы для выбранного зала на эту дату
                var sessionsInHall = _currentSessions
                    .Where(s => s.hallId == hallId &&
                               s.sessionDateTime.Date == sessionDateTime.Date &&
                               s.sessionId != _currentSessionId)
                    .ToList();

                // Получаем длительность выбранного фильма
                var selectedMovie = _movies.FirstOrDefault(m => m.movieId == movieId);
                if (selectedMovie == null)
                {
                    ShowError("Выбранный фильм не найден");
                    return;
                }

                var movieDuration = TimeSpan.FromMinutes(selectedMovie.movieDuration);

                // Проверяем пересечение с другими сеансами
                foreach (var existingSession in sessionsInHall)
                {
                    var existingMovie = _movies.FirstOrDefault(m => m.movieId == existingSession.movieId);
                    if (existingMovie == null) continue;

                    var existingDuration = TimeSpan.FromMinutes(existingMovie.movieDuration);
                    var existingStart = existingSession.sessionDateTime;
                    var existingEnd = existingStart.Add(existingDuration);
                    var newEnd = sessionDateTime.Add(movieDuration);

                    // Проверяем пересечение временных интервалов
                    if (sessionDateTime < existingEnd && newEnd > existingStart)
                    {
                        ShowWarning($"Зал занят с {existingStart:HH:mm} до {existingEnd:HH:mm}");
                        return;
                    }
                }

                var result = _currentSessionId.HasValue
                    ? await _api.UpdateSessionAsync(
                        _currentSessionId.Value,
                        hallId,
                        movieId,
                        sessionDateTime,
                        price)
                    : await _api.CreateSessionAsync(
                        hallId,
                        movieId,
                        sessionDateTime,
                        price);

                if (result.Success)
                {
                    ShowSuccess(_currentSessionId.HasValue ? "Сеанс обновлен" : "Сеанс добавлен");
                    await LoadSessionsList();
                }
                else
                {
                    ShowError(result.Error ?? "Неизвестная ошибка");
                }
            }
            catch (Exception ex)
            {
                ShowError($"Ошибка сохранения: {ex.Message}");
            }
        }

        private bool ValidateForm()
        {
            if (comboHall.SelectedItem == null)
            {
                ShowWarning("Выберите зал");
                return false;
            }

            if (comboFilm.SelectedItem == null)
            {
                ShowWarning("Выберите фильм");
                return false;
            }

            if (!int.TryParse(tHours.Text, out int hours) || hours < 0 || hours > 23)
            {
                ShowWarning("Часы должны быть числом от 0 до 23");
                return false;
            }

            if (!int.TryParse(tMinutes.Text, out int minutes) || minutes < 0 || minutes > 59)
            {
                ShowWarning("Минуты должны быть числом от 0 до 59");
                return false;
            }

            if (!int.TryParse(takePrice.Text, out int price) || price <= 0)
            {
                ShowWarning("Цена должна быть положительным числом");
                return false;
            }

            return true;
        }

        private async Task<(bool Success, string? Error)> UpdateExistingSession(int hallId, int movieId, DateTime sessionDateTime, int price)
        {
            return await _api.UpdateSessionAsync(
                _currentSessionId.Value,
                hallId,
                movieId,
                sessionDateTime,
                price);
        }

        private async Task<(bool Success, string? Error)> CreateNewSession(int hallId, int movieId, DateTime sessionDateTime, int price)
        {
            return await _api.CreateSessionAsync(
                hallId,
                movieId,
                sessionDateTime,
                price);
        }

        private void OnCancelClicked(object sender, EventArgs e)
        {
            ResetFormFields();
        }

        private async void OnDeleteClicked(object sender, EventArgs e)
        {
            if (!_currentSessionId.HasValue)
            {
                ShowWarning("Выберите сеанс для удаления");
                return;
            }

            if (ConfirmAction("Удалить выбранный сеанс?"))
            {
                var (success, error) = await _api.DeleteSessionAsync(_currentSessionId.Value);

                if (success)
                {
                    ShowSuccess("Сеанс удален");
                    await LoadSessionsList();
                }
                else
                {
                    ShowError(error ?? "Ошибка при удалении");
                }
            }
        }

        private void AdjustDataGridViewLayout()
        {
            if (sessionList.Rows.Count == 0 || sessionList.Columns.Count == 0)
                return;

            int rowHeight = sessionList.Height / sessionList.Rows.Count;

            foreach (DataGridViewRow row in sessionList.Rows)
            {
                row.Height = rowHeight;
            }

            int visibleColumnsCount = sessionList.Columns.Cast<DataGridViewColumn>()
                .Count(c => c.Visible);
            if (visibleColumnsCount == 0) return;

            int columnWidth = sessionList.Width / visibleColumnsCount;

            foreach (DataGridViewColumn column in sessionList.Columns)
            {
                if (column.Visible)
                {
                    column.Width = columnWidth;
                }
            }
        }
    }
}
