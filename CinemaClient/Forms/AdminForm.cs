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
        private readonly ApiService _api;
        public AdminForm(ApiService api)
        {
            InitializeComponent();
            _api = api;
        }

        private void sessionWork_Click(object sender, EventArgs e)
        {

        }

        private void chFilm_Click(object sender, EventArgs e)
        {
            var f = new FilmForm(_api);   // передаём тот же ApiService
            f.ShowDialog(this);
        }
    }
}
