using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.UI.Dropdown;


namespace Game.Shared {

    /**
     * Translates dropdown options according to the current locale.
     */
    [RequireComponent(typeof(Dropdown))]
    public class LocalizedDropdown : MonoBehaviour {

        /** Preferences instnace */
        private Preferences preferences = null;

        /** Catalan translation */
        public List<OptionData> Catalan = null;

        /** English translation */
        public List<OptionData> English = null;

        /** Spanish translation */
        public List<OptionData> Spanish = null;


        /**
         * Initialization.
         */
        private void Start() {
            preferences = Preferences.GetService();
            preferences.localeChanged += LoadLocaleOptions;
            LoadLocaleOptions(preferences.GetLocaleCode());
        }


        /**
         * Replaces the current options with the localized options.
         */
        private void LoadLocaleOptions(string localeCode) {
            List<OptionData> options = GetLocaleOptions(localeCode);

            if (options != null && options.Count > 0) {
                GetComponent<Dropdown>().options = options;
            }
        }


        /**
         * Obtain a localized string for a language code.
         */
        private List<OptionData> GetLocaleOptions(string code) {
            switch (code) {
                case "ca": return Catalan;
                case "en": return English;
                case "es": return Spanish;
            }

            return null;
        }
    }
}
