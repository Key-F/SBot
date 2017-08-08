using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using OpenQA.Selenium;
using System.Threading;
using System.Runtime.InteropServices;
using System.Xml;
using System.IO;
using OpenQA.Selenium.Chrome;

namespace SatanBot
{

    public partial class Form1 : Form
    {

        IWebDriver Browser;        
        Thread Login;
        string login;
        public delegate void AddMessageDelegate(int good, int bad);
        public delegate void AddMessage(string hey);

        public Form1()
        {
            if (Properties.Settings.Default.ontop == true)
                TopMost = true;
            else
                TopMost = false;

            InitializeComponent();
            textBox1.LostFocus += OnDefocus1; // Из Form1.Designer студия удаляет их
            textBox2.LostFocus += OnDefocus2;
            textBox1.GotFocus += Onfocus1; // Из Form1.Designer студия удаляет их
            textBox2.GotFocus += Onfocus2;
            textBox2.UseSystemPasswordChar = false;
            tabControl1.Select();
        }

        private void button2_Click(object sender, EventArgs e) // Выход
        {
            if (Browser != null) Browser.Quit(); 
            Application.Exit();
        }
              
        //for Delegates
        public void LogAdd(int good, int bad) // Обновление счетчиков
        {
            textBox4.Text = good.ToString();
            textBox5.Text = bad.ToString();
        }

        public void Say(string hey) // Сказать в строке состояний
        {
            toolStripStatusLabel1.Text = hey;
        }


       
        private void checkBox1_MouseClick_1(object sender, MouseEventArgs e) // Запуск с Windows
        {
            if (checkBox1.CheckState == CheckState.Checked) // Запускаем
            {
                try
                {
                    Microsoft.Win32.RegistryKey Key = Microsoft.Win32.Registry.LocalMachine.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run\\", true);
                    string ourpath = Application.ExecutablePath;
                    Key.SetValue("SBot", ourpath);
                    Key.Close();
                }
                catch
                {
                    MessageBox.Show("Запустите с правами администратора");
                    checkBox1.CheckState = CheckState.Unchecked;
                    return;

                }
            }

            if (checkBox1.CheckState == CheckState.Unchecked) // Не запускаем
            {
                try
                {
                    Microsoft.Win32.RegistryKey key =
                 Microsoft.Win32.Registry.LocalMachine.OpenSubKey(
                "SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true);
                    key.DeleteValue("SBot", false);
                    key.Close();
                }
                catch
                {
                    MessageBox.Show("Запустите с правами администратора");
                    checkBox1.CheckState = CheckState.Checked;
                    return;
                }
            }
        }

       

        private void Form1_Load(object sender, EventArgs e)
        {
            checkBox4.Checked = Properties.Settings.Default.autologin;
            checkBox7.Checked = Properties.Settings.Default.pictues;
            checkBox6.Checked = Properties.Settings.Default.ontop;
            if (checkBox4.CheckState == CheckState.Checked)
            {
                Onfocus1(sender, e);
                Onfocus2(sender, e);                
                textBox1.Text = (Properties.Settings.Default.login);
                textBox2.Text = (Properties.Settings.Default.password);
            }
            //string path = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData); // 		path	"C:\\Users\\keyf\\AppData\\Roaming"	string

            // if (!File.Exists(path + "\\settings.xml"))
            // File.Create(path + "\\settings.xml");
            //XmlTextWriter xW = new XmlTextWriter(path + "\\settings.xml");

        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            Properties.Settings.Default.autologin = checkBox4.Checked;
            Properties.Settings.Default.pictues = checkBox7.Checked;
            Properties.Settings.Default.ontop = checkBox6.Checked;
            Properties.Settings.Default.Save();
        }

        private void checkBox6_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox6.CheckState == CheckState.Checked)
                TopMost = true;
            else
                TopMost = false;
        }
    }
}
