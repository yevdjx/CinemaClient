using CinemaClient.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace CinemaClient.Forms
{
    public partial class SessionAdminForm : Form
    {
        private readonly ApiService _api;
        private int? _currentSessionId = null;
        private List<SessionDto> _currentSessions = new List<SessionDto>();

        public SessionAdminForm(ApiService api)
        {
            InitializeComponent();
            _api = api;

            SetupEventHandlers();
            LoadHallsAndMovies();
            LoadSessionList();
        }

        private void SetupEventHandlers()
        {
            // Изменяем обработчик для клика по ячейкам
            sessionList.CellClick += OnSessionCellClicked;
            sohrButton.Click += OnSaveClicked;
            otmenButton.Click += OnCancelClicked;
            delButton.Click += OnDeleteClicked;
            takePrice.KeyPress += OnPriceInput;
        }

        private void OnPriceInput(object sender, KeyPressEventArgs e)
        {
            e.Handled = !(char.IsDigit(e.KeyChar) || e.KeyChar == ',' || char.IsControl(e.KeyChar));
        }

        private void ResetFormFields()
        {
            _currentSessionId = null;
            comboHall.SelectedIndex = -1;
            comboFilm.SelectedIndex = -1;
            takeTime.Value = DateTime.Now;
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

        private async void LoadHallsAndMovies()
        {
            try
            {
                // Заполняем comboHall значениями 1 и 2
                comboHall.Items.AddRange(new object[] { "1", "2" });

                var movies = await _api.GetMoviesAsync();
                comboFilm.DataSource = movies.ToList();
                comboFilm.DisplayMember = "movieTitle";
                comboFilm.ValueMember = "movieId";
            }
            catch (Exception ex)
            {
                ShowError($"Ошибка загрузки данных: {ex.Message}");
            }
        }

        private async void LoadSessionList()
        {
            try
            {
                _currentSessions = (await _api.GetSessionsAsync()).ToList();
                sessionList.DataSource = _currentSessions;

                if (sessionList.Columns.Count > 0)
                {
                    sessionList.Columns["SessionId"].Visible = false;
                    sessionList.Columns["SessionDateTime"].HeaderText = "Время сеанса";
                    sessionList.Columns["HallNumber"].HeaderText = "Номер зала";
                    sessionList.Columns["MovieTitle"].HeaderText = "Название фильма";
                    sessionList.Columns["Price"].HeaderText = "Цена билета";

                    // Делаем все столбцы кликабельными
                    foreach (DataGridViewColumn column in sessionList.Columns)
                    {
                        column.SortMode = DataGridViewColumnSortMode.NotSortable;
                    }
                }

                ResetFormFields();
            }
            catch (Exception ex)
            {
                ShowError($"Ошибка загрузки сеансов: {ex.Message}");
            }
        }

        private void OnSessionCellClicked(object sender, DataGridViewCellEventArgs e)
        {
            // Игнорируем клики по заголовкам
            if (e.RowIndex < 0) return;

            // Получаем выбранную строку
            var selectedRow = sessionList.Rows[e.RowIndex];

            // Проверяем, что строка содержит данные
            if (selectedRow.DataBoundItem == null)
            {
                ResetFormFields();
                return;
            }

            var selectedSession = (SessionDto)selectedRow.DataBoundItem;
            _currentSessionId = selectedSession.SessionId;

            // Заполняем поля формы
            comboHall.SelectedItem = selectedSession.HallNumber;

            // Находим фильм по названию (так как SelectedValue работает с ID)
            foreach (var item in comboFilm.Items)
            {
                if (item is MovieDto movie && movie.movieTitle == selectedSession.MovieTitle)
                {
                    comboFilm.SelectedItem = item;
                    break;
                }
            }

            takeTime.Value = selectedSession.SessionDateTime;
            takePrice.Text = selectedSession.Price.ToString("0.00");
        }

        private async void OnSaveClicked(object sender, EventArgs e)
        {
            if (!ValidateForm()) return;

            try
            {
                var hallNumber = comboHall.SelectedItem.ToString();
                var movieTitle = ((MovieDto)comboFilm.SelectedItem).movieTitle;
                var sessionTime = takeTime.Value;
                var price = decimal.Parse(takePrice.Text);

                var result = _currentSessionId.HasValue
                    ? await _api.UpdateSessionAsync(_currentSessionId.Value, hallNumber, movieTitle, sessionTime, price)
                    : await _api.CreateSessionAsync(hallNumber, movieTitle, sessionTime, price);

                if (result.Success)
                {
                    ShowSuccess(_currentSessionId.HasValue ? "Сеанс обновлен" : "Сеанс добавлен");
                    LoadSessionList();
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
            if (comboHall.SelectedIndex == -1)
            {
                ShowWarning("Выберите номер зала");
                return false;
            }

            if (comboFilm.SelectedIndex == -1)
            {
                ShowWarning("Выберите фильм");
                return false;
            }

            if (!decimal.TryParse(takePrice.Text, out decimal price) || price <= 0)
            {
                ShowWarning("Цена должна быть положительным числом");
                return false;
            }

            // Проверка, что выбранные дата и время меньше текущих даты и времени
            if (takeTime.Value < DateTime.Now)
            {
                ShowWarning("Выбранные дата и время должны быть больше текущих даты и времени");
                return false;
            }

            return true;
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
                    LoadSessionList();
                }
                else
                {
                    ShowError(error ?? "Ошибка при удалении");
                }
            }
        }
    }
}