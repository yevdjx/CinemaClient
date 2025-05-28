namespace CinemaClient.Forms
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
            comboHall = new ComboBox();
            comboFilm = new ComboBox();
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
            // takeTime
            // 
            takeTime.CalendarMonthBackground = Color.Pink;
            takeTime.CalendarTitleBackColor = Color.Maroon;
            takeTime.Location = new Point(293, 218);
            takeTime.Name = "takeTime";
            takeTime.Size = new Size(329, 31);
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
            takePrice.Location = new Point(293, 290);
            takePrice.Name = "takePrice";
            takePrice.Size = new Size(329, 36);
            takePrice.TabIndex = 11;
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Font = new Font("Bahnschrift Condensed", 14F, FontStyle.Regular, GraphicsUnit.Point, 204);
            label6.ForeColor = Color.DeepPink;
            label6.Location = new Point(34, 292);
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
            // comboHall
            // 
            comboHall.BackColor = Color.Pink;
            comboHall.FormattingEnabled = true;
            comboHall.Location = new Point(293, 67);
            comboHall.Name = "comboHall";
            comboHall.Size = new Size(329, 33);
            comboHall.TabIndex = 17;
            // 
            // comboFilm
            // 
            comboFilm.BackColor = Color.Pink;
            comboFilm.FormattingEnabled = true;
            comboFilm.Location = new Point(293, 153);
            comboFilm.Name = "comboFilm";
            comboFilm.Size = new Size(329, 33);
            comboFilm.TabIndex = 18;
            // 
            // SessionAdminForm
            // 
            AutoScaleDimensions = new SizeF(10F, 25F);
            AutoScaleMode = AutoScaleMode.Font;
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
    }
}