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
            txtPass2.Location = new Point(31, 417);
            txtPass2.Name = "txtPass2";
            txtPass2.Size = new Size(225, 32);
            txtPass2.TabIndex = 5;
            // 
            // RegistrationForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 632);
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
    }
}