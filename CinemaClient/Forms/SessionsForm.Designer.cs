namespace CinemaClient.Forms
{
    partial class SessionsForm
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
            dgvSessions = new DataGridView();
            ((System.ComponentModel.ISupportInitialize)dgvSessions).BeginInit();
            SuspendLayout();
            // 
            // dgvSessions
            // 
            dgvSessions.BackgroundColor = Color.Pink;
            dgvSessions.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvSessions.GridColor = Color.PaleVioletRed;
            dgvSessions.Location = new Point(48, 41);
            dgvSessions.Margin = new Padding(3, 4, 3, 4);
            dgvSessions.Name = "dgvSessions";
            dgvSessions.RowHeadersWidth = 51;
            dgvSessions.Size = new Size(737, 516);
            dgvSessions.TabIndex = 0;
            dgvSessions.CellContentDoubleClick += dgvSessions_CellDoubleClick;
            // 
            // SessionsForm
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.AntiqueWhite;
            ClientSize = new Size(914, 600);
            Controls.Add(dgvSessions);
            Margin = new Padding(3, 4, 3, 4);
            Name = "SessionsForm";
            Text = "SessionsForm";
            Load += SessionsForm_Load;
            ((System.ComponentModel.ISupportInitialize)dgvSessions).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private DataGridView dgvSessions;
    }
}