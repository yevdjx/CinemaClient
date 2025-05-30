﻿namespace CinemaClient.Forms
{
    partial class SessionAdminForm
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
            groupBox2 = new GroupBox();
            label1 = new Label();
            sessionList = new DataGridView();
            groupBox1 = new GroupBox();
            label10 = new Label();
            label9 = new Label();
            label8 = new Label();
            label7 = new Label();
            tMinutes = new TextBox();
            tHours = new TextBox();
            comboFilm = new ComboBox();
            comboHall = new ComboBox();
            takeTime = new DateTimePicker();
            delButton = new Button();
            otmenButton = new Button();
            sohrButton = new Button();
            takePrice = new TextBox();
            label6 = new Label();
            label5 = new Label();
            label4 = new Label();
            label3 = new Label();
            label2 = new Label();
            groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)sessionList).BeginInit();
            groupBox1.SuspendLayout();
            SuspendLayout();
            // 
            // groupBox2
            // 
            groupBox2.Controls.Add(label1);
            groupBox2.Controls.Add(sessionList);
            groupBox2.Location = new Point(12, 428);
            groupBox2.Name = "groupBox2";
            groupBox2.Size = new Size(1006, 308);
            groupBox2.TabIndex = 6;
            groupBox2.TabStop = false;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Bahnschrift SemiBold Condensed", 16F, FontStyle.Bold, GraphicsUnit.Point, 204);
            label1.ForeColor = Color.DeepPink;
            label1.Location = new Point(8, 0);
            label1.Name = "label1";
            label1.Size = new Size(190, 39);
            label1.TabIndex = 1;
            label1.Text = "Список сеансов:";
            // 
            // sessionList
            // 
            sessionList.BackgroundColor = Color.BlanchedAlmond;
            sessionList.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            sessionList.ColumnHeadersVisible = false;
            sessionList.Location = new Point(26, 77);
            sessionList.Name = "sessionList";
            sessionList.RowHeadersVisible = false;
            sessionList.RowHeadersWidth = 62;
            sessionList.Size = new Size(954, 202);
            sessionList.TabIndex = 0;
            // 
            // groupBox1
            // 
            groupBox1.Controls.Add(label10);
            groupBox1.Controls.Add(label9);
            groupBox1.Controls.Add(label8);
            groupBox1.Controls.Add(label7);
            groupBox1.Controls.Add(tMinutes);
            groupBox1.Controls.Add(tHours);
            groupBox1.Controls.Add(comboFilm);
            groupBox1.Controls.Add(comboHall);
            groupBox1.Controls.Add(takeTime);
            groupBox1.Controls.Add(delButton);
            groupBox1.Controls.Add(otmenButton);
            groupBox1.Controls.Add(sohrButton);
            groupBox1.Controls.Add(takePrice);
            groupBox1.Controls.Add(label6);
            groupBox1.Controls.Add(label5);
            groupBox1.Controls.Add(label4);
            groupBox1.Controls.Add(label3);
            groupBox1.Controls.Add(label2);
            groupBox1.Location = new Point(12, 21);
            groupBox1.Name = "groupBox1";
            groupBox1.Size = new Size(1006, 401);
            groupBox1.TabIndex = 5;
            groupBox1.TabStop = false;
            // 
            // label10
            // 
            label10.AutoSize = true;
            label10.Font = new Font("Bahnschrift Condensed", 14F, FontStyle.Regular, GraphicsUnit.Point, 204);
            label10.ForeColor = Color.DeepPink;
            label10.Location = new Point(545, 271);
            label10.Name = "label10";
            label10.Size = new Size(49, 34);
            label10.TabIndex = 24;
            label10.Text = "мин";
            // 
            // label9
            // 
            label9.AutoSize = true;
            label9.Font = new Font("Bahnschrift Condensed", 14F, FontStyle.Regular, GraphicsUnit.Point, 204);
            label9.ForeColor = Color.DeepPink;
            label9.Location = new Point(421, 270);
            label9.Name = "label9";
            label9.Size = new Size(26, 34);
            label9.TabIndex = 23;
            label9.Text = "ч";
            // 
            // label8
            // 
            label8.AutoSize = true;
            label8.Font = new Font("Bahnschrift Condensed", 14F, FontStyle.Regular, GraphicsUnit.Point, 204);
            label8.ForeColor = Color.DeepPink;
            label8.Location = new Point(453, 271);
            label8.Name = "label8";
            label8.Size = new Size(20, 34);
            label8.TabIndex = 22;
            label8.Text = ":";
            // 
            // label7
            // 
            label7.AutoSize = true;
            label7.Location = new Point(227, 296);
            label7.Name = "label7";
            label7.Size = new Size(0, 25);
            label7.TabIndex = 21;
            // 
            // tMinutes
            // 
            tMinutes.BackColor = Color.Pink;
            tMinutes.Font = new Font("Bahnschrift", 12F, FontStyle.Regular, GraphicsUnit.Point, 204);
            tMinutes.ForeColor = Color.Maroon;
            tMinutes.Location = new Point(488, 268);
            tMinutes.Name = "tMinutes";
            tMinutes.Size = new Size(60, 36);
            tMinutes.TabIndex = 20;
            tMinutes.TextAlign = HorizontalAlignment.Center;
            // 
            // tHours
            // 
            tHours.BackColor = Color.Pink;
            tHours.Font = new Font("Bahnschrift", 12F, FontStyle.Regular, GraphicsUnit.Point, 204);
            tHours.ForeColor = Color.Maroon;
            tHours.Location = new Point(355, 268);
            tHours.Name = "tHours";
            tHours.Size = new Size(60, 36);
            tHours.TabIndex = 19;
            tHours.TextAlign = HorizontalAlignment.Center;
            // 
            // comboFilm
            // 
            comboFilm.BackColor = Color.Pink;
            comboFilm.Font = new Font("Bahnschrift", 12F, FontStyle.Regular, GraphicsUnit.Point, 204);
            comboFilm.ForeColor = Color.Maroon;
            comboFilm.FormattingEnabled = true;
            comboFilm.Location = new Point(293, 148);
            comboFilm.Name = "comboFilm";
            comboFilm.Size = new Size(329, 37);
            comboFilm.TabIndex = 18;
            // 
            // comboHall
            // 
            comboHall.BackColor = Color.Pink;
            comboHall.Font = new Font("Bahnschrift", 12F, FontStyle.Regular, GraphicsUnit.Point, 204);
            comboHall.ForeColor = Color.Maroon;
            comboHall.FormattingEnabled = true;
            comboHall.Location = new Point(293, 77);
            comboHall.Name = "comboHall";
            comboHall.Size = new Size(329, 37);
            comboHall.TabIndex = 17;
            // 
            // takeTime
            // 
            takeTime.CalendarFont = new Font("Bahnschrift SemiCondensed", 10F, FontStyle.Bold, GraphicsUnit.Point, 204);
            takeTime.CalendarForeColor = Color.Maroon;
            takeTime.CalendarMonthBackground = Color.Pink;
            takeTime.CalendarTitleBackColor = Color.Maroon;
            takeTime.Font = new Font("Bahnschrift Light", 12F, FontStyle.Regular, GraphicsUnit.Point, 204);
            takeTime.Location = new Point(293, 213);
            takeTime.Name = "takeTime";
            takeTime.Size = new Size(329, 36);
            takeTime.TabIndex = 15;
            // 
            // delButton
            // 
            delButton.BackColor = Color.MistyRose;
            delButton.Font = new Font("Bahnschrift SemiBold Condensed", 12F, FontStyle.Bold);
            delButton.ForeColor = Color.Maroon;
            delButton.Location = new Point(737, 279);
            delButton.Name = "delButton";
            delButton.Size = new Size(209, 67);
            delButton.TabIndex = 14;
            delButton.Text = "Удалить";
            delButton.UseVisualStyleBackColor = false;
            // 
            // otmenButton
            // 
            otmenButton.BackColor = Color.MistyRose;
            otmenButton.Font = new Font("Bahnschrift SemiBold Condensed", 12F, FontStyle.Bold);
            otmenButton.ForeColor = Color.Maroon;
            otmenButton.Location = new Point(737, 163);
            otmenButton.Name = "otmenButton";
            otmenButton.Size = new Size(209, 67);
            otmenButton.TabIndex = 13;
            otmenButton.Text = "Отменить изменения";
            otmenButton.UseVisualStyleBackColor = false;
            // 
            // sohrButton
            // 
            sohrButton.BackColor = Color.MistyRose;
            sohrButton.Font = new Font("Bahnschrift SemiBold Condensed", 12F, FontStyle.Bold);
            sohrButton.ForeColor = Color.Maroon;
            sohrButton.Location = new Point(737, 47);
            sohrButton.Name = "sohrButton";
            sohrButton.Size = new Size(209, 67);
            sohrButton.TabIndex = 12;
            sohrButton.Text = "Сохранить";
            sohrButton.UseVisualStyleBackColor = false;
            // 
            // takePrice
            // 
            takePrice.BackColor = Color.Pink;
            takePrice.Font = new Font("Bahnschrift", 12F, FontStyle.Regular, GraphicsUnit.Point, 204);
            takePrice.ForeColor = Color.Maroon;
            takePrice.Location = new Point(293, 326);
            takePrice.Name = "takePrice";
            takePrice.Size = new Size(329, 36);
            takePrice.TabIndex = 11;
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Font = new Font("Bahnschrift Condensed", 14F, FontStyle.Regular, GraphicsUnit.Point, 204);
            label6.ForeColor = Color.DeepPink;
            label6.Location = new Point(34, 328);
            label6.Name = "label6";
            label6.Size = new Size(135, 34);
            label6.TabIndex = 6;
            label6.Text = "Цена билета:";
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Font = new Font("Bahnschrift Condensed", 14F, FontStyle.Regular, GraphicsUnit.Point, 204);
            label5.ForeColor = Color.DeepPink;
            label5.Location = new Point(34, 215);
            label5.Name = "label5";
            label5.Size = new Size(147, 34);
            label5.TabIndex = 5;
            label5.Text = "Время показа:";
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Font = new Font("Bahnschrift Condensed", 14F, FontStyle.Regular, GraphicsUnit.Point, 204);
            label4.ForeColor = Color.DeepPink;
            label4.Location = new Point(34, 148);
            label4.Name = "label4";
            label4.Size = new Size(180, 34);
            label4.TabIndex = 4;
            label4.Text = "Название фильма";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Font = new Font("Bahnschrift Condensed", 14F, FontStyle.Regular, GraphicsUnit.Point, 204);
            label3.ForeColor = Color.DeepPink;
            label3.Location = new Point(34, 80);
            label3.Name = "label3";
            label3.Size = new Size(120, 34);
            label3.TabIndex = 3;
            label3.Text = "Номер зала";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Font = new Font("Bahnschrift SemiBold Condensed", 16F, FontStyle.Bold, GraphicsUnit.Point, 204);
            label2.ForeColor = Color.DeepPink;
            label2.Location = new Point(6, 0);
            label2.Name = "label2";
            label2.Size = new Size(81, 39);
            label2.TabIndex = 2;
            label2.Text = "Сеанс";
            // 
            // SessionAdminForm
            // 
            AutoScaleDimensions = new SizeF(10F, 25F);
            AutoScaleMode = AutoScaleMode.Font;
            AutoSizeMode = AutoSizeMode.GrowAndShrink;
            BackColor = Color.AntiqueWhite;
            ClientSize = new Size(1040, 735);
            Controls.Add(groupBox2);
            Controls.Add(groupBox1);
            Name = "SessionAdminForm";
            Text = "Сеансы";
            groupBox2.ResumeLayout(false);
            groupBox2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)sessionList).EndInit();
            groupBox1.ResumeLayout(false);
            groupBox1.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private GroupBox groupBox2;
        private Label label1;
        private DataGridView sessionList;
        private GroupBox groupBox1;
        private Button delButton;
        private Button otmenButton;
        private Button sohrButton;
        private TextBox takePrice;
        private TextBox takeDir;
        private TextBox takeProd;
        private Label label6;
        private Label label5;
        private Label label4;
        private Label label3;
        private Label label2;
        private DateTimePicker takeTime;
        private ComboBox comboFilm;
        private ComboBox comboHall;
        private Label label8;
        private Label label7;
        private TextBox tMinutes;
        private TextBox tHours;
        private Label label10;
        private Label label9;
    }
}