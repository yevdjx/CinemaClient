using CinemaClient.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
        }

        private void FilmForm_Load(object sender, EventArgs e)
        {

        }

        private void SetupEventHandlers()
        {
            filmList.SelectionChanged += OnFilmSelected;
            saveButton.Click += OnSaveClicked;
            changeButton.Click += OnCancelClicked;
            deleteButton.Click += OnDeleteClicked;
            takeProd.KeyPress += OnDurationInput;


            if (filmList.Columns.Count == 0)
            {
                filmList.Columns.Add("MovieId", "ID");
                filmList.Columns.Add("Title", "Название");
                filmList.Columns.Add("DurationMinutes", "Длительность");
                filmList.Columns.Add("Director", "Режиссер");
                filmList.Columns.Add("AgeRestriction", "Возрастное ограничение");

                filmList.Columns["MovieId"].Visible = false;
                filmList.Columns["DurationMinutes"].DefaultCellStyle.Format = "0 мин";
            }
        }

        private async void LoadMovieList()
        {
            //try
            //{
            //    var movies = await _api.GetMoviesAsync();

            //    // проверка данных
            //    MessageBox.Show($"Количество фильмов: {movies.Count()}");

            //    filmList.DataSource = movies.ToList();
            //    filmList.Columns["MovieId"].Visible = false;
            //    ResetFormFields();
            //}
            //catch (Exception ex)
            //{
            //    ShowError($"Ошибка загрузки фильмов: {ex.Message}");
            //}

            try
            {
                _currentMovies = (await _api.GetMoviesAsync()).ToList();

                // Обновляем DataGridView
                filmList.DataSource = null;
                filmList.DataSource = _currentMovies;

                // Настраиваем отображение колонок
                foreach (DataGridViewColumn column in filmList.Columns)
                {
                    column.DataPropertyName = column.Name;
                }

                filmList.Columns["MovieId"].Visible = false;
                ResetFormFields();
            }
            catch (Exception ex)
            {
                ShowError($"Ошибка загрузки фильмов: {ex.Message}");
            }
        }

        private void OnFilmSelected(object sender, EventArgs e)
        {
            if (filmList.SelectedRows.Count == 0 || filmList.SelectedRows[0].Index < 0)
            {
                ResetFormFields();
                return;
            }

            var selectedRow = filmList.SelectedRows[0];
            var selectedMovie = selectedRow.DataBoundItem as MovieDto;

            if (selectedMovie != null)
            {
                _currentMovieId = selectedMovie.movieId;
                takeFilmName.Text = selectedMovie.movieTitle ?? string.Empty;
                takeProd.Text = selectedMovie.movieDuration.ToString();
                takeDir.Text = selectedMovie.movieAuthor ?? string.Empty;
                takeAge.Text = selectedMovie.movieAgeRating ?? string.Empty;
            }
        }

        private async void OnSaveClicked(object sender, EventArgs e)
        {
            //if (!ValidateForm()) return;

            //try
            //{
            //    var result = _currentMovieId.HasValue
            //        ? await UpdateExistingMovie()
            //        : await CreateNewMovie();

            //    if (result.Success)
            //    {
            //        ShowSuccess(_currentMovieId.HasValue ? "Фильм обновлен" : "Фильм добавлен");
            //        LoadMovieList();
            //    }
            //    else
            //    {
            //        ShowError(result.Error ?? "Неизвестная ошибка");
            //    }
            //}
            //catch (Exception ex)
            //{
            //    ShowError($"Ошибка сохранения: {ex.Message}");
            //}

            if (!ValidateForm()) return;

            try
            {
                var result = _currentMovieId.HasValue
                    ? await UpdateExistingMovie()
                    : await CreateNewMovie();

                if (result.Success)
                {
                    ShowSuccess(_currentMovieId.HasValue ? "Фильм обновлен" : "Фильм добавлен");
                    LoadMovieList(); // Обновляем список
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

        private async Task<(bool Success, string? Error)> UpdateExistingMovie()
        {
            return await _api.UpdateMovieAsync(
                _currentMovieId.Value,
                takeFilmName.Text,
                int.Parse(takeProd.Text),
                takeDir.Text,
                takeAge.Text);
        }

        private async Task<(bool Success, string? Error)> CreateNewMovie()
        {
            return await _api.CreateMovieAsync(
                takeFilmName.Text,
                int.Parse(takeProd.Text),
                takeDir.Text,
                takeAge.Text);
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

        private void OnDurationInput(object sender, KeyPressEventArgs e)
        {
            e.Handled = !(char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar));
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

        private void ResetFormFields()
        {
            _currentMovieId = null;
            takeFilmName.Clear();
            takeProd.Clear();
            takeDir.Clear();
            takeAge.Clear();
            filmList.ClearSelection();
        }

        // Вспомогательные методы для отображения сообщений
        private void ShowError(string message) =>
            MessageBox.Show(message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);

        private void ShowWarning(string message) =>
            MessageBox.Show(message, "Предупреждение", MessageBoxButtons.OK, MessageBoxIcon.Warning);

        private void ShowSuccess(string message) =>
            MessageBox.Show(message, "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);

        private bool ConfirmAction(string question) =>
            MessageBox.Show(question, "Подтверждение", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes;
    }
}
