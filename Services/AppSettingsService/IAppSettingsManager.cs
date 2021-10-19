using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Security;
using System.Text;
using DayZTediratorToolz.Helpers;
using DayZTediratorToolz.Models;
using Newtonsoft.Json;

namespace DayZTediratorToolz.Services
{

    public interface IAppSettingsManager
    {
        string GetQueryUri();
        string GetServerIp();
        string GetServerPort();
        string GetGamePath();
        string GetModPagePath();
    }


    public class AppSettingsManager : IAppSettingsManager
    {
        public AppSettingsManager(string appSettings)
        {
            AppConfigurationModel.Root configRoot = JsonConvert.DeserializeObject<AppConfigurationModel.Root>(appSettings);
            InititializeSettings(configRoot.AppConfiguration);
        }

        private AppConfigurationModel.AppConfiguration LocalAppConfiguration { get; set; }

        private void InititializeSettings(AppConfigurationModel.AppConfiguration config)
        {
            LocalAppConfiguration = config;
        }

        public string GetServerIp() => LocalAppConfiguration.ServerIP;

        public string GetServerPort() => LocalAppConfiguration.ServerPort;
        public string GetQueryUri() => LocalAppConfiguration.QueryUri;
        public string GetGamePath() => LocalAppConfiguration.GameExecutablePath;
        public string GetModPagePath() => LocalAppConfiguration.ModPagePath;
    }
}
