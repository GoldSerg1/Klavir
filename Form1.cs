using Microsoft.Win32;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using static System.Net.Mime.MediaTypeNames;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace Klavir
{
    public partial class Form1 : Form
    {
        public static string configPath = "";
        public static string exePath = "";
        public static string regAutostartPath = "SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run";


        public Form1()
        {
            InitializeComponent();
            openFileDialog1.Filter = "Exe files(*.exe)|*.exe|All files(*.*)|*.*";
            SetCheckBoxAutoStart();
            GetProgramPath();
        }

        // Получить полный путь к запущенному файлу программы
        public void GetProgramPath()
        {
            // Environment.CurrentDirectory
            var path = AppDomain.CurrentDomain.BaseDirectory;
            configPath = Path.Combine(path, MyTrayApp.configName);
            exePath = Path.Combine(path, MyTrayApp.programName);
        }

        // Установить checkBoxAutoStart в зависимости от существования ключа реестра regAutostartPath
        private void SetCheckBoxAutoStart()
        {
            RegistryKey? key = Registry.CurrentUser.OpenSubKey(regAutostartPath, true);
            if (key != null)
            {
                if (key.GetValue("Klavir") == null) checkBoxAutoStart.Checked = false;
                else checkBoxAutoStart.Checked = true;
            }
        }


        // Изменение свойства автозагрузки
        private void checkBoxAutoStart_CheckedChanged(object sender, EventArgs e)
        {

            if (checkBoxAutoStart.Checked)
            {
                // Включить автозагрузку в реестре
                RegistryKey? key = Registry.CurrentUser.OpenSubKey(regAutostartPath, true);
                if (key != null)
                {
                    if (key.GetValue("Klavir") == null) key.SetValue("Klavir", exePath);
                }
            }
            else
            {
                // Выключить автозагрузку в реестре
                RegistryKey? key = Registry.CurrentUser.OpenSubKey(regAutostartPath, true);
                if (key != null)
                {
                    if (key.GetValue("Klavir") != null) key.DeleteValue("Klavir", false);
                }
            }
        }

        // Событие нажатия клавиши в поле textBoxKeys
        private void textBoxKeys_KeyDown(object sender, KeyEventArgs e)
        {
            textBoxKeys.Text = Program.Key2Str(e);
        }

        // Событие щелчок мышью в поле textBoxPath
        private void textBoxPath_MouseClick(object sender, MouseEventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                textBoxPath.Clear();
                textBoxPath.AppendText(openFileDialog1.FileName);
                textBoxKeys.Select();
            }
        }

        // Добавить в таблицу новую комбинацию и путь
        private void buttonAdd_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(textBoxKeys.Text) || string.IsNullOrEmpty(textBoxPath.Text))
            {
                MessageBox.Show("Не введены все параметры", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            string s = textBoxKeys.Text;

            foreach (ListViewItem itm in listView1.Items)
            {
                if (itm.Text.Equals(s))
                {
                    MessageBox.Show("Такая комбинация уже существует", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }

            // Добавить в таблицу
            AddKeyPath2ListView(textBoxKeys.Text, textBoxPath.Text);

            // Очистить поля на форме
            textBoxKeys.Clear();
            textBoxPath.Clear();
        }

        // Добавить параметры клавиша + строка в таблицу
        private void AddKeyPath2ListView(string keys, string path)
        {
            ListViewItem item = new ListViewItem(keys);
            item.SubItems.Add(path);
            listView1.Items.Add(item);
        }

        // Удалить из списка выделенную строку
        private void buttonDel_Click(object sender, EventArgs e)
        {
            listView1.SelectedItems[0].Remove();
        }


        // Очистить список
        private void buttonClear_Click(object sender, EventArgs e)
        {
            listView1.Items.Clear();
        }


        // Событие перед закрытием формы - записать данные в файл и обновить список в программе
        private async void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            string save2file = "";

            // Подготовить строку для записи
            foreach (ListViewItem itm in listView1.Items)
            {
                //itm.SubItems[0].Text  key
                //itm.SubItems[1].Text  path
                save2file += itm.SubItems[0].Text + "," + itm.SubItems[1].Text + "\n";
            }

            // Записать строку в файл
            using (StreamWriter writer = new StreamWriter(configPath, false))
            {
                await writer.WriteLineAsync(save2file);
            }

            // Обновить словарь в Program
            MyTrayApp.Str2keylist(save2file);
        }


        // Событие перед отображением формы
        private void Form1_Shown(object sender, EventArgs e)
        {
            foreach (KeyValuePair<string, string> entry in MyTrayApp.keylist) 
            {
                AddKeyPath2ListView(entry.Key, entry.Value);
            }
        }
    }
}