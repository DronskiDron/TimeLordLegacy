﻿using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;

using TaleWorlds.Library;

namespace TimeLord
{
    internal sealed class ExternalSavedValues
    {
        internal string SavesDir { get; private set; }
        internal string DataStorePath { get; private set; }

        internal ExternalSavedValues(string moduleName)
        {
            try
            {
                if (string.IsNullOrEmpty(moduleName))
                {
                    throw new ArgumentNullException(nameof(moduleName));
                }

                var personalDir = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
                var userDir = Path.Combine(personalDir, "Mount and Blade II Bannerlord");
                SavesDir = Path.Combine(userDir, "Game Saves");
                DataStorePath = Path.Combine(SavesDir, $"{moduleName}.esv.json");

                if (!File.Exists(DataStorePath))
                {
                    return;
                }

                _map = JsonConvert.DeserializeObject<Dictionary<string, SavedValues>>(File.ReadAllText(DataStorePath),
                                                                                      new JsonSerializerSettings { ObjectCreationHandling = ObjectCreationHandling.Replace }) ?? new();
            }
            catch (Exception e)
            {
                Debug.PrintError(e.Message, e.StackTrace); Debug.WriteDebugLineOnScreen(e.ToString());  Debug.SetCrashReportCustomString(e.Message); Debug.SetCrashReportCustomStack(e.StackTrace); 
            }
        }

        internal void Set(string charName, string clanName, SavedValues savedValues) =>
            _map[PackKey(charName, clanName)] = savedValues;

        internal SavedValues? Get(string charName, string clanName) =>
            _map.TryGetValue(PackKey(charName, clanName), out SavedValues ret) ? ret : null;

        internal void Serialize()
        {
            Directory.CreateDirectory(SavesDir);
            File.WriteAllText(DataStorePath, JsonConvert.SerializeObject(_map, Formatting.Indented));
        }

        private string PackKey(string charName, string clanName) => charName + "/" + clanName;

        private readonly Dictionary<string, SavedValues> _map = new();
    }
}
