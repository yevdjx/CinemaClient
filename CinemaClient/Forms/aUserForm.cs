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
        ApiService _api;

        public aUserForm()
        {
            InitializeComponent();
        }

        private async void LoadUsersData() // загружаем данные юзеров
        {
            try
            {
                // загружаем данные пользователей из бд и заносим в табличку

                //var users = await _api.GetUsersAsync();
                //_usersBindingSource.DataSource = users.ToList();
                //dataGridViewUsers.Refresh();
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

        private void userInfoTable_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            // логика удаления юзеров
        }
    }
}
