using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SatanBot
{
    public partial class Form1 : Form
    {

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
                //MessageBox.Show("Already logged in");
                MessageBox.Show("Что-то не так");
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
