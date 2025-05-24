using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using CinemaClient.Services;

namespace CinemaClient.Forms
{
    public partial class LoginForm : Form
    {
        private readonly ApiService _api;           // поле

        // конструктор для Program.cs
        public LoginForm(ApiService api)
        {
            InitializeComponent();
            _api = api;
        }

        private LoginForm() => InitializeComponent();

        private async void btnLogin_Click(object sender, EventArgs e)
        {
            //bool ok = await _api.LoginAsync(txtLogin.Text, txtPassword.Text);
            //if (ok)
            //{
            //    var sesForm = new SessionsForm(_api);   // передаём сервис дальше
            //    sesForm.Show();
            //    Hide();
            //}
            //else
            //    MessageBox.Show("Неверный логин или пароль");

            bool ok = await _api.LoginAsync(txtLogin.Text, txtPassword.Text);
            if (ok)
            {
                // Получаем роль пользователя
                string? role = _api.GetUserRole();

                // Создаем соответствующую форму
                Form nextForm = role switch
                {
                    "Admin" => new AdminForm(_api),      // Форма для администратора
                    "Customer" => new SessionsForm(_api),     // Существующая форма сеансов
                    _ => throw new Exception("Неизвестная роль пользователя")
                };

                // Настраиваем переход
                nextForm.FormClosed += (s, args) => this.Close();
                nextForm.Show();
                this.Hide();
            }
            else
            {
                MessageBox.Show("Неверный логин или пароль");
            }
        }

        private void btnReg_Click(object sender, EventArgs e)
        {
            var dlg = new RegistrationForm(_api);   // передаём тот же ApiService
            dlg.ShowDialog(this);
        }

        private void txtPassword_KeyPress(object sender, KeyPressEventArgs e)
        {
            txtPassword.UseSystemPasswordChar = true; // Это свойство заменяет символы точками
            txtPassword.PasswordChar = '•'; // Можно использовать любой символ вместо точек
        }
    }
}
