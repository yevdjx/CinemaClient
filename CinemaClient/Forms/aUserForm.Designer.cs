namespace CinemaClient.Forms
{
    partial class aUserForm
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
            label2 = new Label();
            userInfoTable = new DataGridView();
            label3 = new Label();
            refreshTable = new Button();
            ((System.ComponentModel.ISupportInitialize)userInfoTable).BeginInit();
            SuspendLayout();
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Font = new Font("Bahnschrift SemiCondensed", 14F, FontStyle.Regular, GraphicsUnit.Point, 204);
            label2.ForeColor = Color.Maroon;
            label2.Location = new Point(15, 25);
            label2.Name = "label2";
            label2.Size = new Size(267, 34);
            label2.TabIndex = 1;
            label2.Text = "Список пользователей";
            // 
            // userInfoTable
            // 
            userInfoTable.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            userInfoTable.Location = new Point(15, 75);
            userInfoTable.Name = "userInfoTable";
            userInfoTable.RowHeadersWidth = 62;
            userInfoTable.Size = new Size(969, 548);
            userInfoTable.TabIndex = 2;
            userInfoTable.CellDoubleClick += userInfoTable_CellDoubleClick;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Font = new Font("Bahnschrift", 10F, FontStyle.Regular, GraphicsUnit.Point, 204);
            label3.ForeColor = Color.Maroon;
            label3.Location = new Point(621, 660);
            label3.Name = "label3";
            label3.Size = new Size(363, 48);
            label3.TabIndex = 3;
            label3.Text = "*Выберите пользователя в таблице, \r\nнажмите на него 2 раза для удаления";
            // 
            // refreshTable
            // 
            refreshTable.BackColor = Color.PaleVioletRed;
            refreshTable.Font = new Font("Bahnschrift SemiBold Condensed", 12F, FontStyle.Bold, GraphicsUnit.Point, 204);
            refreshTable.Location = new Point(835, 28);
            refreshTable.Name = "refreshTable";
            refreshTable.Size = new Size(149, 41);
            refreshTable.TabIndex = 4;
            refreshTable.Text = "Обновить";
            refreshTable.UseVisualStyleBackColor = false;
            // 
            // aUserForm
            // 
            AutoScaleDimensions = new SizeF(10F, 25F);
            AutoScaleMode = AutoScaleMode.Font;
            AutoSizeMode = AutoSizeMode.GrowAndShrink;
            BackColor = Color.AntiqueWhite;
            ClientSize = new Size(1014, 743);
            Controls.Add(refreshTable);
            Controls.Add(label3);
            Controls.Add(userInfoTable);
            Controls.Add(label2);
            Name = "aUserForm";
            Text = "Пользователи";
            FormClosed += aUserForm_FormClosed;
            ((System.ComponentModel.ISupportInitialize)userInfoTable).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
        private Label label2;
        private DataGridView userInfoTable;
        private Label label3;
        private Button refreshTable;
    }
}