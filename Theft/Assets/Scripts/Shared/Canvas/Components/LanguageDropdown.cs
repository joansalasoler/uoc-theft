using System;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;


namespace Game.Shared {

    /**
     * Allows toggling on an off an audio group.
     */
    [RequireComponent(typeof(Dropdown))]
    public class LanguageDropdown : MonoBehaviour {

        /** Preferences service */
        private Preferences preferences = null;

        /** Supported locales */
        private string[] locales = {"ca", "en", "es"};

        /** Qualified key of the locale preference */
        private string localeKey = string.Empty;

        /** Reference to the dropdown */
        private Dropdown dropdown = null;


        /**
         * Initialization.
         */
        private void Start() {
            dropdown = GetComponent<Dropdown>();
            preferences = Preferences.GetService();
            localeKey = preferences.GetKey("locale.code");
            dropdown.value = Array.IndexOf(locales, preferences.GetLocaleCode());
            dropdown.onValueChanged.AddListener(OnDropdownValueChanged);
        }


        /**
         * Set the locale when the dropdown changes.
         */
        private void OnDropdownValueChanged(int value) {
            preferences.SetLocaleCode(locales[value]);
        }
    }
}
