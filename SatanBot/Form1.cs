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

namespace SatanBot
{
    public partial class Form1 : Form
    {
        IWebDriver Browser;
        Thread thread;
        public delegate void AddMessageDelegate(int good, int bad);
        public Form1()
        {
            InitializeComponent();

        }

        private void button1_Click(object sender, EventArgs e)
        {
            Browser = new OpenQA.Selenium.Chrome.ChromeDriver();
            Browser.Manage().Window.Maximize();
           // OpenQA.Selenium.Chrome.ChromeOptions options = new OpenQA.Selenium.Chrome.ChromeOptions();
           // options.AddArguments("--no-startup-window");
           // OpenQA.Selenium.Chrome.ChromeDriver Browser = new OpenQA.Selenium.Chrome.ChromeDriver(options);
            Browser.Navigate().GoToUrl("https://yandex.ru/");

            IWebElement SearchInput = Browser.FindElement(By.Id("text"));
            SearchInput.SendKeys("аниме" + OpenQA.Selenium.Keys.Enter);

        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (Browser != null) Browser.Quit(); 
            Application.Exit();
        }

        private void button3_Click(object sender, EventArgs e) // Логин
        {
            string login = textBox1.Text;
            string password = textBox2.Text;
            Browser = new OpenQA.Selenium.Chrome.ChromeDriver();
            Browser.Manage().Window.Maximize();
            Browser.Navigate().GoToUrl("https://myanimeshelf.com/users/");
            IWebElement SearchLoginButton = Browser.FindElement(By.ClassName("logInButt"));
            SearchLoginButton.Click();

            IWebElement LoginField = Browser.FindElement(By.Name("AUTH[auth_login]"));
            LoginField.SendKeys(login + OpenQA.Selenium.Keys.Enter);

            IWebElement PassWordField = Browser.FindElement(By.Name("AUTH[auth_pass]"));
            PassWordField.SendKeys(password + OpenQA.Selenium.Keys.Enter);

            IWebElement OkLogin = Browser.FindElement(By.ClassName("submitButton"));
            OkLogin.Click();

            
        }

        private void button4_Click(object sender, EventArgs e)
        {
            thread = new Thread(FollowStart);
            thread.Start();           
        }
        private void FollowStart()
        {
            int goodtry = 0;
            int badtry = 0;

            //IWebElement Next;
            for (int i = Convert.ToInt32(textBox6.Text) - 1; i < Convert.ToInt32(textBox3.Text); i++)
            {
                //int counter = 0;
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
                    //if (user.Text == "Following") counter++;
                    
                }

                if (FollowUsers == null) MessageBox.Show("Пользователи закончились");
                System.Threading.Thread.Sleep(Convert.ToInt32(textBox7.Text));
            }       
        }
                //Next = Browser.FindElement(By.PartialLinkText("/users/?field=rdate&how=asc&p="));
               // Next = Browser.FindElement(By.Name("Next"));
               // Next.Click();
                                               
               private void button5_Click(object sender, EventArgs e) // Стоп
               {
                   thread.Abort();
               }
               public void LogAdd(int good, int bad) // Обновление счетчиков
               {
                   textBox4.Text = good.ToString();
                   textBox5.Text = bad.ToString();
               }

               private void button6_Click(object sender, EventArgs e) 
               {
                   //thread.Resume(); // разобраться в этом говне
               }
               private void textBox1_MouseClick(object sender, MouseEventArgs e)
               {
                   textBox1.Text = string.Empty;
                   textBox1.ForeColor = Color.Black;       
               }
               private void textBox2_MouseClick(object sender, MouseEventArgs e)
               {
                   textBox2.Text = string.Empty;
                   textBox2.ForeColor = Color.Black;
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
                       textBox2.Text = "Password";
                       textBox2.ForeColor = Color.Gray;
                   }
               }

           

  

               
    }
}
