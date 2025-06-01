using CinemaClient.Services; // Используем сервисы приложения
using System;
using System.Collections.Generic;
using System.Drawing; // Работа с графикой
using System.IO; // Работа с потоками данных
using System.Linq; // LINQ для работы с коллекциями
using System.Windows.Forms; // Элементы Windows Forms

namespace CinemaClient.Forms
{
    public partial class KorzinaForm : Form
    {
        private readonly ApiService _api; // Сервис для работы с API
        private readonly TicketService _ticketService; // Сервис для работы с билетами
        private List<UserTicketDto> _userTickets; // Список билетов пользователя

        // Конструктор формы, принимает ApiService для работы с API
        public KorzinaForm(ApiService api)
        {
            InitializeComponent(); // Инициализация компонентов формы
            _api = api; // Сохраняем сервис API
            _ticketService = new TicketService(api); // Создаем сервис билетов
            SetupUI(); // Настраиваем интерфейс
            LoadTickets(); // Загружаем билеты
        }

        // Настройка пользовательского интерфейса
        private void SetupUI()
        {
            this.Text = "Мои билеты"; // Заголовок формы
            this.BackColor = Color.AntiqueWhite; // Цвет фона
            this.Size = new Size(800, 600); // Размер формы
            this.StartPosition = FormStartPosition.CenterScreen; // Позиция по центру экрана

            // Основной контейнер с прокруткой
            var mainPanel = new Panel
            {
                Dock = DockStyle.Fill, // Занимает все доступное пространство
                AutoScroll = true, // Включаем прокрутку
                AutoSize = false // Отключаем авторазмер (важно для скролла)
            };
            this.Controls.Add(mainPanel); // Добавляем на форму

            // Внутренний контейнер для содержимого
            var contentPanel = new Panel
            {
                Width = mainPanel.ClientSize.Width - 20, // Ширина с учетом скроллбара
                AutoSize = true // Включаем авторазмер по содержимому
            };
            mainPanel.Controls.Add(contentPanel); // Добавляем в основной контейнер

            // Заголовок формы
            var titleLabel = new Label
            {
                Text = "Мои билеты",
                Dock = DockStyle.Top, // Прикрепляем сверху
                Font = new Font("Bahnschrift", 16, FontStyle.Bold), // Стиль шрифта
                TextAlign = ContentAlignment.MiddleCenter, // Выравнивание по центру
                Height = 60, // Фиксированная высота
                BackColor = Color.PaleVioletRed, // Цвет фона
                ForeColor = Color.White // Цвет текста
            };
            contentPanel.Controls.Add(titleLabel); // Добавляем в контейнер

            // Панель для группировка билетов
            var groupPanel = new Panel
            {
                Dock = DockStyle.Fill, // Занимает все доступное пространство
                AutoSize = true // Авторазмер по содержимому
            };
            contentPanel.Controls.Add(groupPanel); // Добавляем в контейнер
        }

