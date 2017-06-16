﻿using System;
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
        public delegate void AddMessageDelegate(int good, int bad);

        public Form1()
        {
            TopMost = true;
            InitializeComponent();
            textBox1.LostFocus += OnDefocus1; // Из Form1.Designer студия удаляет их
            textBox2.LostFocus += OnDefocus2;
            textBox2.UseSystemPasswordChar = false;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (Browser != null) Browser.Quit(); 
            Application.Exit();
        }

        private void button3_Click(object sender, EventArgs e) // Логин
        {
            Login = new Thread(goLogin);
            Login.Start();                  
        }

        private void goLogin()
        {
            string login = textBox1.Text;
            string password = textBox2.Text;
            Browser = new OpenQA.Selenium.Chrome.ChromeDriver();
            Browser.Manage().Window.Maximize();
            Browser.Navigate().GoToUrl("https://myanimeshelf.com/users/");
            IWebElement SearchLoginButton = Browser.FindElement(By.ClassName("logInButt"));
            SearchLoginButton.Click();

            IWebElement LoginField = Browser.FindElement(By.Name("AUTH[auth_login]"));
            LoginField.SendKeys(login);

            IWebElement PassWordField = Browser.FindElement(By.Name("AUTH[auth_pass]"));
            PassWordField.SendKeys(password + OpenQA.Selenium.Keys.Enter);

            //IWebElement OkLogin = Browser.FindElement(By.Name("auth_user"));
            // OkLogin.Click();
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
        public void LogAdd(int good, int bad) // Обновление счетчиков
        {
            textBox4.Text = good.ToString();
            textBox5.Text = bad.ToString();
        }

        private void textBox1_MouseClick(object sender, MouseEventArgs e)
        {
            textBox1.Text = string.Empty;
            textBox1.ForeColor = Color.Black;
        }
        private void textBox2_MouseClick(object sender, MouseEventArgs e)
        {
            textBox2.UseSystemPasswordChar = true;
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
                textBox2.UseSystemPasswordChar = false;
                textBox2.Text = "Password";
                textBox2.ForeColor = Color.Gray;
            }
        }

     
    }
}
