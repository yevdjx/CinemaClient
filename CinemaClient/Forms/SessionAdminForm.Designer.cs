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
            filmList = new DataGridView();
            groupBox1 = new GroupBox();
            deleteButton = new Button();
            changeButton = new Button();
            saveButton = new Button();
            takeAge = new TextBox();
            takeDir = new TextBox();
            takeProd = new TextBox();
            takeFilmName = new TextBox();
            label6 = new Label();
            label5 = new Label();
            label4 = new Label();
            label3 = new Label();
            label2 = new Label();
            groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)filmList).BeginInit();
            groupBox1.SuspendLayout();
            SuspendLayout();
            // 
            // groupBox2
            // 
            groupBox2.Controls.Add(label1);
            groupBox2.Controls.Add(filmList);
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
            // filmList
            // 
            filmList.BackgroundColor = Color.BlanchedAlmond;
            filmList.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            filmList.ColumnHeadersVisible = false;
            filmList.Location = new Point(26, 77);
            filmList.Name = "filmList";
            filmList.RowHeadersVisible = false;
            filmList.RowHeadersWidth = 62;
            filmList.Size = new Size(954, 202);
            filmList.TabIndex = 0;
            // 
            // groupBox1
            // 
            groupBox1.Controls.Add(deleteButton);
            groupBox1.Controls.Add(changeButton);
            groupBox1.Controls.Add(saveButton);
            groupBox1.Controls.Add(takeAge);
            groupBox1.Controls.Add(takeDir);
            groupBox1.Controls.Add(takeProd);
            groupBox1.Controls.Add(takeFilmName);
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
            // deleteButton
            // 
            deleteButton.BackColor = Color.MistyRose;
            deleteButton.Font = new Font("Bahnschrift SemiBold Condensed", 12F, FontStyle.Bold);
            deleteButton.ForeColor = Color.Maroon;
            deleteButton.Location = new Point(737, 279);
            deleteButton.Name = "deleteButton";
            deleteButton.Size = new Size(209, 67);
            deleteButton.TabIndex = 14;
            deleteButton.Text = "Удалить";
            deleteButton.UseVisualStyleBackColor = false;
            // 
            // changeButton
            // 
            changeButton.BackColor = Color.MistyRose;
            changeButton.Font = new Font("Bahnschrift SemiBold Condensed", 12F, FontStyle.Bold);
            changeButton.ForeColor = Color.Maroon;
            changeButton.Location = new Point(737, 163);
            changeButton.Name = "changeButton";
            changeButton.Size = new Size(209, 67);
            changeButton.TabIndex = 13;
            changeButton.Text = "Отменить изменения";
            changeButton.UseVisualStyleBackColor = false;
            // 
            // saveButton
            // 
            saveButton.BackColor = Color.MistyRose;
            saveButton.Font = new Font("Bahnschrift SemiBold Condensed", 12F, FontStyle.Bold);
            saveButton.ForeColor = Color.Maroon;
            saveButton.Location = new Point(737, 47);
            saveButton.Name = "saveButton";
            saveButton.Size = new Size(209, 67);
            saveButton.TabIndex = 12;
            saveButton.Text = "Сохранить";
            saveButton.UseVisualStyleBackColor = false;
            // 
            // takeAge
            // 
            takeAge.BackColor = Color.Pink;
            takeAge.Font = new Font("Bahnschrift", 12F, FontStyle.Regular, GraphicsUnit.Point, 204);
            takeAge.ForeColor = Color.Maroon;
            takeAge.Location = new Point(293, 290);
            takeAge.Name = "takeAge";
            takeAge.Size = new Size(329, 36);
            takeAge.TabIndex = 11;
            // 
            // takeDir
            // 
            takeDir.BackColor = Color.Pink;
            takeDir.Font = new Font("Bahnschrift", 12F, FontStyle.Regular, GraphicsUnit.Point, 204);
            takeDir.ForeColor = Color.Maroon;
            takeDir.Location = new Point(293, 216);
            takeDir.Name = "takeDir";
            takeDir.Size = new Size(329, 36);
            takeDir.TabIndex = 10;
            // 
            // takeProd
            // 
            takeProd.BackColor = Color.Pink;
            takeProd.Font = new Font("Bahnschrift", 12F, FontStyle.Regular, GraphicsUnit.Point, 204);
            takeProd.ForeColor = Color.Maroon;
            takeProd.Location = new Point(293, 149);
            takeProd.Name = "takeProd";
            takeProd.Size = new Size(329, 36);
            takeProd.TabIndex = 8;
            // 
            // takeFilmName
            // 
            takeFilmName.BackColor = Color.Pink;
            takeFilmName.Font = new Font("Bahnschrift", 12F, FontStyle.Regular, GraphicsUnit.Point, 204);
            takeFilmName.ForeColor = Color.Maroon;
            takeFilmName.Location = new Point(293, 80);
            takeFilmName.Name = "takeFilmName";
            takeFilmName.Size = new Size(329, 36);
            takeFilmName.TabIndex = 7;
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Font = new Font("Bahnschrift Condensed", 14F, FontStyle.Regular, GraphicsUnit.Point, 204);
            label6.ForeColor = Color.DeepPink;
            label6.Location = new Point(34, 278);
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
            ClientSize = new Size(1040, 735);
            Controls.Add(groupBox2);
            Controls.Add(groupBox1);
            Name = "SessionAdminForm";
            Text = "SessionAdminForm";
            groupBox2.ResumeLayout(false);
            groupBox2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)filmList).EndInit();
            groupBox1.ResumeLayout(false);
            groupBox1.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private GroupBox groupBox2;
        private Label label1;
        private DataGridView filmList;
        private GroupBox groupBox1;
        private Button deleteButton;
        private Button changeButton;
        private Button saveButton;
        private TextBox takeAge;
        private TextBox takeDir;
        private TextBox takeProd;
        private TextBox takeFilmName;
        private Label label6;
        private Label label5;
        private Label label4;
        private Label label3;
        private Label label2;
    }
}