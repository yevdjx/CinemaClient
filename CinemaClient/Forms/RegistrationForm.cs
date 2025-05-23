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
    public partial class RegistrationForm : Form
    {
        private readonly ApiService _api;
        public RegistrationForm(ApiService api)
        {
            InitializeComponent();
            _api = api;
        }
        private void Pass2_TextChanged(object? sender, EventArgs e) 
        {
            bool ok = txtPass1.Text == txtPass2.Text;
            lblMatch.Text = ok ? "" : "Пароли не совпадают";
            lblMatch.ForeColor = ok ? Color.Green : Color.Red;
        }

        private async void BtnRegister_Click(object? sender, EventArgs e)
        {
            if (txtPass1.Text != txtPass2.Text)
            {
                MessageBox.Show("Введённые пароли не совпадают!");
                return;
            }

            string? err = await _api.RegisterAsync(
                txtLogin.Text, txtPass1.Text, txtPass2.Text,
                txtFirst.Text, txtLast.Text, txtEmail.Text);

            if (err == null)
            {
                MessageBox.Show("Регистрация прошла успешно!");
                Close();
            }
            else
                MessageBox.Show(err);
        }

    }
}
