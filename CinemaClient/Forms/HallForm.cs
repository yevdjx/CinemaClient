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
    public partial class HallForm : Form
    {
        private readonly ApiService _api;
        private readonly int _sessionId;

        public HallForm(ApiService api, int sessionId, string movieTitle)
        {
            InitializeComponent();
            _api = api;
            _sessionId = sessionId;
            Text = $"Зал – {movieTitle}";
        }

        private async void HallForm_Load(object sender, EventArgs e)
        {
            var seats = (await _api.GetSeatsAsync(_sessionId)).ToList();
            int rows = seats.Max(s => s.Row);
            int cols = seats.Max(s => s.Number);

            tableLayoutPanel1.RowCount = rows;
            tableLayoutPanel1.ColumnCount = cols;

            foreach (var seat in seats)
            {
                var btn = new Button
                {
                    Text = seat.Number.ToString(),
                    Dock = DockStyle.Fill,
                    Tag = seat      // чтобы знать ticketId
                };
                btn.BackColor = seat.Status switch
                {
                    "sold" => Color.Red,
                    "booked" => Color.Gold,
                    _ => Color.LightGreen
                };
                btn.Enabled = seat.Status == "free";
                btn.Click += Seat_Click;
                tableLayoutPanel1.Controls.Add(btn, seat.Number - 1, seat.Row - 1);
            }
        }

        private async void Seat_Click(object? sender, EventArgs e)
        {
            var seat = (SeatDto)((Button)sender!).Tag;
            if (await _api.BookAsync(seat.TicketId) == 0)
            {
                MessageBox.Show($"Место {seat.Row}-{seat.Number} забронировано!");
                ((Button)sender).BackColor = Color.Gold;
                ((Button)sender).Enabled = false;
            }
            else
                MessageBox.Show("Не удалось забронировать. Место, возможно, занято.");
        }
    }

}
