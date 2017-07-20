using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;

namespace SatanBot
{
    class Shifrovanie
    {
        private static string RsaKey = "yttetvyucryextbtybtcrtyxetvtutbunulnyucrxr";
        private static int bias = 666;
       
        public static string Encode(string str)
        {
                        
            var provider = new RSACryptoServiceProvider(); // Инициализирует новый экземпляр класса RSACryptoServiceProvider, используя ключ по умолчанию.
            //RsaKey = provider.ToXmlString(true); // ToXmlString Метод создает строку XML, содержащего открытый и закрытый ключи текущего RSA объекта или содержит только открытый ключ текущего RSA объекта.
            return Encoding.UTF8.GetString(provider.Encrypt(Encoding.UTF8.GetBytes(str), true)); // В Encrypt передаем байты, возращаем строку
        }

        public static string Decode(string str)
        {
            if (RsaKey != null)
            {
                var provider = new RSACryptoServiceProvider(); // Инициализирует новый экземпляр класса RSACryptoServiceProvider, используя ключ по умолчанию.
                provider.FromXmlString(RsaKey); // Инициализирует объект RSA, используя данные ключа из строки XML.
                return Encoding.UTF8.GetString(provider.Decrypt(Encoding.UTF8.GetBytes(str), true)); // В Decrypt передаем байты, возращаем строку
            }
            else return null;
        }
    }
}
