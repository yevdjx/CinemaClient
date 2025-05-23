namespace CinemaClient.Forms
{
    partial class LoginForm
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
            btnLogin = new Button();
            txtLogin = new TextBox();
            txtPassword = new TextBox();
            label1 = new Label();
            label2 = new Label();
            btnReg = new Button();
            SuspendLayout();
            // 
            // btnLogin
            // 
            btnLogin.Font = new Font("Times New Roman", 15.75F);
            btnLogin.Location = new Point(176, 400);
            btnLogin.Name = "btnLogin";
            btnLogin.Size = new Size(209, 55);
            btnLogin.TabIndex = 0;
            btnLogin.Text = "Вход";
            btnLogin.UseVisualStyleBackColor = true;
            btnLogin.Click += btnLogin_Click;
            // 
            // txtLogin
            // 
            txtLogin.Font = new Font("Times New Roman", 15.75F);
            txtLogin.Location = new Point(115, 202);
            txtLogin.Name = "txtLogin";
            txtLogin.Size = new Size(151, 32);
            txtLogin.TabIndex = 1;
            // 
            // txtPassword
            // 
            txtPassword.Font = new Font("Times New Roman", 15.75F);
            txtPassword.Location = new Point(115, 282);
            txtPassword.Name = "txtPassword";
            txtPassword.Size = new Size(151, 32);
            txtPassword.TabIndex = 2;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Times New Roman", 15.75F);
            label1.Location = new Point(115, 176);
            label1.Name = "label1";
            label1.Size = new Size(65, 23);
            label1.TabIndex = 3;
            label1.Text = "Логин";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Font = new Font("Times New Roman", 15.75F);
            label2.Location = new Point(115, 256);
            label2.Name = "label2";
            label2.Size = new Size(75, 23);
            label2.TabIndex = 4;
            label2.Text = "Пароль";
            // 
            // btnReg
            // 
            btnReg.Font = new Font("Times New Roman", 15.75F);
            btnReg.Location = new Point(176, 480);
            btnReg.Name = "btnReg";
            btnReg.Size = new Size(209, 55);
            btnReg.TabIndex = 5;
            btnReg.Text = " Регистрация";
            btnReg.UseVisualStyleBackColor = true;
            btnReg.Click += btnReg_Click;
            // 
            // LoginForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(721, 608);
            Controls.Add(btnReg);
            Controls.Add(label2);
            Controls.Add(label1);
            Controls.Add(txtPassword);
            Controls.Add(txtLogin);
            Controls.Add(btnLogin);
            Name = "LoginForm";
            Text = "LoginForm";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Button btnLogin;
        private TextBox txtLogin;
        private TextBox txtPassword;
        private Label label1;
        private Label label2;
        private Button btnReg;
    }
}