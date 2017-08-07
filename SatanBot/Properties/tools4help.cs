using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium;
using System.Globalization;

namespace SatanBot
{
    class Tools4help
    {
        public static List<string> GetString(List<IWebElement> WebEl) // Необходим, тк при переходе на другую страницу, поля List<IWebElement> уходят
        {
            List<string> newS = new List<string>();
            for (int i = 0; i < WebEl.Count; i++)
            {
                newS.Add(WebEl[i].Text);
            }
            return newS;
        }

        public static List<DateTime> GetDateFromComm(List<IWebElement> WebEl) // Получаем список дат комментариев
        {
            List<string> Datestr = new List<string>();
            List<DateTime> cDate = new List<DateTime>();
            for (int i = 0; i < WebEl.Count; i++)
            {
                int start = WebEl[i].Text.IndexOf("\r\n");
                int end = WebEl[i].Text.LastIndexOf(',');
                Datestr.Add(WebEl[i].Text.Substring(start + 2, end - start - 2)); // Вытаскиваем только нужную часть, с датой
                //Datestr[i] = Datestr[i].Substring(0, );
                cDate.Add(DateTime.ParseExact(Datestr[i], "dd MMM yyyy", CultureInfo.InvariantCulture));
            }
            return cDate;
            
        }
    }
}
