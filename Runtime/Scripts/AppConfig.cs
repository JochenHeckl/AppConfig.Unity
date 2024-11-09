using System;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace JH.AppConfig
{
    public class AppConfig<ConfigDataType>
        where ConfigDataType : new()
    {
        public ConfigDataType Data { get; private set; }
        public string ConfigFilePath { get; private set; }

        private AppConfig() { }

        public AppConfig(string projectName)
            : this(projectName, MakeDefaultConfigFileName()) { }

        public AppConfig(string projectName, string configFileName)
        {
            ConfigFilePath = MakeConfigFilePath(projectName, configFileName);
            Data = ReadConfigData(ConfigFilePath);
        }

        public static AppConfig<ConfigDataType> CreateFromFile(string configFilePath)
        {
            return new AppConfig<ConfigDataType>()
            {
                ConfigFilePath = configFilePath,
                Data = ReadConfigData(configFilePath),
            };
        }

        public void Save()
        {
            WriteConfigData();
        }

        private static ConfigDataType ReadConfigData(string configFileName)
        {
            var directory = Path.GetDirectoryName(configFileName);

            if (directory != null)
            {
                if (!Directory.Exists(directory))
                {
                    Directory.CreateDirectory(directory);
                }    
            }

            if (!File.Exists(configFileName))
            {
                var newConfig = new ConfigDataType();

                File.WriteAllText(configFileName, JsonUtility.ToJson(newConfig));

                return newConfig;
            }

            return JsonUtility.FromJson<ConfigDataType>(File.ReadAllText(configFileName));
        }

        private void WriteConfigData()
        {
            var directory = Path.GetDirectoryName(ConfigFilePath);

            if ((directory != null) && !Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }

            File.WriteAllText(ConfigFilePath, JsonUtility.ToJson(Data, true));
        }

        private static string MakeDefaultConfigFileName()
        {
            return $"{typeof(ConfigDataType).Name}.json";
        }

        private static string MakeConfigFilePath(string projectName, string configFileName)
        {
            var appDirectory = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            var projectDirectory = Path.Combine(appDirectory, projectName);
            var configFilePathName = Path.Combine(projectDirectory, configFileName);

            return configFilePathName;
        }
    }
}
