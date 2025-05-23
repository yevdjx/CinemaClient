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
    public partial class aUserForm : Form
    {
        private readonly ApiService _api;

        public aUserForm(ApiService api)
        {
            InitializeComponent();
            _api = api;
            LoadUsersData().ConfigureAwait(false); // Используем ConfigureAwait(false) для избежания deadlock
            Load += async (_, _) => await LoadUsersData();
        }

        protected override async void OnShown(EventArgs e)
        {
            base.OnShown(e);
            await LoadUsersData();
        }


        private async Task LoadUsersData() // загружаем данные юзеров
        {
            try
            {
                // загружаем данные пользователей из бд и заносим в табличку

                var data = (await _api.GetUsersAsync()).ToList();
                userInfoTable.DataSource = data;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки данных: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void aUserForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            // уже обработали в предыдущей форме
        }

        private async void userInfoTable_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            // логика удаления юзеров
            // Проверяем, что кликнули по строке с данными (не по заголовку)
            if (e.RowIndex < 0) return;

            // Получаем выделенного пользователя
            var selectedUser = userInfoTable.Rows[e.RowIndex].DataBoundItem as UserDto;
            if (selectedUser == null) return;

            // Диалог подтверждения удаления
            var result = MessageBox.Show(
                $"Точно ли вы хотите удалить пользователя {selectedUser.Login} (ID: {selectedUser.UserId})?",
                "Подтверждение удаления",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            // Если пользователь подтвердил удаление
            if (result == DialogResult.Yes)
            {
                try
                {
                    // Вызываем метод удаления
                    bool success = await _api.DeleteUserAsync(selectedUser.UserId);

                    if (success)
                    {
                        MessageBox.Show("Пользователь успешно удален", "Успех",
                                      MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        MessageBox.Show("Не удалось удалить пользователя", "Ошибка",
                                      MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка при удалении: {ex.Message}", "Ошибка",
                                  MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

            }

            // В любом случае обновляем данные в таблице
            await LoadUsersData();
        }

        private async void refreshTable_Click(object sender, EventArgs e)
        {
            await LoadUsersData(); // Принудительное обновление данных
            MessageBox.Show("Данные обновлены", "Инфо", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private async void aUserForm_Shown(object sender, EventArgs e)
        {
            await LoadUsersData();
        }
    }
}
