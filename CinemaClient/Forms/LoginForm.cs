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
        private readonly ApiService _api;           // ★ поле

        // конструктор для Program.cs
        public LoginForm(ApiService api)
        {
            InitializeComponent();
            _api = api;
        }

        // ← если дизайнеру нужен параметрless-ctor, оставь его «для дизайнера»
        //    и сделай его protected / private, чтобы им не пользовались извне:
        private LoginForm() => InitializeComponent();

        private async void btnLogin_Click(object sender, EventArgs e)
        {
            bool ok = await _api.LoginAsync(txtLogin.Text, txtPassword.Text);
            if (ok)
            {
                var sesForm = new SessionsForm(_api);   // передаём сервис дальше
                sesForm.Show();
                Hide();
            }
            else
                MessageBox.Show("Неверный логин или пароль");
        }
    }
}
