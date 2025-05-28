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
            sessionList.SelectionChanged += OnSessionSelected;
            sohrButton.Click += OnSaveClicked;
            otmenButton.Click += OnCancelClicked;
            delButton.Click += OnDeleteClicked;
            takePrice.KeyPress += OnPriceInput;
        }

        private void OnPriceInput(object sender, KeyPressEventArgs e)
        {
            e.Handled = !(char.IsDigit(e.KeyChar) || char.IsControl(e.KeyChar));
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
                comboHall.Items.AddRange(new object[] { 1, 2 });

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
                }

                ResetFormFields();
            }
            catch (Exception ex)
            {
                ShowError($"Ошибка загрузки сеансов: {ex.Message}");
            }
        }

        private void OnSessionSelected(object sender, EventArgs e)
        {
            if (sessionList.SelectedRows.Count == 0 ||
                sessionList.SelectedRows[0].Index < 0 ||
                sessionList.SelectedRows[0].DataBoundItem == null)
            {
                ResetFormFields();
                return;
            }

            var selectedSession = (SessionDto)sessionList.SelectedRows[0].DataBoundItem;
            _currentSessionId = selectedSession.SessionId;
            comboHall.SelectedItem = selectedSession.HallNumber;
            comboFilm.SelectedValue = selectedSession.MovieTitle;
            takeTime.Value = selectedSession.SessionDateTime;
            takePrice.Text = selectedSession.Price.ToString();
        }

        private async void OnSaveClicked(object sender, EventArgs e)
        {
            if (!ValidateForm()) return;

            try
            {
                var result = _currentSessionId.HasValue
                    ? await UpdateExistingSession()
                    : await CreateNewSession();

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
                ShowWarning("Выберите название фильма");
                return false;
            }

            if (!decimal.TryParse(takePrice.Text, out decimal price) || price <= 0)
            {
                ShowWarning("Цена должна быть положительным числом");
                return false;
            }

            return true;
        }

        private async Task<(bool Success, string? Error)> UpdateExistingSession()
        {
            return await _api.UpdateSessionAsync(
                _currentSessionId.Value,
                comboHall.SelectedItem.ToString(),
                comboFilm.SelectedValue.ToString(),
                takeTime.Value,
                decimal.Parse(takePrice.Text));
        }

        private async Task<(bool Success, string? Error)> CreateNewSession()
        {
            return await _api.CreateSessionAsync(
                comboHall.SelectedItem.ToString(),
                comboFilm.SelectedValue.ToString(),
                takeTime.Value,
                decimal.Parse(takePrice.Text));
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
