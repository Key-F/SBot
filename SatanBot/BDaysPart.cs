using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using OpenQA.Selenium;
using System.Threading;
using System.Globalization;

namespace SatanBot
{
    public partial class Form1 : Form
    {
        Thread BDay;

        private void button6_Click(object sender, EventArgs e) // Finder
        {
            BDay = new Thread(FindBD);
            BDay.Start();
        }

        private void button9_Click(object sender, EventArgs e) // Stop
        {
            if (BDay != null)
            BDay.Abort();
        }

        private void FindBD()
        {
            if (Browser == null)
            {
                Browser = new OpenQA.Selenium.Chrome.ChromeDriver();
            }
            else
            {
                Browser.Navigate().GoToUrl("http://myanimeshelf.com/");
                List<IWebElement> BDayGuy = Browser.FindElements(By.CssSelector(".content td:nth-last-child(1) a")).ToList(); // B-days
                List<string> BDay = Tools4help.GetString(BDayGuy);
                DateTime thisDay = DateTime.Today;
                for (int i = 0; i < BDay.Count; i++)
                {
                    Browser.Navigate().GoToUrl("https://myanimeshelf.com/shelf/" + BDay[i]);
                    IWebElement LastOnline = Browser.FindElement(By.CssSelector(".mc .broken tr:nth-last-child(6) td:nth-last-child(1) "));
                    string LastOnlineNoTime = LastOnline.Text.Substring(0, LastOnline.Text.LastIndexOf(','));
                    DateTime regDate = DateTime.ParseExact(LastOnlineNoTime, "dd MMM yy", CultureInfo.InvariantCulture);
                    string age;
                    try
                    {
                        IWebElement Age = Browser.FindElement(By.CssSelector(".mb .broken tr:nth-last-child(4) td:nth-last-child(1)"));
                        int start = (Age.Text.IndexOf("(") + 1);
                        int end = Age.Text.LastIndexOf("years)");
                        age = Age.Text.Substring(start, end - start - 1);
                    }
                    catch
                    {
                        IWebElement Age = Browser.FindElement(By.CssSelector(".mb .broken tr:nth-last-child(3) td:nth-last-child(1)")); // Не у всех указан пол
                        int start = (Age.Text.IndexOf("(") + 1);
                        int end = Age.Text.LastIndexOf("years)");
                        age = Age.Text.Substring(start, end - start - 1);
                    }
                    TimeSpan span = thisDay - regDate;
                    int deltaDay = span.Days;
                    bool fsb = false;
                    if ((BDay[i].StartsWith("id")) || (BDay[i].StartsWith("tw_")) || (BDay[i].StartsWith("fb_")) || (BDay[i].StartsWith("vk_")) || (BDay[i].StartsWith("gp_"))) // это в отдельную функцию
                    {
                        //MessageBox.Show("eto fsb");
                        fsb = true;
                    }
                    if (deltaDay < Convert.ToInt32(zahodDay.Text))
                    {
                        if (yatutnuzhen7())
                            if (!fsb)
                                BDaypostCreate(BDay[i], Convert.ToInt32(age));
                            else
                                BDaypostCreate("", Convert.ToInt32(age));

                    }
                }
            }

        }

        private  bool yatutnuzhen7()
        {
            //login = "Key_f";
            bool yatut = true;
            List<IWebElement> CommentsName = Browser.FindElements(By.CssSelector(".descr b")).ToList();
            List<IWebElement> CommentsDate = Browser.FindElements(By.CssSelector(".descr")).ToList();
            List<DateTime> Date = Tools4help.GetDateFromComm(CommentsDate);
            int j = CommentsName.Count - 1;
            int mylastcomment = 0;
            do
            {
                if (CommentsName.Count != 0)
                {
                    TimeSpan span = DateTime.Today - Date[j];
                    if (CommentsName[j].Text.ToLower() == login.ToLower() && (span.Days < 3)) // ToLower чтобы не учитывать регистр, нужно проверить работает ли без него
                    {                        
                        yatut = true;
                        mylastcomment = j;
                        return !yatut;
                    }
                    else yatut = false;
                }
                else
                {
                    yatut = false;
                }
                j--;
            }
            while (j > 0);
            return !yatut; // тк нужен я только когда меня нет
        }
        private void BDaypostCreate(string name, int age)
        {
            IWebElement SendMessage = Browser.FindElement(By.Name("post"));
            if (name != "")
                SendMessage.SendKeys("[CENTER] [SIZE=5] Happy B-Day " + name + "![/SIZE] [/CENTER]");
            else
                SendMessage.SendKeys("[CENTER] [SIZE=5] Happy B-Day![/SIZE] [/CENTER]");
            SendMessage.SendKeys(OpenQA.Selenium.Keys.Enter);
            SendMessage.SendKeys(OpenQA.Selenium.Keys.Enter);
            if((age > 0)&&(age < 40))
            SendMessage.SendKeys("[SIZE=30][COLOR=#e1e15a][CENTER]");
            else if (age < 100)
                SendMessage.SendKeys("[SIZE=5][COLOR=#e1e15a][CENTER]");
            else if (age > 100)
                SendMessage.SendKeys("[SIZE=1][COLOR=#e1e15a][CENTER]");
            for (int i = 0; i < Math.Abs(age); i++)
            {
                SendMessage.SendKeys("i");
            }
            SendMessage.SendKeys("[/CENTER][/COLOR][/SIZE]");
            SendMessage.SendKeys(OpenQA.Selenium.Keys.Enter);
            SendMessage.SendKeys("[CENTER][MYPHOTO=1112762][/CENTER]");
            MessageBox.Show("");
            IWebElement SearchPostButton = Browser.FindElement(By.ClassName("commentSubmit"));
            SearchPostButton.Click();
            Thread.Sleep(500); // Немножк
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
