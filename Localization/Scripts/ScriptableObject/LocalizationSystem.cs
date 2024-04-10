using System;
using System.Collections.Generic;
using UnityEngine;

namespace Localization
{
    [CreateAssetMenu(fileName = "BDD", menuName = "Traduction", order = 0)]
    public class LocalizationSystem : ScriptableObject
    {
        public string currentLang;
        public Dictionary<string, Dictionary<string, string>> langagesTraduction;

        public delegate void OnValueChange();
        public event OnValueChange onValueChange;

        public string CurrentLang
        {
            get
            {
                return currentLang;
            }
            set
            {
                currentLang = value;
                onValueChange?.Invoke();
            }
        }

        public string GetValue(string key)
        {
            if (langagesTraduction[CurrentLang].TryGetValue("[" + key + "]", out string value))
            {
                return value;
            }
            else
            {
                return key + " [NOT FOUND]";
            }
        }

        public string GetValueFormat(string key, params string[] values)
        {
            string value = GetValue(key);
            return string.Format(value, values);
        }

        public List<SystemLanguage> GetLanguageAvailable()
        {
            List<SystemLanguage> list = new();
            foreach ( string key in langagesTraduction.Keys)
            {
                list.Add((SystemLanguage)Enum.Parse(typeof(SystemLanguage), key));
            }
            return list;
        }
    }
}
