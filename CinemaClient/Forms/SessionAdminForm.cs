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
    public partial class SessionAdminForm : Form
    {
        private readonly ApiService _api;

        public SessionAdminForm(ApiService api)
        {
            InitializeComponent();
            _api = api;
        }
    }
}
