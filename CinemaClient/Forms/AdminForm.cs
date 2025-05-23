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
    public partial class AdminForm : Form
    {
        public AdminForm(ApiService _api)
        {
            InitializeComponent();
        }

        private void chOtmena_Click(object sender, EventArgs e)
        {

        }


        private void chUser_Click(object sender, EventArgs e)
        {
            var userForm = new aUserForm();
            userForm.FormClosed += (s, args) => this.Show(); // Возвращаем AdminForm после закрытия UserForm

            this.Hide(); // Скрываем текущую форму (AdminForm)
            userForm.Show(); // Открываем UserForm
        }

        private void chFilm_Click(object sender, EventArgs e)
        {

        }
    }
}
