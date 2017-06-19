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

        private void button1_Click(object sender, EventArgs e)
        {
            if ((Browser != null) && (toolStripStatusLabel1.Text.Contains("Logged") == true))
            {
                Browser.Navigate().GoToUrl("http://myanimeshelf.com/shelf/USERNAME");
                //IWebElement SendMessage = Browser.FindElement(By.Name("post"));
                //SendMessage.SendKeys("bot test");
                //List<IWebElement> Comments = Browser.FindElements(By.CssSelector("div[id ^= 'comments']  b")).ToList();
                List<IWebElement> Comments = Browser.FindElements(By.CssSelector("b[class = 'user_name']")).ToList(); // vrode rabotaet

            }
            else
                MessageBox.Show("Нужно выполнить вход и не закрывать браузер");


        }
        private void button7_Click(object sender, EventArgs e)
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
                if ((NewGuyNames[i].StartsWith("id") || (NewGuyNames[i].StartsWith("tw_")) || (NewGuyNames[i].StartsWith("fb_"))))
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
                        if (Comments[j].Text == "Key_F") // Имя из логина брать
                        {
                            MessageBox.Show("Ya tut");
                            yatut = true;
                            break;
                        }
                        else yatut = false;
                    }
                    else
                    {
                        yatut = false;

                        //

                        //

                    }
                    j++;
                }
                while (j < Comments.Count);
                if (yatut == false)
                {

                    MessageBox.Show("Menya net");
                    IWebElement SendMessage = Browser.FindElement(By.Name("post"));
                    if (!fsb)
                    {
                        SendMessage.SendKeys("[CENTER] [SIZE=5] Hi " + NewGuyNames[i] + "![/SIZE] [/CENTER]");
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
                    if (fsb)
                    {
                        SendMessage.SendKeys("[CENTER] [SIZE=5] Hi![/SIZE] [/CENTER]" + OpenQA.Selenium.Keys.Enter + "[MYPHOTO=1104636]");
                        MessageBox.Show("");
                        IWebElement SearchPostButton = Browser.FindElement(By.ClassName("commentSubmit"));
                        SearchPostButton.Click();
                        System.Threading.Thread.Sleep(2000);
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
        }
    }
}