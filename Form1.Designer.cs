namespace Klavir
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            checkBoxAutoStart = new CheckBox();
            textBoxKeys = new TextBox();
            label1 = new Label();
            textBoxPath = new TextBox();
            label2 = new Label();
            buttonAdd = new Button();
            buttonDel = new Button();
            buttonClear = new Button();
            listView1 = new ListView();
            columnHeader1 = new ColumnHeader();
            columnHeader2 = new ColumnHeader();
            openFileDialog1 = new OpenFileDialog();
            SuspendLayout();
            // 
            // checkBoxAutoStart
            // 
            checkBoxAutoStart.AutoSize = true;
            checkBoxAutoStart.Location = new Point(12, 12);
            checkBoxAutoStart.Name = "checkBoxAutoStart";
            checkBoxAutoStart.Size = new Size(195, 19);
            checkBoxAutoStart.TabIndex = 0;
            checkBoxAutoStart.Text = "Запускать при старте Windows";
            checkBoxAutoStart.UseVisualStyleBackColor = true;
            checkBoxAutoStart.CheckedChanged += checkBoxAutoStart_CheckedChanged;
            // 
            // textBoxKeys
            // 
            textBoxKeys.Location = new Point(12, 224);
            textBoxKeys.Name = "textBoxKeys";
            textBoxKeys.ReadOnly = true;
            textBoxKeys.Size = new Size(123, 23);
            textBoxKeys.TabIndex = 1;
            textBoxKeys.KeyDown += textBoxKeys_KeyDown;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(12, 206);
            label1.Name = "label1";
            label1.Size = new Size(123, 15);
            label1.TabIndex = 2;
            label1.Text = "Комбинация клавиш";
            // 
            // textBoxPath
            // 
            textBoxPath.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            textBoxPath.Location = new Point(151, 224);
            textBoxPath.Name = "textBoxPath";
            textBoxPath.ReadOnly = true;
            textBoxPath.Size = new Size(362, 23);
            textBoxPath.TabIndex = 3;
            textBoxPath.MouseClick += textBoxPath_MouseClick;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(151, 206);
            label2.Name = "label2";
            label2.Size = new Size(175, 15);
            label2.TabIndex = 4;
            label2.Text = "Путь к программе для запуска";
            // 
            // buttonAdd
            // 
            buttonAdd.Location = new Point(12, 171);
            buttonAdd.Name = "buttonAdd";
            buttonAdd.Size = new Size(90, 23);
            buttonAdd.TabIndex = 5;
            buttonAdd.Text = "Добавить";
            buttonAdd.UseVisualStyleBackColor = true;
            buttonAdd.Click += buttonAdd_Click;
            // 
            // buttonDel
            // 
            buttonDel.Location = new Point(124, 171);
            buttonDel.Name = "buttonDel";
            buttonDel.Size = new Size(90, 23);
            buttonDel.TabIndex = 6;
            buttonDel.Text = "Удалить";
            buttonDel.UseVisualStyleBackColor = true;
            buttonDel.Click += buttonDel_Click;
            // 
            // buttonClear
            // 
            buttonClear.Location = new Point(236, 171);
            buttonClear.Name = "buttonClear";
            buttonClear.Size = new Size(90, 23);
            buttonClear.TabIndex = 7;
            buttonClear.Text = "Очистить";
            buttonClear.UseVisualStyleBackColor = true;
            buttonClear.Click += buttonClear_Click;
            // 
            // listView1
            // 
            listView1.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            listView1.Columns.AddRange(new ColumnHeader[] { columnHeader1, columnHeader2 });
            listView1.Location = new Point(12, 37);
            listView1.Name = "listView1";
            listView1.Size = new Size(501, 128);
            listView1.TabIndex = 8;
            listView1.UseCompatibleStateImageBehavior = false;
            listView1.View = View.Details;
            // 
            // columnHeader1
            // 
            columnHeader1.Text = "Комбинация";
            columnHeader1.Width = 105;
            // 
            // columnHeader2
            // 
            columnHeader2.Text = "Путь";
            columnHeader2.Width = 390;
            // 
            // openFileDialog1
            // 
            openFileDialog1.FileName = "openFileDialog1";
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(524, 259);
            Controls.Add(listView1);
            Controls.Add(buttonClear);
            Controls.Add(buttonDel);
            Controls.Add(buttonAdd);
            Controls.Add(label2);
            Controls.Add(textBoxPath);
            Controls.Add(label1);
            Controls.Add(textBoxKeys);
            Controls.Add(checkBoxAutoStart);
            Icon = (Icon)resources.GetObject("$this.Icon");
            Name = "Form1";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Настройка";
            FormClosing += Form1_FormClosing;
            Shown += Form1_Shown;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private CheckBox checkBoxAutoStart;
        private TextBox textBoxKeys;
        private Label label1;
        private TextBox textBoxPath;
        private Label label2;
        private Button buttonAdd;
        private Button buttonDel;
        private Button buttonClear;
        private ListView listView1;
        private ColumnHeader columnHeader1;
        private ColumnHeader columnHeader2;
        private OpenFileDialog openFileDialog1;
    }
}