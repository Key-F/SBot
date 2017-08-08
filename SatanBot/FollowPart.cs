using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SatanBot
{
    public partial class Form1 : Form
    {
        Thread Work;

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
            else MessageBox.Show("Log in first");
        }

        private void button5_Click(object sender, EventArgs e) // Стоп
        {
            if (Work != null)
                Work.Abort();
            if (Login != null)
                Login.Abort();
        }
    }
}
