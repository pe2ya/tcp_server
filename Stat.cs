using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using System.IO;
using System.Configuration;
using System.Security.Cryptography;

namespace Server
{
    static class Stat
    {
        public static string GetConf(string key)
        {
            var appSettings = ConfigurationManager.AppSettings;
            string result = appSettings[key];
            return result;
        }

        public static (string, string) Encrypt(string input)
        {
            string result = string.Empty;
            string key = string.Empty;

            for (int i = 0; i < input.Length; i++)
            {
                char input_char = input[i];
                char key_char = (char)RandomNumberGenerator.GetInt32(150);

                result += (char)(input_char ^ key_char);
                key += (char)(key_char);
            }

            return (result, key);
        }

        public static string Decrypt(string input, string key)
        {
            string result = string.Empty;

            for (int i = 0; i < input.Length; i++)
            {
                char input_char = input[i];
                char key_char = key[i];

                result += (char)(input_char ^ key_char);
            }

            return result;
        }

    }
}
