using Core.Helpers;
using Core.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Configuration;
using Newtonsoft.Json;
using System.Linq;

namespace WinApp
{
    public interface ISettingsManager
    {
        KeyValueConfigurationCollection BasicSettings { get; }
        string GetSettingValue(string key);
        string LogFilePath { get; }
        bool IsDevelopment { get; }
    }

    public class SettingsManager : ISettingsManager
    {
        private KeyValueConfigurationCollection _settings;
        

        public SettingsManager()
        {
            this._settings = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None)
                                                 .AppSettings.Settings;

            this._isDevelopment = GetSettingValue(AppSettingsKey.Environment) == "Development";
        }

        public string GetSettingValue(string key) =>_settings[key].Value;

        public string LogFilePath => GetSettingValue(AppSettingsKey.LogFile);

        public KeyValueConfigurationCollection BasicSettings => _settings;


        bool _isDevelopment = true;
        public  bool IsDevelopment => _isDevelopment;

    }
}
