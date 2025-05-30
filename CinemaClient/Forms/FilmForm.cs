﻿using CinemaClient.Services;
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

        // органичения на ввод
        private void OnDurationInput(object sender, KeyPressEventArgs e)
        {
            e.Handled = !(char.IsDigit(e.KeyChar) || char.IsControl(e.KeyChar));
        }

        // типа очистка формы
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

        // заполнение таблицы
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

        // выделение строки таблицы - получаем объектик фильма
        async private void OnFilmSelected(object sender, EventArgs e)
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

            if (selectedMovie.movieImage != null && selectedMovie.movieImage.Length > 0)
            {
                using var ms = new MemoryStream(selectedMovie.movieImage);
                pictureBox1.Image = Image.FromStream(ms);
            }
            else
            {
                Image apiImage = await _api.GetMovieImageAsync((int)_currentMovieId);

                if (apiImage != null)
                {
                    Image imageCopy = new Bitmap(apiImage);

                    pictureBox1.Image?.Dispose();
                    pictureBox1.Image = imageCopy;

                    apiImage.Dispose(); // чтобы не висел лишний ресурс
                }
            }
        }

        // событие кнопки СОХРАНИТЬ
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

        // проверяем чтобы не оставляли поля пустыми
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
                PictureBoxImageToByteArray(pictureBox1)); 
        }

        private async Task<(bool Success, string? Error)> CreateNewMovie()
        {
            return await _api.CreateMovieAsync(
                takeFilmName.Text,
                int.Parse(takeProd.Text),
                takeDir.Text,
                takeAge.Text,
                PictureBoxImageToByteArray(pictureBox1));

        }

        // очистка полей - сброс изменений
        private void OnCancelClicked(object sender, EventArgs e)
        {
            ResetFormFields();
        }

        // событие на кнопку УДАЛИТЬ
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

        // наводим красоту в таблице
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

        // кнопка добавить ПОСТЕР
        private void addPoster_Click(object sender, EventArgs e)
        {
            try
            {
                using (OpenFileDialog openFileDialog = new OpenFileDialog())
                {
                    openFileDialog.Filter = "JPEG Images|*.jpg;*.jpeg";
                    openFileDialog.Title = "Выберите JPEG-изображение";

                    if (openFileDialog.ShowDialog() == DialogResult.OK)
                    {
                        string filePath = openFileDialog.FileName;

                        // Проверка расширения файла
                        if (!filePath.EndsWith(".jpg", StringComparison.OrdinalIgnoreCase) &&
                            !filePath.EndsWith(".jpeg", StringComparison.OrdinalIgnoreCase))
                        {
                            MessageBox.Show("Пожалуйста, выберите файл в формате JPEG (.jpg или .jpeg).", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }

                        // Загружаем и копируем изображение в память
                        using (var stream = new FileStream(filePath, FileMode.Open, FileAccess.Read))
                        {
                            Image originalImage = Image.FromStream(stream);
                            Image imageCopy = new Bitmap(originalImage);

                            pictureBox1.Image?.Dispose(); // освобождаем старое
                            pictureBox1.Image = imageCopy;
                        }
                    }
                }
            }
            catch (OutOfMemoryException)
            {
                MessageBox.Show("Файл не является допустимым изображением или поврежден.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (FileNotFoundException)
            {
                MessageBox.Show("Файл не найден.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Произошла ошибка: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // переводим картинку в байты
        public static byte[] PictureBoxImageToByteArray(PictureBox pictureBox) 
        {
            if (pictureBox.Image == null)
            {
                return null;
            }

            using (var ms = new MemoryStream())
            {
                // Создаём новую Bitmap копию — 100% безопасно для сохранения
                using (var bmp = new Bitmap(pictureBox.Image))
                {
                    bmp.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
                }

                return ms.ToArray();
            }
        }
    }
}
