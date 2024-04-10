using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace Localization
{
    public class ChangementDeLangueExemple : MonoBehaviour
    {
        [SerializeField]
        private TMP_Dropdown dropdownLangue;
        private List<SystemLanguage> langueEnum;
        [SerializeField]
        LocalizationSystem trad;
        // Start is called before the first frame update
        void Start()
        {
            dropdownLangue.onValueChanged.AddListener(ChangeLangue);
            trad.onValueChange += Load;
            Load();
        }
        private void Load()
        {
            langueEnum = new List<SystemLanguage>(trad.GetLanguageAvailable());
            dropdownLangue.options.Clear();
            langueEnum.ForEach(x =>
            {
                dropdownLangue.options.Add(new TMP_Dropdown.OptionData(trad.GetValue(x.ToString().ToUpper())));
            });
            dropdownLangue.value = langueEnum.FindIndex(x => x.ToString() == trad.currentLang.ToString());
            dropdownLangue.RefreshShownValue();
        }
        private void ChangeLangue(int i)
        {
            trad.CurrentLang = langueEnum[i].ToString();
            PlayerPrefs.SetString(LocalizationManager.KEY_LANG, trad.CurrentLang);
        }

    }
}
