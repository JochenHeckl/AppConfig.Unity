using System;
using System.IO;
using Unity.Logging;
using UnityEditor;
using UnityEngine;

public static class AppConfigMenu
{
    [MenuItem("AppConfig/Create default AppConfigData class")]
    public static void MakeDefaultAppConfig()
    {
        var productName = Application.productName;
        var configFileClassName = $"{productName}Config";
        var configFileName = $"{configFileClassName}.cs";

        var configFilePath = Path.Combine(Application.dataPath, configFileName);

        if (!File.Exists(configFilePath))
        {
            File.WriteAllText(
                configFilePath,
                $"using System;{Environment.NewLine}{Environment.NewLine}[Serializable]{Environment.NewLine}public class {configFileClassName}{Environment.NewLine}{{{Environment.NewLine}public string sampleString = \"sampleValue\";{Environment.NewLine}}}{Environment.NewLine}"
            );

            Log.Info($"Default AppConfig was written to {configFilePath}.");

            EditorUtility.OpenWithDefaultApp(configFilePath);
        }
        else
        {
            Log.Error($"The file {configFilePath} already exists.");
        }
    }
}
