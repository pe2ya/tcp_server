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
        public static string GetConf(string key)
        {
            var appSettings = ConfigurationManager.AppSettings;
            string result = appSettings[key];
            return result;
        }

    }
}