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

namespace SatanBot
{

    public partial class Form1 : Form
    {

        IWebDriver Browser;
        Thread Work;
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
            Login = new Thread(goLogin);
            Browser = new OpenQA.Selenium.Chrome.ChromeDriver();
            Login.Start();                  
        }

        private void goLogin()
        {
            login = textBox1.Text;
            string password = textBox2.Text;
            //Browser = new OpenQA.Selenium.Chrome.ChromeDriver();
            //Browser.Manage().Window.Maximize();
            Browser.Navigate().GoToUrl("https://myanimeshelf.com/users/");
            IWebElement SearchLoginButton = Browser.FindElement(By.ClassName("logInButt"));
            SearchLoginButton.Click();

            IWebElement LoginField = Browser.FindElement(By.Name("AUTH[auth_login]"));
            LoginField.SendKeys(login);

            IWebElement PassWordField = Browser.FindElement(By.Name("AUTH[auth_pass]"));
            PassWordField.SendKeys(password + OpenQA.Selenium.Keys.Enter);
            
            try
            {
                System.Threading.Thread.Sleep(2000); // todo Это вынести как отдельную настройку
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
        private void button4_Click(object sender, EventArgs e) // Follow
        {
            Work = new Thread(FollowStart);
            Work.Start();           
        }
        private void FollowStart()
        {
            int goodtry = 0;
            int badtry = 0;
            if (Browser != null)
            {
                for (int i = Convert.ToInt32(textBox6.Text) - 1; i < Convert.ToInt32(textBox3.Text); i++)
                {
                    Browser.Navigate().GoToUrl("https://myanimeshelf.com/users/?p=" + (i + 1).ToString());
                    List<IWebElement> FollowUsers = Browser.FindElements(By.ClassName("followMeButton")).ToList();
                    foreach (IWebElement user in FollowUsers)
                    {
                        if (user.Text == "Follow")
                        {
                            try
                            {
                                user.Click();
                                goodtry++;
                                System.Threading.Thread.Sleep(Convert.ToInt32(textBox8.Text)); // 500 - стабильно, 100 - вообще нет, 250 - иногда проходы в конце
                            }
                            catch
                            {
                                badtry++;
                            }
                        }
                        Invoke(new AddMessageDelegate(LogAdd), new object[] { goodtry, badtry });
                    }

                    if (FollowUsers == null) MessageBox.Show("Пользователи закончились");
                    System.Threading.Thread.Sleep(Convert.ToInt32(textBox7.Text));
                }
            }
        }
        private void button5_Click(object sender, EventArgs e) // Стоп
        {
            if (Work != null) 
            Work.Abort();
            if (Login != null)
            Login.Abort();
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


        private void button6_Click(object sender, EventArgs e)
        {
            if (Browser == null)
            {
                Browser = new OpenQA.Selenium.Chrome.ChromeDriver();
                Browser.Navigate().GoToUrl("http://myanimeshelf.com/");
                List<IWebElement> BDay = Browser.FindElements(By.CssSelector(".content td:nth-last-child(1) a")).ToList(); // B-days
                //List<IWebElement> NewGuys = Browser.FindElements(By.CssSelector(".content td:nth-last-child(3) a")).ToList(); // Новые

           }

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
    }
}
