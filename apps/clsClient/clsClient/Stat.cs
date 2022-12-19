using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.IO;
using System.Configuration;

namespace clsClient
{
    public static class Stat
    {
        /// <summary>
        /// Static class; contains static method which you can use in any another class
        /// <param name="path"> path to the config file</param>
        /// </summary>
        static string path = "../../../../../../config/app.config";


        /// <summary>
        /// Get information from config file
        /// </summary>
        /// <param name="key">The name of config key</param>
        /// <returns>value from config key in string</returns>

        public static string GetConf(string key)
        {
            string result = ConfigurationManager.AppSettings.Get(key);
            //Configuration config = ConfigurationManager.OpenExeConfiguration(path);
            //string result = config.AppSettings.Settings[key].Value;
            return result;
        }

    }
}