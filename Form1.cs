using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.IO.Compression;

namespace SoD2Saver
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            if (!Directory.Exists(ClassData.path + "\\UserSaves")) Directory.CreateDirectory(ClassData.path + "\\UserSaves");
            if (!File.Exists(ClassData.path + "\\UserSaves\\settings.txt"))
            {
                using (StreamWriter sw = File.CreateText(ClassData.path + "\\UserSaves\\settings.txt"))
                {
                    sw.WriteLine("extended_mode=false");
                    sw.WriteLine("user_path=");
                }
            }
            string[] Lines = File.ReadAllLines(ClassData.path + "\\UserSaves\\settings.txt");
            string[] temp = new string[2];
            for (int i = 0; i < Lines.Length; i++)
            {
                temp = Lines[i].Split('=');
                if (temp[0] == "extended_mode") ClassData.extended_mode = temp[1];
                if (temp[0] == "user_path") ClassData.user_path = temp[1];
            }
            FillList();
        }
        public Form Settings = new Form2();

        private void FillList()
        {
            var dir = new DirectoryInfo(ClassData.path + "\\UserSaves");
            foreach (FileInfo file in dir.GetFiles())
            {
                if (Path.GetFileName(file.FullName).Contains("." + ClassData.extended_mode))
                {
                    if (Path.GetFileNameWithoutExtension(file.FullName).ToString() != "")
                    {
                        listBox1.Items.Add(Path.GetFileNameWithoutExtension(file.FullName).ToString());
                    }
                    else
                    {
                        listBox1.Items.Add("{EmptyString}");
                    }
                }
            }
        }

        private void buttonRaname_Click(object sender, EventArgs e)
        {
            int index = Convert.ToInt32(listBox1.SelectedIndex);
            if (index != -1 && File.Exists(ClassData.path + "\\UserSaves\\" + listBox1.Text + "." + ClassData.extended_mode) && !File.Exists(ClassData.path + "\\UserSaves\\" + textBox1.Text + "." + ClassData.extended_mode))
            {
                File.Move(ClassData.path + "\\UserSaves\\" + listBox1.Text + "." + ClassData.extended_mode, ClassData.path + "\\UserSaves\\" + textBox1.Text + "." + ClassData.extended_mode);
                textBox1.Text = string.Empty;
                listBox1.Items.Clear();
                FillList();
            }
        }

        private void buttonSettings_Click(object sender, EventArgs e)
        {
            Settings.Show();
        }

        private void buttonAdd_Click(object sender, EventArgs e)
        {
            if (!File.Exists(ClassData.path + "\\UserSaves\\" + textBox1.Text + "." + ClassData.extended_mode) && textBox1.Text != "")
            {
                string zipPath = ClassData.path + "\\UserSaves\\" + textBox1.Text + "." + ClassData.extended_mode;
                if (ClassData.extended_mode == "false")
                {
                    ZipFile.CreateFromDirectory(ClassData.path + "\\Saved\\Steam", zipPath);
                }
                else
                {
                    string[] allfolders = Directory.GetDirectories(ClassData.path + "\\Saved\\Steam\\" + ClassData.user_path);
                    ZipFile.CreateFromDirectory(allfolders[0] + "\\public\\v2", zipPath);
                }
                textBox1.Text = string.Empty;
                listBox1.Items.Clear();
                FillList();
            }
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            int index = Convert.ToInt32(listBox1.SelectedIndex);
            if (index == -1)
                textBox1.Text = string.Empty;
            else
                textBox1.Text = listBox1.Text;
        }

        private void buttonDelete_Click(object sender, EventArgs e)
        {
            int index = Convert.ToInt32(listBox1.SelectedIndex);
            if (index != -1 && File.Exists(ClassData.path + "\\UserSaves\\" + listBox1.Text + "." + ClassData.extended_mode))
            {
                File.Move(ClassData.path + "\\UserSaves\\" + listBox1.Text + "." + ClassData.extended_mode, ClassData.path + "\\UserSaves\\" + listBox1.Text + ".remove");
                textBox1.Text = string.Empty;
                listBox1.Items.Clear();
                FillList();
            }
        }

        private void buttonLoad_Click(object sender, EventArgs e)
        {
            int index = Convert.ToInt32(listBox1.SelectedIndex);
            if (index != -1 && File.Exists(ClassData.path + "\\UserSaves\\" + listBox1.Text + "." + ClassData.extended_mode))
            {
                Form dialog = new Form();
                dialog.Text = "Внимание";
                dialog.Size = new Size(237, 139);
                Label l0 = new Label();
                Label l1 = new Label();
                l0.Text = "Текущее сохранение будет заменено.";
                l0.Size = new Size(201, 13);
                l0.Location = new Point(12, 9);
                l1.Text = "Вы уверены?";
                l1.Size = new Size(74, 13);
                l1.Location = new Point(12, 32);
                Button buttonAccept = new Button();
                Button buttonCancel = new Button();
                buttonCancel.Location = new Point(11, 64);
                buttonCancel.Text = "Отмена";
                buttonCancel.DialogResult = DialogResult.Cancel;
                buttonAccept.Location = new Point(134, 64);
                buttonAccept.Text = "Да";
                buttonAccept.DialogResult = DialogResult.OK;
                dialog.FormBorderStyle = FormBorderStyle.FixedDialog;
                dialog.AcceptButton = buttonAccept;
                dialog.CancelButton = buttonCancel;
                dialog.StartPosition = FormStartPosition.CenterScreen;
                dialog.Controls.Add(buttonAccept);
                dialog.Controls.Add(buttonCancel);
                dialog.Controls.Add(l0);
                dialog.Controls.Add(l1);
                dialog.ShowDialog();
                if (dialog.DialogResult == DialogResult.OK)
                {
                    string[] allfolders = Directory.GetDirectories(ClassData.path + "\\Saved\\Steam\\" + ClassData.user_path);
                    var dir = new DirectoryInfo(allfolders[0] + "\\public\\v2");
                    foreach (FileInfo file in dir.GetFiles())
                    {
                        file.Delete();
                    }
                    ZipFile.ExtractToDirectory(ClassData.path + "\\UserSaves\\" + listBox1.Text + "." + ClassData.extended_mode, allfolders[0] + "\\public\\v2");
                    dialog.Dispose();
                }
                else
                {
                    dialog.Dispose();
                }
            }
        }
    }
}
