using System;
using System.Security.Cryptography;
using HtmlAgilityPack;
using System.Configuration;

namespace Cipher
{
    public static class Cipher
    {
        static HtmlWeb website = new HtmlWeb();

        static string url = ConfigurationManager.AppSettings.Get("url");
        static HtmlDocument document = website.Load(url);

        static string key = document.DocumentNode.OuterHtml;
        static int key_length = key.Length;

        public static string Encrypt(string input)
        {
            string result = string.Empty;
            int index = 0;
            char input_char, key_char;

            for (int i = 0; i < input.Length; i++)
            {
                input_char = (char) input[i];
                key_char = (char) key[i];

                result += (char)(input_char ^ key_char);

                index = (index + 1) >= key_length ? 0 : index++;
            }

            return result;
        }

        public static string Decrypt(string input)
        {
            string result = string.Empty;
            int index = 0;
            char input_char, key_char;

            for (int i = 0; i < input.Length; i++)
            {
                input_char = (char) input[i];
                key_char = (char) key[i];

                result += (char)(input_char ^ key_char);

                index = (index + 1) >= key_length ? 0 : index++;
            }

            return result;
        }
    }
}