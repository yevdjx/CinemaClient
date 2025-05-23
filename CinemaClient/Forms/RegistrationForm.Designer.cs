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
            SuspendLayout();
            // 
            // txtFirst
            // 
            txtFirst.Font = new Font("Times New Roman", 15.75F, FontStyle.Regular, GraphicsUnit.Point, 204);
            txtFirst.Location = new Point(31, 97);
            txtFirst.Name = "txtFirst";
            txtFirst.Size = new Size(225, 32);
            txtFirst.TabIndex = 0;
            // 
            // txtLast
            // 
            txtLast.Font = new Font("Times New Roman", 15.75F, FontStyle.Regular, GraphicsUnit.Point, 204);
            txtLast.Location = new Point(31, 165);
            txtLast.Name = "txtLast";
            txtLast.Size = new Size(225, 32);
            txtLast.TabIndex = 1;
            // 
            // txtEmail
            // 
            txtEmail.Font = new Font("Times New Roman", 15.75F, FontStyle.Regular, GraphicsUnit.Point, 204);
            txtEmail.Location = new Point(31, 296);
            txtEmail.Name = "txtEmail";
            txtEmail.Size = new Size(225, 32);
            txtEmail.TabIndex = 3;
            // 
            // txtLogin
            // 
            txtLogin.Font = new Font("Times New Roman", 15.75F, FontStyle.Regular, GraphicsUnit.Point, 204);
            txtLogin.Location = new Point(31, 228);
            txtLogin.Name = "txtLogin";
            txtLogin.Size = new Size(225, 32);
            txtLogin.TabIndex = 2;
            // 
            // txtPass1
            // 
            txtPass1.Font = new Font("Times New Roman", 15.75F, FontStyle.Regular, GraphicsUnit.Point, 204);
            txtPass1.Location = new Point(31, 363);
            txtPass1.Name = "txtPass1";
            txtPass1.Size = new Size(225, 32);
            txtPass1.TabIndex = 4;
            // 
            // txtPass2
            // 
            txtPass2.Font = new Font("Times New Roman", 15.75F, FontStyle.Regular, GraphicsUnit.Point, 204);
            txtPass2.Location = new Point(31, 433);
            txtPass2.Name = "txtPass2";
            txtPass2.Size = new Size(225, 32);
            txtPass2.TabIndex = 5;
            txtPass2.TextChanged += Pass2_TextChanged;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Times New Roman", 15.75F, FontStyle.Regular, GraphicsUnit.Point, 204);
            label1.Location = new Point(31, 71);
            label1.Name = "label1";
            label1.Size = new Size(48, 23);
            label1.TabIndex = 6;
            label1.Text = "Имя";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Font = new Font("Times New Roman", 15.75F, FontStyle.Regular, GraphicsUnit.Point, 204);
            label2.Location = new Point(31, 139);
            label2.Name = "label2";
            label2.Size = new Size(91, 23);
            label2.TabIndex = 7;
            label2.Text = "Фамилия";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Font = new Font("Times New Roman", 15.75F, FontStyle.Regular, GraphicsUnit.Point, 204);
            label3.Location = new Point(31, 202);
            label3.Name = "label3";
            label3.Size = new Size(65, 23);
            label3.TabIndex = 8;
            label3.Text = "Логин";
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Font = new Font("Times New Roman", 15.75F, FontStyle.Regular, GraphicsUnit.Point, 204);
            label4.Location = new Point(31, 270);
            label4.Name = "label4";
            label4.Size = new Size(64, 23);
            label4.TabIndex = 9;
            label4.Text = "Почта";
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Font = new Font("Times New Roman", 15.75F, FontStyle.Regular, GraphicsUnit.Point, 204);
            label5.Location = new Point(31, 337);
            label5.Name = "label5";
            label5.Size = new Size(75, 23);
            label5.TabIndex = 10;
            label5.Text = "Пароль";
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Font = new Font("Times New Roman", 15.75F, FontStyle.Regular, GraphicsUnit.Point, 204);
            label6.Location = new Point(31, 407);
            label6.Name = "label6";
            label6.Size = new Size(141, 23);
            label6.TabIndex = 11;
            label6.Text = "Повтор пароля";
            // 
            // lblMatch
            // 
            lblMatch.AutoSize = true;
            lblMatch.Font = new Font("Times New Roman", 15.75F, FontStyle.Regular, GraphicsUnit.Point, 204);
            lblMatch.Location = new Point(292, 407);
            lblMatch.Name = "lblMatch";
            lblMatch.Size = new Size(141, 23);
            lblMatch.TabIndex = 12;
            lblMatch.Text = "Повтор пароля";
            // 
            // BtnRegister
            // 
            BtnRegister.Font = new Font("Times New Roman", 15.75F, FontStyle.Regular, GraphicsUnit.Point, 204);
            BtnRegister.Location = new Point(430, 529);
            BtnRegister.Name = "BtnRegister";
            BtnRegister.Size = new Size(229, 67);
            BtnRegister.TabIndex = 13;
            BtnRegister.Text = "Зарегистрироваться";
            BtnRegister.UseVisualStyleBackColor = true;
            BtnRegister.Click += BtnRegister_Click;
            // 
            // RegistrationForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 632);
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
    }
}