namespace CinemaClient.Forms
{
    partial class RegistrationForm
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
            txtFirst = new TextBox();
            txtLast = new TextBox();
            txtEmail = new TextBox();
            txtLogin = new TextBox();
            txtPass1 = new TextBox();
            txtPass2 = new TextBox();
            label1 = new Label();
            label2 = new Label();
            label3 = new Label();
            label4 = new Label();
            label5 = new Label();
            label6 = new Label();
            lblMatch = new Label();
            BtnRegister = new Button();
            lblRegiction = new Label();
            SuspendLayout();
            // 
            // txtFirst
            // 
            txtFirst.BackColor = Color.PaleVioletRed;
            txtFirst.Font = new Font("Bahnschrift Condensed", 16.2F, FontStyle.Bold);
            txtFirst.Location = new Point(111, 155);
            txtFirst.Margin = new Padding(3, 4, 3, 4);
            txtFirst.Name = "txtFirst";
            txtFirst.Size = new Size(216, 40);
            txtFirst.TabIndex = 0;
            // 
            // txtLast
            // 
            txtLast.BackColor = Color.PaleVioletRed;
            txtLast.Font = new Font("Bahnschrift Condensed", 16.2F, FontStyle.Bold);
            txtLast.Location = new Point(111, 251);
            txtLast.Margin = new Padding(3, 4, 3, 4);
            txtLast.Name = "txtLast";
            txtLast.Size = new Size(216, 40);
            txtLast.TabIndex = 1;
            // 
            // txtEmail
            // 
            txtEmail.BackColor = Color.PaleVioletRed;
            txtEmail.Font = new Font("Bahnschrift Condensed", 16.2F, FontStyle.Bold);
            txtEmail.Location = new Point(111, 431);
            txtEmail.Margin = new Padding(3, 4, 3, 4);
            txtEmail.Name = "txtEmail";
            txtEmail.Size = new Size(216, 40);
            txtEmail.TabIndex = 3;
            // 
            // txtLogin
            // 
            txtLogin.BackColor = Color.PaleVioletRed;
            txtLogin.Font = new Font("Bahnschrift Condensed", 16.2F, FontStyle.Bold);
            txtLogin.Location = new Point(111, 345);
            txtLogin.Margin = new Padding(3, 4, 3, 4);
            txtLogin.Name = "txtLogin";
            txtLogin.Size = new Size(216, 40);
            txtLogin.TabIndex = 2;
            // 
            // txtPass1
            // 
            txtPass1.BackColor = Color.PaleVioletRed;
            txtPass1.Font = new Font("Bahnschrift Condensed", 16.2F, FontStyle.Bold);
            txtPass1.Location = new Point(111, 511);
            txtPass1.Margin = new Padding(3, 4, 3, 4);
            txtPass1.Name = "txtPass1";
            txtPass1.Size = new Size(216, 40);
            txtPass1.TabIndex = 4;
            txtPass1.KeyPress += txtPass1_KeyPress;
            // 
            // txtPass2
            // 
            txtPass2.BackColor = Color.PaleVioletRed;
            txtPass2.Font = new Font("Bahnschrift Condensed", 16.2F, FontStyle.Bold);
            txtPass2.Location = new Point(111, 607);
            txtPass2.Margin = new Padding(3, 4, 3, 4);
            txtPass2.Name = "txtPass2";
            txtPass2.Size = new Size(216, 40);
            txtPass2.TabIndex = 5;
            txtPass2.TextChanged += Pass2_TextChanged;
            txtPass2.KeyPress += txtPass2_KeyPress;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Bahnschrift Condensed", 18F, FontStyle.Bold);
            label1.ForeColor = Color.DeepPink;
            label1.Location = new Point(81, 117);
            label1.Name = "label1";
            label1.Size = new Size(53, 36);
            label1.TabIndex = 6;
            label1.Text = "Имя";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Font = new Font("Bahnschrift Condensed", 18F, FontStyle.Bold);
            label2.ForeColor = Color.DeepPink;
            label2.Location = new Point(81, 213);
            label2.Name = "label2";
            label2.Size = new Size(105, 36);
            label2.TabIndex = 7;
            label2.Text = "Фамилия";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Font = new Font("Bahnschrift Condensed", 18F, FontStyle.Bold);
            label3.ForeColor = Color.DeepPink;
            label3.Location = new Point(81, 307);
            label3.Name = "label3";
            label3.Size = new Size(72, 36);
            label3.TabIndex = 8;
            label3.Text = "Логин";
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Font = new Font("Bahnschrift Condensed", 18F, FontStyle.Bold);
            label4.ForeColor = Color.DeepPink;
            label4.Location = new Point(81, 393);
            label4.Name = "label4";
            label4.Size = new Size(72, 36);
            label4.TabIndex = 9;
            label4.Text = "Почта";
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Font = new Font("Bahnschrift Condensed", 18F, FontStyle.Bold);
            label5.ForeColor = Color.DeepPink;
            label5.Location = new Point(81, 473);
            label5.Name = "label5";
            label5.Size = new Size(86, 36);
            label5.TabIndex = 10;
            label5.Text = "Пароль";
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Font = new Font("Bahnschrift Condensed", 18F, FontStyle.Bold);
            label6.ForeColor = Color.DeepPink;
            label6.Location = new Point(81, 569);
            label6.Name = "label6";
            label6.Size = new Size(157, 36);
            label6.TabIndex = 11;
            label6.Text = "Повтор пароля";
            // 
            // lblMatch
            // 
            lblMatch.AutoSize = true;
            lblMatch.Font = new Font("Bahnschrift Condensed", 18F, FontStyle.Bold);
            lblMatch.ForeColor = Color.DeepPink;
            lblMatch.Location = new Point(81, 651);
            lblMatch.Name = "lblMatch";
            lblMatch.Size = new Size(157, 36);
            lblMatch.TabIndex = 12;
            lblMatch.Text = "Повтор пароля";
            // 
            // BtnRegister
            // 
            BtnRegister.BackColor = Color.PaleVioletRed;
            BtnRegister.Dock = DockStyle.Bottom;
            BtnRegister.Font = new Font("Bahnschrift Condensed", 16.2F, FontStyle.Bold, GraphicsUnit.Point, 204);
            BtnRegister.Location = new Point(0, 721);
            BtnRegister.Margin = new Padding(3, 4, 3, 4);
            BtnRegister.Name = "BtnRegister";
            BtnRegister.Size = new Size(450, 89);
            BtnRegister.TabIndex = 13;
            BtnRegister.Text = "Зарегистрироваться";
            BtnRegister.UseVisualStyleBackColor = false;
            BtnRegister.Click += BtnRegister_Click;
            // 
            // lblRegiction
            // 
            lblRegiction.AutoSize = true;
            lblRegiction.Font = new Font("Bahnschrift Condensed", 48F, FontStyle.Bold, GraphicsUnit.Point, 204);
            lblRegiction.ForeColor = Color.DeepPink;
            lblRegiction.Location = new Point(37, 9);
            lblRegiction.Name = "lblRegiction";
            lblRegiction.Size = new Size(375, 96);
            lblRegiction.TabIndex = 14;
            lblRegiction.Text = "Регистрация";
            // 
            // RegistrationForm
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.AntiqueWhite;
            ClientSize = new Size(450, 810);
            Controls.Add(lblRegiction);
            Controls.Add(BtnRegister);
            Controls.Add(lblMatch);
            Controls.Add(label6);
            Controls.Add(label5);
            Controls.Add(label4);
            Controls.Add(label3);
            Controls.Add(label2);
            Controls.Add(label1);
            Controls.Add(txtPass2);
            Controls.Add(txtPass1);
            Controls.Add(txtEmail);
            Controls.Add(txtLogin);
            Controls.Add(txtLast);
            Controls.Add(txtFirst);
            Margin = new Padding(3, 4, 3, 4);
            Name = "RegistrationForm";
            Text = "RegistrationForm";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private TextBox txtFirst;
        private TextBox txtLast;
        private TextBox txtEmail;
        private TextBox txtLogin;
        private TextBox txtPass1;
        private TextBox txtPass2;
        private Label label1;
        private Label label2;
        private Label label3;
        private Label label4;
        private Label label5;
        private Label label6;
        private Label lblMatch;
        private Button BtnRegister;
        private Label lblRegiction;
    }
}