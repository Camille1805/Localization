using TMPro;
using UnityEngine;

namespace Localization
{
    public class LocalizationTextFormatExemple : MonoBehaviour
    {
        [SerializeField]
        private string key;
        [SerializeField]
        private string prenom;
        [SerializeField]
        private LocalizationSystem localizationSystem = null;
        private TextMeshProUGUI mText;

        private void Start()
        {
            mText = GetComponent<TextMeshProUGUI>();
            localizationSystem.onValueChange += Load;
            Load();
        }

        private void Load()
        {
            if (mText != null)
                mText.text = localizationSystem.GetValueFormat(key, prenom);
        }
        private void OnDestroy() => localizationSystem.onValueChange -= Load;
    }
}
