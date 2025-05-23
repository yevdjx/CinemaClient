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
    public partial class SessionsForm : Form
    {
        private readonly ApiService _api;

        public SessionsForm(ApiService api)
        {
            InitializeComponent();
            _api = api;
        }

        private async void SessionsForm_Load(object sender, EventArgs e)
        {
            var data = (await _api.GetSessionsAsync()).ToList();
            dgvSessions.DataSource = data;
        }

        private void dgvSessions_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;
            var session = (SessionDto)dgvSessions.Rows[e.RowIndex].DataBoundItem;
            new HallForm(_api, session.SessionId, session.MovieTitle).Show();
        }
    }

}
