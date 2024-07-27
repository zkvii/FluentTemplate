using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.UI.Xaml;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace FluentTemplate
{
    public static class SettingsHelper
    {
        public static string SettingsPath = @"Data/Settings.json";

        public static Settings AppSettings;
        public static void LoadSettings()
        {
            // Load settings from file
            using var file = System.IO.File.OpenText(SettingsPath);
            AppSettings = JsonConvert.DeserializeObject<Settings>(file.ReadToEnd());
            Debug.WriteLine(AppSettings);

            // Apply settings
        }

        public static void SaveSettings()
        {
            var content= JsonConvert.SerializeObject(AppSettings);
            System.IO.File.WriteAllText(SettingsPath, content);
            // Save settings to file
        }
    }

    public enum BackDrop
    {
        Mica,
        Acrylic,
        Transparent,
        Normal
    }

    public class Settings
    {
        [JsonConverter(typeof(StringEnumConverter))]

        public ApplicationTheme Theme { get; set; }

        [JsonConverter(typeof(StringEnumConverter))]
        public BackDrop BackDrop { get; set; }
    }
}