        // Асинхронная загрузка билетов пользователя
        private async void LoadTickets()
        {
            try
            {
                // Получаем билеты через API и преобразуем в список
                _userTickets = (await _api.GetUserTicketsAsync()).ToList();
                DisplayTickets(); // Отображаем билеты
            }
            catch (Exception ex)
            {
                // Обработка ошибок загрузки
                MessageBox.Show($"Ошибка загрузки билетов: {ex.Message}", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Отображение списка билетов
        private void DisplayTickets()
        {
            // Получаем ссылки на панели из иерархии контролов
            var mainPanel = (Panel)this.Controls[0];
            var contentPanel = (Panel)mainPanel.Controls[0];
            var groupPanel = (Panel)contentPanel.Controls[1]; // Панель для билетов
            groupPanel.Controls.Clear(); // Очищаем перед обновлением

            // Если билетов нет - показываем сообщение
            if (!_userTickets.Any())
            {
                var emptyLabel = new Label
                {
                    Text = "У вас пока нет билетов",
                    Dock = DockStyle.Fill,
                    TextAlign = ContentAlignment.MiddleCenter,
                    Font = new Font("Bahnschrift", 14),
                    ForeColor = Color.Gray
                };
                groupPanel.Controls.Add(emptyLabel);
                return;
            }

            // Группируем билеты по статутсу (будущие/прошедшие)
            var now = DateTime.Now;
            var futureTickets = _userTickets.Where(t => t.Flag == 1).OrderBy(t => t.SessionDateTime).ToList();
            var pastTickets = _userTickets.Where(t => t.Flag == 0).OrderByDescending(t => t.SessionDateTime).ToList();

            int yPos = 10; // Начальная позиция для размещения элементов

            // Отображение будущие сеансы
            if (futureTickets.Any())
            {
                var futureLabel = CreateGroupLabel("Будущие сеансы", yPos);
                groupPanel.Controls.Add(futureLabel);
                yPos += 40; // Смещаем позицию вниз

                // Создаем карточки для каждого билета
                foreach (var ticket in futureTickets)
                {
                    var card = CreateTicketCard(ticket, true); // true - будущий сеанс
                    card.Location = new Point(20, yPos);
                    groupPanel.Controls.Add(card);
                    yPos += card.Height + 10; // Смещаем позицию с отступом
                }
                yPos += 20; // Дополнительный отступ между группами
            }

            // ОТображение прошедшие сеансы
            if (pastTickets.Any())
            {
                var pastLabel = CreateGroupLabel("Прошедшие сеансы", yPos);
                groupPanel.Controls.Add(pastLabel);
                yPos += 40;

                foreach (var ticket in pastTickets)
                {
                    var card = CreateTicketCard(ticket, false); // false - прошедший сеанс
                    card.Location = new Point(20, yPos);
                    groupPanel.Controls.Add(card);
                    yPos += card.Height + 10;
                }
            }
        }

        // Создание заголовка группы билетов
        private Label CreateGroupLabel(string text, int yPos)
        {
            return new Label
            {
                Text = text,
                Location = new Point(20, yPos),
                AutoSize = true,
                Font = new Font("Bahnschrift", 12, FontStyle.Bold),
                ForeColor = Color.Maroon
            };
        }

        // Создание карточки билета
        private Panel CreateTicketCard(UserTicketDto ticket, bool isFuture)
        {
            // Основная панель карточки
            var card = new Panel
            {
                Width = 730, // Фиксированная ширина
                Height = 150, // Фиксированная высота
                BackColor = isFuture ? Color.MistyRose : Color.Lavender, // Разные цвета для будущих/прошедших
                BorderStyle = BorderStyle.FixedSingle, // Рамка
                Padding = new Padding(10) // Внутренние отступы
            };

            // Загрузка изображения фильма (если есть)
            if (ticket.MovieImage != null && ticket.MovieImage.Length > 0)
            {
                try
                {
                    using (var ms = new MemoryStream(ticket.MovieImage))
                    {
                        var pictureBox = new PictureBox
                        {
                            Image = Image.FromStream(ms), // Загружаем изображение из потока
                            SizeMode = PictureBoxSizeMode.Zoom, // Режим масштабирования
                            Size = new Size(100, 130), // Фиксированный размер
                            Location = new Point(10, 10) // Позиция в карточке
                        };
                        card.Controls.Add(pictureBox); // Добавляем в контейнер
                    }
                }
                catch { /* Игнорируем ошибки загрузки изображения */ }
            }

            // Основная информация о билете
            var infoLabel = new Label
            {
                Text = $"{ticket.MovieTitle}\n" +
                      $"Дата: {ticket.SessionDateTime:dd.MM.yyyy}\n" +
                      $"Время: {ticket.SessionDateTime:HH:mm}\n" +
                      $"Зал: {ticket.HallNumber}\n" +
                      $"Место: Ряд {ticket.Row}, Место {ticket.Seat}",
                Location = new Point(120, 10), // Позиция справа от изображения
                AutoSize = true, // Авторазмер по содержимому
                Font = new Font("Bahnschrift", 10) // Стиль шрифта
            };
            card.Controls.Add(infoLabel);

            // ОТображения статуса и цены (бронь/оплачено)
            var statusLabel = new Label
            {
                Text = ticket.Status == "booked" ? "Бронь" : "Оплачено",
                Location = new Point(400, 10),
                AutoSize = true,
                Font = new Font("Bahnschrift", 10, FontStyle.Bold),
                ForeColor = ticket.Status == "booked" ? Color.Blue : Color.Green // Разные цвета статусов
            };
            card.Controls.Add(statusLabel);

            // Отображение цены
            var priceLabel = new Label
            {
                Text = $"{ticket.Price} руб.",
                Location = new Point(400, 40),
                AutoSize = true,
                Font = new Font("Bahnschrift", 10, FontStyle.Bold)
            };
            card.Controls.Add(priceLabel);

            // Кнопка отправки только для будущих сеансов
            if (isFuture)
            {
                var sendButton = new Button
                {
                    Text = "Отправить билет",
                    Location = new Point(550, 80),
                    Size = new Size(150, 30),
                    BackColor = Color.DeepPink,
                    ForeColor = Color.White,
                    FlatStyle = FlatStyle.Flat,
                    Tag = ticket // Сохраняем билет в Tag для доступа при клике
                };
                sendButton.FlatStyle = FlatStyle.Standard;
                sendButton.FlatAppearance.BorderSize = 0;
                sendButton.Click += SendButton_Click; // Подписываемся на событие клика
                card.Controls.Add(sendButton);
            }

            return card;
        }

        // Обработчик клика по кнопке отправки билета
        private async void SendButton_Click(object sender, EventArgs e)
        {
            var button = (Button)sender;
            var ticket = (UserTicketDto)button.Tag; // Получаем билет из Tag кнопки

            // Создаем форму для ввода email
            using (var inputForm = new Form())
            {
                inputForm.Text = "Отправить билет на email";
                inputForm.Size = new Size(350, 180);
                inputForm.StartPosition = FormStartPosition.CenterParent;
                inputForm.BackColor = Color.AntiqueWhite;
                inputForm.FormBorderStyle = FormBorderStyle.FixedDialog;

                // Метка для поля ввода
                var emailLabel = new Label
                {
                    Text = "Введите email:",
                    Location = new Point(20, 20),
                    AutoSize = true
                };
                inputForm.Controls.Add(emailLabel);

                // Поле ввода email
                var emailTextBox = new TextBox
                {
                    Location = new Point(20, 50),
                    Width = 300
                };
                inputForm.Controls.Add(emailTextBox);
                // Кнопка отправки

                var sendButton = new Button
                {
                    Text = "Отправить",
                    Location = new Point(120, 100),
                    Size = new Size(100, 30),
                    BackColor = Color.DeepPink,
                    ForeColor = Color.White,
                    FlatStyle = FlatStyle.Flat,
                    DialogResult = DialogResult.OK
                };
                inputForm.Controls.Add(sendButton);
                inputForm.AcceptButton = sendButton;

                // Если пользователь подтвердил ввод
                if (inputForm.ShowDialog() == DialogResult.OK && !string.IsNullOrWhiteSpace(emailTextBox.Text))
                {
                    try
                    {
                        // Генерируем PDF и отправляем на email
                        var pdfBytes = _ticketService.GeneratePdfTicket(ticket);
                        await _ticketService.SendEmailWithTicketAsync(emailTextBox.Text, pdfBytes, ticket);

                        MessageBox.Show($"Билет на {ticket.MovieTitle} успешно отправлен!", "Успех",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Ошибка отправки: {ex.Message}", "Ошибка",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }
    }
}