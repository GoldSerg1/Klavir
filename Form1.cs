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

        // �������� ������ ���� � ����������� ����� ���������
        public void GetProgramPath()
        {
            // Environment.CurrentDirectory
            var path = AppDomain.CurrentDomain.BaseDirectory;
            configPath = Path.Combine(path, MyTrayApp.configName);
            exePath = Path.Combine(path, MyTrayApp.programName);
        }

        // ���������� checkBoxAutoStart � ����������� �� ������������� ����� ������� regAutostartPath
        private void SetCheckBoxAutoStart()
        {
            RegistryKey? key = Registry.CurrentUser.OpenSubKey(regAutostartPath, true);
            if (key != null)
            {
                if (key.GetValue("Klavir") == null) checkBoxAutoStart.Checked = false;
                else checkBoxAutoStart.Checked = true;
            }
        }


        // ��������� �������� ������������
        private void checkBoxAutoStart_CheckedChanged(object sender, EventArgs e)
        {

            if (checkBoxAutoStart.Checked)
            {
                // �������� ������������ � �������
                RegistryKey? key = Registry.CurrentUser.OpenSubKey(regAutostartPath, true);
                if (key != null)
                {
                    if (key.GetValue("Klavir") == null) key.SetValue("Klavir", exePath);
                }
            }
            else
            {
                // ��������� ������������ � �������
                RegistryKey? key = Registry.CurrentUser.OpenSubKey(regAutostartPath, true);
                if (key != null)
                {
                    if (key.GetValue("Klavir") != null) key.DeleteValue("Klavir", false);
                }
            }
        }

        // ������� ������� ������� � ���� textBoxKeys
        private void textBoxKeys_KeyDown(object sender, KeyEventArgs e)
        {
            textBoxKeys.Text = Program.Key2Str(e);
        }

        // ������� ������ ����� � ���� textBoxPath
        private void textBoxPath_MouseClick(object sender, MouseEventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                textBoxPath.Clear();
                textBoxPath.AppendText(openFileDialog1.FileName);
                textBoxKeys.Select();
            }
        }

        // �������� � ������� ����� ���������� � ����
        private void buttonAdd_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(textBoxKeys.Text) || string.IsNullOrEmpty(textBoxPath.Text))
            {
                MessageBox.Show("�� ������� ��� ���������", "������", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            string s = textBoxKeys.Text;

            foreach (ListViewItem itm in listView1.Items)
            {
                if (itm.Text.Equals(s))
                {
                    MessageBox.Show("����� ���������� ��� ����������", "������", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }

            // �������� � �������
            AddKeyPath2ListView(textBoxKeys.Text, textBoxPath.Text);

            // �������� ���� �� �����
            textBoxKeys.Clear();
            textBoxPath.Clear();
        }

        // �������� ��������� ������� + ������ � �������
        private void AddKeyPath2ListView(string keys, string path)
        {
            ListViewItem item = new ListViewItem(keys);
            item.SubItems.Add(path);
            listView1.Items.Add(item);
        }

        // ������� �� ������ ���������� ������
        private void buttonDel_Click(object sender, EventArgs e)
        {
            listView1.SelectedItems[0].Remove();
        }


        // �������� ������
        private void buttonClear_Click(object sender, EventArgs e)
        {
            listView1.Items.Clear();
        }


        // ������� ����� ��������� ����� - �������� ������ � ���� � �������� ������ � ���������
        private async void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            string save2file = "";

            // ����������� ������ ��� ������
            foreach (ListViewItem itm in listView1.Items)
            {
                //itm.SubItems[0].Text  key
                //itm.SubItems[1].Text  path
                save2file += itm.SubItems[0].Text + "," + itm.SubItems[1].Text + "\n";
            }

            // �������� ������ � ����
            using (StreamWriter writer = new StreamWriter(configPath, false))
            {
                await writer.WriteLineAsync(save2file);
            }

            // �������� ������� � Program
            MyTrayApp.Str2keylist(save2file);
        }


        // ������� ����� ������������ �����
        private void Form1_Shown(object sender, EventArgs e)
        {
            foreach (KeyValuePair<string, string> entry in MyTrayApp.keylist) 
            {
                AddKeyPath2ListView(entry.Key, entry.Value);
            }
        }
    }
}