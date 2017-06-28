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
        Thread Welcome;

        private void button1_Click(object sender, EventArgs e) // eto test
        {
            if ((Browser != null) && (toolStripStatusLabel1.Text.Contains("Logged") == true))
            {
                Browser.Navigate().GoToUrl("http://myanimeshelf.com/shelf/Key_F");
                //IWebElement SendMessage = Browser.FindElement(By.Name("post"));
                //SendMessage.SendKeys("bot test");
                //List<IWebElement> Comments = Browser.FindElements(By.CssSelector("div[id ^= 'comments']  b")).ToList();
                //List<IWebElement> Comments = Browser.FindElements(By.CssSelector("b[class = 'user_name']")).ToList(); // vrode rabotaet
                IWebElement SendMessage = Browser.FindElement(By.Name("post"));
                SendMessage.SendKeys("[CENTER] [SIZE=5] Hi " + "SS" + "![/SIZE] [/CENTER]");
                SendMessage.SendKeys(OpenQA.Selenium.Keys.Enter);
                SendMessage.SendKeys(OpenQA.Selenium.Keys.Enter);
                SendMessage.SendKeys("[MYPHOTO=1104636]");
                MessageBox.Show("");
                IWebElement SearchPostButton = Browser.FindElement(By.ClassName("commentSubmit"));
                SearchPostButton.Click();
                System.Threading.Thread.Sleep(500); // Немножк
                try
                {
                    IWebElement DaButton = Browser.FindElement(By.ClassName("approve"));
                    DaButton.Click();
                }
                catch
                {
                    MessageBox.Show("Хуй с ним");
                }

            }
            else
                MessageBox.Show("Нужно выполнить вход и не закрывать браузер");


        }

        private void button8_Click(object sender, EventArgs e) // Stop 
        {
            if (Welcome != null)
                Welcome.Abort();
        }

        private void FindGuys()
        {
            if (Browser == null)
            {
                MessageBox.Show("Необходимо залогиниться и не закрывать браузер");
                return;
            }
            Browser.Navigate().GoToUrl("http://myanimeshelf.com/");
            List<IWebElement> NewGuys = Browser.FindElements(By.CssSelector(".content td:nth-last-child(3) a")).ToList(); // Новые
            List<string> NewGuyNames = new List<string>();
            for (int i = 0; i < NewGuys.Count; i++)
            {
                NewGuyNames.Add(NewGuys[i].Text);
            }

            for (int i = 0; i < NewGuyNames.Count; i++)
            {
                bool yatut = true;
                bool fsb = false; // fsb, vk, tw
                Browser.Navigate().GoToUrl("http://myanimeshelf.com/shelf/" + NewGuyNames[i]);
                List<IWebElement> Comments = Browser.FindElements(By.CssSelector("b[class = 'user_name']")).ToList(); // vrode rabotaet
                if ((NewGuyNames[i].StartsWith("id") || (NewGuyNames[i].StartsWith("tw_")) || (NewGuyNames[i].StartsWith("fb_")) || (NewGuyNames[i].StartsWith("vk_"))))
                {
                    MessageBox.Show("eto fsb");
                    fsb = true;
                }

                // это в отдедьную функцию
                int j = 0;
                do
                {
                    if (Comments.Count != 0)
                    {
                        if (Comments[j].Text.ToLower() == login.ToLower()) // ToLower чтобы не учитывать регистр, нужно проверить работает ли без него
                        {
                            MessageBox.Show("Ya tut");
                            yatut = true;
                            // return;
                            break;
                        }
                        else yatut = false;
                    }
                    else
                    {
                        yatut = false;
                    }
                    j++;
                }
                while (j < Comments.Count);
                if (yatut == false)
                {

                    MessageBox.Show("Menya net");
                    
                    if (!fsb)
                    {
                        postCreate(NewGuyNames[i]);
                    }
                    if (fsb)
                    {
                        postCreate("");
                    }
                }

            }

        }
        private void button7_Click(object sender, EventArgs e) // Finder 
        {
            Welcome = new Thread(FindGuys);
            Welcome.Start();            
        }

        private void postCreate(string name)
        {
            IWebElement SendMessage = Browser.FindElement(By.Name("post"));
            if (name != "")
            SendMessage.SendKeys("[CENTER] [SIZE=5] Hi " + name + "![/SIZE] [/CENTER]");
            else
                SendMessage.SendKeys("[CENTER] [SIZE=5] Hi![/SIZE] [/CENTER]");
            SendMessage.SendKeys(OpenQA.Selenium.Keys.Enter);
            SendMessage.SendKeys(OpenQA.Selenium.Keys.Enter);
            SendMessage.SendKeys("[MYPHOTO=1104636]");
            MessageBox.Show("");
            IWebElement SearchPostButton = Browser.FindElement(By.ClassName("commentSubmit"));
            SearchPostButton.Click();
            Thread.Sleep(5000); // Немножк
            try
            {
                IWebElement DaButton = Browser.FindElement(By.ClassName("approve"));
                DaButton.Click();
            }
            catch
            {
                MessageBox.Show("Хуй с ним");
            }
        }
    }
}