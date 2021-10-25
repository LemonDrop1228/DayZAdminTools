using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using DayZTediratorToolz.Models;
using Newtonsoft.Json;
using static DayZTediratorToolz.Helpers.DayZTediratorConstants;

namespace DayZTediratorToolz.Services.ToolConfigService
{
    public interface IToolConfigService
    {
        IConfigObj GetConfigObj(Tools tool);
        void SaveConfigData();
    }
    
    public class ToolConfigService : IToolConfigService
    {
        private List<ConfigInfo> ConfigDictionary { get; set;}
        private const string configPath = "Configs/";

        public ToolConfigService()
        {
            InitializeService();
            TryLoadingConfigs();
        }

        private void InitializeService()
        {
            ConfigDictionary = new List<ConfigInfo>()
            {
                new ConfigInfo(Tools.Admin, "AdminToolSettings.config.json"),
                new ConfigInfo(Tools.Types, "TypesToolSettings.config.json")
            };

            if (!System.IO.Directory.Exists(configPath))
                Directory.CreateDirectory(configPath);
        }

        private void TryLoadingConfigs() => ConfigDictionary.ForEach(c => LoadingConfig(c));

        public IConfigObj GetConfigObj(Tools tool) =>
            ConfigDictionary.FirstOrDefault(t => t.ToolType == tool)
                .ConfigObj;

        public void SaveConfigData()
        {
            ConfigDictionary.ForEach(c => WriteConfigToFile(c));
        }

        private void WriteConfigToFile(ConfigInfo configInfo)
        {
            try
            {
                var configJson = JsonConvert.SerializeObject(configInfo.ConfigObj);
                File.WriteAllText(GetMergedPath(configInfo.FileName),configJson);
            }
            catch (Exception e)
            {
                throw new InvalidOperationException("Failed to save tool configs.");
            }
        }

        private void LoadingConfig(ConfigInfo configInfo)
        {
            
            try
            {
                var configStrData = System.IO.File.Exists(GetMergedPath(configInfo.FileName))
                    ? System.IO.File.ReadAllText(GetMergedPath(configInfo.FileName))
                    : string.Empty;
                switch (configInfo.ToolType)
                {
                    case Tools.Admin:
                        configInfo.ConfigObj = JsonConvert.DeserializeObject<AdminCfg>(configStrData) ?? new AdminCfg();
                        break;
                    case Tools.Types:
                        configInfo.ConfigObj = JsonConvert.DeserializeObject<TypesCfg>(configStrData) ?? new TypesCfg
                        {
                            RecentTypesHistory = new RecentTypesHistoryModel().InitializeEmpty()
                        };
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
            catch (Exception e)
            {
                throw new InvalidOperationException("Unable to load tool configs.");
            }
        }

        private string GetMergedPath(string fileName) => $"{configPath}{fileName}";


        public record ConfigInfo
        {
            public ConfigInfo(Tools toolType, string fileName)
            {
                ToolType = toolType;
                FileName = fileName;
            }

            public Tools ToolType { get; set; }
            public string FileName { get; set; }
            public IConfigObj ConfigObj { get; set; }
        }
    }
}