using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Localization
{
    public class LocalizationManager :  MonoBehaviour
    {
        public LocalizationSystem traductionSystem;
        private static LocalizationManager instance;   // GameSystem local instance
        public SystemLanguage systemLanguageTest;
        public SystemLanguage defaultLangage;
        public static readonly string KEY_LANG = "lang";

        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
                DontDestroyOnLoad(gameObject);
                TextAsset[] testResources = Resources.LoadAll<TextAsset>("Traduction").ToArray();
                traductionSystem.langagesTraduction = new Dictionary<string, Dictionary<string, string>>();
                for (int i = 0; i < testResources.Length; ++i)
                {
                    LoadJson(testResources[i].name);
                }
                if (!PlayerPrefs.HasKey(KEY_LANG))
                {
                    traductionSystem.CurrentLang = Application.systemLanguage.ToString();
                    PlayerPrefs.SetString(KEY_LANG, traductionSystem.CurrentLang);
                }
                else
                    traductionSystem.CurrentLang = PlayerPrefs.GetString(KEY_LANG);
            }
            else
            {
                Destroy(gameObject);
            }
        }

        public void LoadJson(string lang)
        {
            LocalizationData loadedData = new();
            TextAsset text = Resources.Load<TextAsset>("Traduction/" + lang);
            traductionSystem.langagesTraduction.Add(lang.Split('_')[1], new Dictionary<string, string>());
            if (text != null)
            {
                loadedData = JsonUtility.FromJson<LocalizationData>(text.text);
            }
            if (loadedData.items != null)
            {
                for (int i = 0; i < loadedData.items.Length; i++)
                {
                    traductionSystem.langagesTraduction[lang.Split('_')[1]].Add(loadedData.items[i].key, loadedData.items[i].value);
                }          
            }
        }

        [System.Serializable]
        public class LocalizationData
        {
            public LocalizationItem[] items;
        }

        [System.Serializable]
        public class LocalizationItem
        {
            public string key;
            public string value;
        }
    }
}