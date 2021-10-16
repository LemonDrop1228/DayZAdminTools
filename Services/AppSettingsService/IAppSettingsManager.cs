using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Security;
using System.Text;
using DayZTediratorToolz.Helpers;

namespace DayZTediratorToolz.Services
{

    public interface IAppSettingsManager
    {
        string GetServerIp();
        string GetServerPort();
    }


    public class AppSettingsManager : IAppSettingsManager
    {
        public AppSettingsManager(NameValueCollection appSettings)
        {
            InititializeSettings(appSettings);
        }

        string GameServerIp { get; set; }
        string GameServerPort { get; set; }

        private void InititializeSettings(NameValueCollection nvCollection)
        {
            GameServerIp = nvCollection.Get("MyServerIP");
            GameServerPort = nvCollection.Get("MyServerPort");
        }

        public string GetServerIp() => GameServerIp;

        public string GetServerPort() => GameServerPort;
    }
}
