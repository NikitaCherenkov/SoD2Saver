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

namespace SoD2Saver
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
            labelVersion.Text = "Version: " + ClassData.version;
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
                if (temp[0].Contains("extended_mode"))
                {
                    if (temp[1] == "true") checkBox1.Checked = true;
                }
                if (temp[0].Contains("user_path")) textBox1.Text = temp[1];
            }
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            e.Cancel = true;
            this.Hide();
        }

        private void buttonAccept_Click(object sender, EventArgs e)
        {
            EditFile();
        }

        private void EditFile()
        {
            File.Delete(ClassData.path + "\\UserSaves\\settings.txt");
            string extended_mode = "false";
            string user_path = textBox1.Text;
            if (checkBox1.Checked) extended_mode = "true";
            using (StreamWriter sw = File.CreateText(ClassData.path + "\\UserSaves\\settings.txt"))
            {
                sw.WriteLine("extended_mode=" + extended_mode);
                sw.WriteLine("user_path=" + user_path);
            }
            ClassData.extended_mode = extended_mode;
            ClassData.user_path = user_path;
            this.Close();
        }
    }
}
