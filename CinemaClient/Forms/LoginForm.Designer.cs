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
            AgoKino = new Label();
            SuspendLayout();
            // 
            // btnLogin
            // 
            btnLogin.BackColor = Color.PaleVioletRed;
            btnLogin.Dock = DockStyle.Bottom;
            btnLogin.Font = new Font("Bahnschrift Condensed", 16.2F, FontStyle.Bold, GraphicsUnit.Point, 204);
            btnLogin.Location = new Point(0, 707);
            btnLogin.Margin = new Padding(4, 5, 4, 5);
            btnLogin.Name = "btnLogin";
            btnLogin.Size = new Size(911, 91);
            btnLogin.TabIndex = 0;
            btnLogin.Text = "Вход";
            btnLogin.UseVisualStyleBackColor = false;
            btnLogin.Click += btnLogin_Click;
            // 
            // txtLogin
            // 
            txtLogin.BackColor = Color.PaleVioletRed;
            txtLogin.Font = new Font("Times New Roman", 15.75F);
            txtLogin.Location = new Point(281, 349);
            txtLogin.Margin = new Padding(4, 5, 4, 5);
            txtLogin.Name = "txtLogin";
            txtLogin.Size = new Size(319, 44);
            txtLogin.TabIndex = 1;
            // 
            // txtPassword
            // 
            txtPassword.BackColor = Color.PaleVioletRed;
            txtPassword.Font = new Font("Times New Roman", 15.75F);
            txtPassword.Location = new Point(281, 478);
            txtPassword.Margin = new Padding(4, 5, 4, 5);
            txtPassword.Name = "txtPassword";
            txtPassword.Size = new Size(319, 44);
            txtPassword.TabIndex = 2;
            txtPassword.KeyPress += txtPassword_KeyPress;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Bahnschrift Condensed", 16.2F, FontStyle.Bold, GraphicsUnit.Point, 204);
            label1.ForeColor = Color.DeepPink;
            label1.Location = new Point(281, 301);
            label1.Margin = new Padding(4, 0, 4, 0);
            label1.Name = "label1";
            label1.Size = new Size(81, 40);
            label1.TabIndex = 3;
            label1.Text = "Логин";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Font = new Font("Bahnschrift Condensed", 16.2F, FontStyle.Bold, GraphicsUnit.Point, 204);
            label2.ForeColor = Color.DeepPink;
            label2.Location = new Point(281, 434);
            label2.Margin = new Padding(4, 0, 4, 0);
            label2.Name = "label2";
            label2.Size = new Size(95, 40);
            label2.TabIndex = 4;
            label2.Text = "Пароль";
            // 
            // btnReg
            // 
            btnReg.BackColor = Color.PaleVioletRed;
            btnReg.Dock = DockStyle.Bottom;
            btnReg.Font = new Font("Bahnschrift Condensed", 16.2F, FontStyle.Bold, GraphicsUnit.Point, 204);
            btnReg.ForeColor = SystemColors.ControlText;
            btnReg.Location = new Point(0, 616);
            btnReg.Margin = new Padding(4, 5, 4, 5);
            btnReg.Name = "btnReg";
            btnReg.Size = new Size(911, 91);
            btnReg.TabIndex = 5;
            btnReg.Text = " Регистрация";
            btnReg.UseVisualStyleBackColor = false;
            btnReg.Click += btnReg_Click;
            // 
            // AgoKino
            // 
            AgoKino.AutoSize = true;
            AgoKino.Font = new Font("Bahnschrift Condensed", 90F, FontStyle.Bold, GraphicsUnit.Point, 204);
            AgoKino.ForeColor = Color.DeepPink;
            AgoKino.Location = new Point(149, 11);
            AgoKino.Margin = new Padding(4, 0, 4, 0);
            AgoKino.Name = "AgoKino";
            AgoKino.Size = new Size(620, 216);
            AgoKino.TabIndex = 6;
            AgoKino.Text = "AgroKino";
            // 
            // LoginForm
            // 
            AutoScaleDimensions = new SizeF(10F, 25F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.AntiqueWhite;
            ClientSize = new Size(911, 798);
            Controls.Add(AgoKino);
            Controls.Add(btnReg);
            Controls.Add(label2);
            Controls.Add(label1);
            Controls.Add(txtPassword);
            Controls.Add(txtLogin);
            Controls.Add(btnLogin);
            Margin = new Padding(4, 5, 4, 5);
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
        private Label AgoKino;
    }
}