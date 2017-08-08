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
            
            TopMost = true;
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

        private void button3_Click(object sender, EventArgs e) // Логин
        {
            if (checkBox4.CheckState == CheckState.Checked) // Сохраняем настройки
            {
                Properties.Settings.Default.login = (textBox1.Text);
                Properties.Settings.Default.password = (textBox2.Text);
            }
            Login = new Thread(goLogin);
            if (Browser == null)
            {
                if (checkBox3.CheckState == CheckState.Unchecked) // Без консоли запуск
                {
                    var Chromeservice = ChromeDriverService.CreateDefaultService();
                    Chromeservice.HideCommandPromptWindow = true;
                    if (checkBox7.CheckState == CheckState.Unchecked) // без картинок без консоли
                    {
                        var options = new ChromeOptions();
                        options.AddUserProfilePreference("profile.default_content_setting_values.images", 2); // Без картинок загрузка
                        Browser = new ChromeDriver(Chromeservice, options);
                    }
                    else // с картинками, без консоли
                    {
                        Browser = new ChromeDriver(Chromeservice, new ChromeOptions());
                    }
                }
                else
                
                    if (checkBox7.CheckState == CheckState.Unchecked) // без картинок с консолью
                    {
                        var options = new ChromeOptions();
                        options.AddUserProfilePreference("profile.default_content_setting_values.images", 2); // Без картинок загрузка
                        Browser = new ChromeDriver(options);
                    }

                else // с картинками с консолью
                {
                    Browser = new ChromeDriver();
                }
            }                      
            
            Login.Start();                  
        }

        private void goLogin()
        {
            login = textBox1.Text;
            string password = textBox2.Text;
            //Browser = new OpenQA.Selenium.Chrome.ChromeDriver();
            //Browser.Manage().Window.Maximize();
            
            Browser.Navigate().GoToUrl("https://myanimeshelf.com/users/");
                        
            
            try
            {
                IWebElement SearchLoginButton = Browser.FindElement(By.ClassName("logInButt"));
                SearchLoginButton.Click();
            }
            catch
            {
                MessageBox.Show("Already logged in");
                return;
            }

            IWebElement LoginField = Browser.FindElement(By.Name("AUTH[auth_login]"));
            LoginField.SendKeys(login);

            IWebElement PassWordField = Browser.FindElement(By.Name("AUTH[auth_pass]"));
            PassWordField.SendKeys(password + OpenQA.Selenium.Keys.Enter);
            
            try
            {
                Thread.Sleep(2000); // todo Это вынести как отдельную настройку
                IWebElement CheckIfLoggedIn = Browser.FindElement(By.ClassName("blankRel"));
                //PassWordField.SendKeys(password + OpenQA.Selenium.Keys.Enter);
                if (CheckIfLoggedIn.Text.Contains("Welcome"))
                {
                    Invoke(new AddMessage(Say), new object[] { "Logged in as " + login });                    
                }
            }
            catch
            {
                Invoke(new AddMessage(Say), new object[] { "Error" });
            }

        }
               
        private void Onfocus1(object sender, EventArgs e)
        {
            if (textBox1.Text == "Login")
            {
                textBox1.Text = string.Empty;
                textBox1.ForeColor = Color.Black;
            }
        }
        private void Onfocus2(object sender, EventArgs e)
        {
            if (textBox2.Text == "Password")
            {
                textBox2.UseSystemPasswordChar = true;
                textBox2.Text = string.Empty;
                textBox2.ForeColor = Color.Black;
            }
        }
        private void OnDefocus1(object sender, EventArgs e)
        {
            if (textBox1.Text == "")
            {
                textBox1.Text = "Login";
                textBox1.ForeColor = Color.Gray;
            }
        }
        private void OnDefocus2(object sender, EventArgs e)
        {
            if (textBox2.Text == "")
            {
                textBox2.UseSystemPasswordChar = false;
                textBox2.Text = "Password";
                textBox2.ForeColor = Color.Gray;
            }
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

        private void textBox1_KeyDown(object sender, KeyEventArgs e) // Жмем enter в первом textbox'e
        {
            if (e.KeyCode == System.Windows.Forms.Keys.Enter)
            {
                textBox2.Focus();
            }
        }

        private void textBox2_KeyDown(object sender, KeyEventArgs e) // Жмем enter во втором textbox'e
        {
            if (e.KeyCode == System.Windows.Forms.Keys.Enter)
            {
                button3_Click(sender, e); 
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            checkBox4.Checked = Properties.Settings.Default.autologin;
            checkBox7.Checked = Properties.Settings.Default.pictues;
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
            Properties.Settings.Default.Save();
        }

        
        
    }
}
