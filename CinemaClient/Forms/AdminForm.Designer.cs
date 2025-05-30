﻿namespace CinemaClient.Forms
{
    partial class AdminForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            menuStrip1 = new MenuStrip();
            chFilm = new ToolStripMenuItem();
            sessionWork = new ToolStripMenuItem();
            label1 = new Label();
            label2 = new Label();
            label3 = new Label();
            menuStrip1.SuspendLayout();
            SuspendLayout();
            // 
            // menuStrip1
            // 
            menuStrip1.ImageScalingSize = new Size(24, 24);
            menuStrip1.Items.AddRange(new ToolStripItem[] { chFilm, sessionWork });
            menuStrip1.Location = new Point(0, 0);
            menuStrip1.Name = "menuStrip1";
            menuStrip1.Size = new Size(724, 30);
            menuStrip1.TabIndex = 0;
            menuStrip1.Text = "menuStrip1";
            // 
            // chFilm
            // 
            chFilm.BackColor = Color.PaleVioletRed;
            chFilm.Font = new Font("Bahnschrift", 9F);
            chFilm.ForeColor = Color.Black;
            chFilm.Name = "chFilm";
            chFilm.Size = new Size(94, 26);
            chFilm.Text = "Фильмы";
            chFilm.Click += chFilm_Click;
            // 
            // sessionWork
            // 
            sessionWork.BackColor = Color.PaleVioletRed;
            sessionWork.Font = new Font("Bahnschrift", 9F);
            sessionWork.ForeColor = Color.Black;
            sessionWork.Name = "sessionWork";
            sessionWork.Size = new Size(89, 26);
            sessionWork.Text = "Сеансы";
            sessionWork.TextDirection = ToolStripTextDirection.Horizontal;
            sessionWork.Click += sessionWork_Click;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Bahnschrift Condensed", 22F, FontStyle.Bold, GraphicsUnit.Point, 204);
            label1.ForeColor = Color.PaleVioletRed;
            label1.Location = new Point(10, 34);
            label1.Margin = new Padding(2, 0, 2, 0);
            label1.Name = "label1";
            label1.Size = new Size(587, 45);
            label1.TabIndex = 1;
            label1.Text = "Добро пожаловать на форму Администратора!";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Font = new Font("Bahnschrift SemiBold", 12F, FontStyle.Bold, GraphicsUnit.Point, 204);
            label2.ForeColor = Color.Maroon;
            label2.Location = new Point(67, 86);
            label2.Margin = new Padding(2, 0, 2, 0);
            label2.Name = "label2";
            label2.Size = new Size(483, 96);
            label2.TabIndex = 2;
            label2.Text = "В меню выше представлены несколько разделов, \r\nгде Вы можете взаимодействовать\r\nс информацией о пользователях, фильмах,\r\nа также о билетах и их состоянии";
            label2.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Font = new Font("Bahnschrift Condensed", 14F, FontStyle.Bold, GraphicsUnit.Point, 204);
            label3.ForeColor = Color.PaleVioletRed;
            label3.Location = new Point(216, 191);
            label3.Margin = new Padding(2, 0, 2, 0);
            label3.Name = "label3";
            label3.Size = new Size(193, 29);
            label3.TabIndex = 3;
            label3.Text = "Продуктивной работы!";
            label3.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // AdminForm
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            AutoSizeMode = AutoSizeMode.GrowAndShrink;
            BackColor = Color.AntiqueWhite;
            ClientSize = new Size(579, 244);
            Controls.Add(label3);
            Controls.Add(label2);
            Controls.Add(label1);
            Controls.Add(menuStrip1);
            MainMenuStrip = menuStrip1;
            Margin = new Padding(2, 2, 2, 2);
            Name = "AdminForm";
            Text = "Режим администратора";
            menuStrip1.ResumeLayout(false);
            menuStrip1.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private MenuStrip menuStrip1;
        private ToolStripMenuItem chFilm;
        private ToolStripMenuItem sessionWork;
        private Label label1;
        private Label label2;
        private Label label3;
    }
}