using TMPro;
using UnityEngine;

namespace Localization
{
    public class LocalizationText : MonoBehaviour
    {
        [SerializeField]
        private string key;

        private TextMeshProUGUI mText;

        [SerializeField]
        private LocalizationSystem localizationSystem = null;

        private void Start()
        {
            mText = GetComponent<TextMeshProUGUI>();
            localizationSystem.onValueChange += Load;
            Load();
        }

        private void Load()
        {
            if(mText!=null)
                mText.text=localizationSystem.GetValue(key);
        }

        private void OnDestroy() => localizationSystem.onValueChange -= Load;
    }
}
