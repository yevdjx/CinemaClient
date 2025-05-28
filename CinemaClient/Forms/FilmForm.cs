using CinemaClient.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace CinemaClient.Forms
{
    public partial class FilmForm : Form
    {
        private readonly ApiService _api;
        private int? _currentMovieId = null;
        private List<MovieDto> _currentMovies = new List<MovieDto>();

        public FilmForm(ApiService api)
        {
            InitializeComponent();
            _api = api;

            SetupEventHandlers();
            LoadMovieList();

            this.Resize += (sender, e) => AdjustDataGridViewLayout();
            filmList.SizeChanged += (sender, e) => AdjustDataGridViewLayout();
        }

        private void SetupEventHandlers()
        {
            filmList.SelectionChanged += OnFilmSelected;
            saveButton.Click += OnSaveClicked;
            changeButton.Click += OnCancelClicked;
            deleteButton.Click += OnDeleteClicked;
            takeProd.KeyPress += OnDurationInput;

            filmList.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            filmList.MultiSelect = false;
        }

        private void OnDurationInput(object sender, KeyPressEventArgs e)
        {
            e.Handled = !(char.IsDigit(e.KeyChar) || char.IsControl(e.KeyChar));
        }

        private void ResetFormFields()
        {
            _currentMovieId = null;
            takeFilmName.Clear();
            takeProd.Clear();
            takeDir.Clear();
            takeAge.Clear();
            filmList.ClearSelection();
        }

        private void ShowError(string message) =>
            MessageBox.Show(message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);

        private void ShowWarning(string message) =>
            MessageBox.Show(message, "Предупреждение", MessageBoxButtons.OK, MessageBoxIcon.Warning);

        private void ShowSuccess(string message) =>
            MessageBox.Show(message, "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);

        private bool ConfirmAction(string question) =>
            MessageBox.Show(question, "Подтверждение", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes;

        private async void LoadMovieList()
        {
            try
            {
                _currentMovies = (await _api.GetMoviesAsync()).ToList();
                filmList.DataSource = _currentMovies;

                if (filmList.Columns.Count > 0)
                {
                    filmList.Columns["movieId"].Visible = false;
                    filmList.Columns["movieTitle"].HeaderText = "Название";
                    filmList.Columns["movieDuration"].HeaderText = "Длительность";
                    filmList.Columns["movieAuthor"].HeaderText = "Режиссер";
                    filmList.Columns["movieAgeRating"].HeaderText = "Возрастное ограничение";
                }

                ResetFormFields();
                AdjustDataGridViewLayout();
            }
            catch (Exception ex)
            {
                ShowError($"Ошибка загрузки фильмов: {ex.Message}");
            }
        }

        private void OnFilmSelected(object sender, EventArgs e)
        {
            if (filmList.SelectedRows.Count == 0 ||
                filmList.SelectedRows[0].Index < 0 ||
                filmList.SelectedRows[0].DataBoundItem == null)
            {
                ResetFormFields();
                return;
            }

            var selectedMovie = (MovieDto)filmList.SelectedRows[0].DataBoundItem;
            _currentMovieId = selectedMovie.movieId;
            takeFilmName.Text = selectedMovie.movieTitle ?? string.Empty;
            takeProd.Text = selectedMovie.movieDuration.ToString();
            takeDir.Text = selectedMovie.movieAuthor ?? string.Empty;
            takeAge.Text = selectedMovie.movieAgeRating ?? string.Empty;
        }

        private async void OnSaveClicked(object sender, EventArgs e)
        {
            if (!ValidateForm()) return;

            try
            {
                var result = _currentMovieId.HasValue
                    ? await UpdateExistingMovie()
                    : await CreateNewMovie();

                if (result.Success)
                {
                    ShowSuccess(_currentMovieId.HasValue ? "Фильм обновлен" : "Фильм добавлен");
                    LoadMovieList();
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
            if (string.IsNullOrWhiteSpace(takeFilmName.Text))
            {
                ShowWarning("Введите название фильма");
                return false;
            }

            if (!int.TryParse(takeProd.Text, out int duration) || duration <= 0)
            {
                ShowWarning("Продолжительность должна быть положительным числом");
                return false;
            }

            if (string.IsNullOrWhiteSpace(takeDir.Text))
            {
                ShowWarning("Введите имя режиссера");
                return false;
            }

            if (string.IsNullOrWhiteSpace(takeAge.Text) || !takeAge.Text.EndsWith("+"))
            {
                ShowWarning("Введите возрастное ограничение (например, '16+')");
                return false;
            }

            return true;
        }

        private async Task<(bool Success, string? Error)> UpdateExistingMovie()
        {
            return await _api.UpdateMovieAsync(
                _currentMovieId.Value,
                takeFilmName.Text,
                int.Parse(takeProd.Text),
                takeDir.Text,
                takeAge.Text,
                pictureBox1.Image == null ? null : (byte[])new ImageConverter().ConvertTo(pictureBox1.Image, typeof(byte[])));
        }

        private async Task<(bool Success, string? Error)> CreateNewMovie()
        {
            return await _api.CreateMovieAsync(
                takeFilmName.Text,
                int.Parse(takeProd.Text),
                takeDir.Text,
                takeAge.Text,
                pictureBox1.Image == null ? null : (byte[])new ImageConverter().ConvertTo(pictureBox1.Image, typeof(byte[])));

        }

        private void OnCancelClicked(object sender, EventArgs e)
        {
            ResetFormFields();
        }

        private async void OnDeleteClicked(object sender, EventArgs e)
        {
            if (!_currentMovieId.HasValue)
            {
                ShowWarning("Выберите фильм для удаления");
                return;
            }

            if (ConfirmAction("Удалить выбранный фильм?"))
            {
                var (success, error) = await _api.DeleteMovieAsync(_currentMovieId.Value);

                if (success)
                {
                    ShowSuccess("Фильм удален");
                    LoadMovieList();
                }
                else
                {
                    ShowError(error ?? "Ошибка при удалении");
                }
            }
        }

        private void AdjustDataGridViewLayout()
        {
            // Проверяем, есть ли данные и столбцы
            if (filmList.Rows.Count == 0 || filmList.Columns.Count == 0)
                return;

            // Рассчитываем высоту строки (общая высота / количество строк)
            int rowHeight = filmList.Height / filmList.Rows.Count;

            // Устанавливаем высоту для всех строк
            foreach (DataGridViewRow row in filmList.Rows)
            {
                row.Height = rowHeight;
            }

            // Рассчитываем ширину столбца (общая ширина / количество видимых столбцов)
            int visibleColumnsCount = filmList.Columns.Cast<DataGridViewColumn>()
                                              .Count(c => c.Visible);
            if (visibleColumnsCount == 0) return;

            int columnWidth = filmList.Width / visibleColumnsCount;

            // Устанавливаем ширину для всех видимых столбцов
            foreach (DataGridViewColumn column in filmList.Columns)
            {
                if (column.Visible)
                {
                    column.Width = columnWidth;
                }
            }
        }

        private void addPoster_Click(object sender, EventArgs e)
        {

        }
    }


}
