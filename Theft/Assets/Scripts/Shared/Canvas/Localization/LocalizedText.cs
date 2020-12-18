using System;
using UnityEngine;
using UnityEngine.UI;


namespace Game.Shared {

    /**
     * Translates a text field according to the current locale.
     */
    [RequireComponent(typeof(Text))]
    public class LocalizedText : MonoBehaviour {

        /** Preferences instnace */
        private Preferences preferences = null;

        /** Catalan translation */
        public string Catalan = string.Empty;

        /** English translation */
        public string English = string.Empty;

        /** Spanish translation */
        public string Spanish = string.Empty;


        /**
         * Initialization.
         */
        private void Start() {
            preferences = Preferences.GetService();
            preferences.localeChanged += LoadLocaleString;
            LoadLocaleString(preferences.GetLocaleCode());
        }


        /**
         * Replaces the current text by the localized string.
         */
        private void LoadLocaleString(string localeCode) {
            string translation = GetLocaleString(localeCode);

            if (!string.IsNullOrEmpty(translation)) {
                string text = translation.Replace("\\n", Environment.NewLine);
                GetComponent<Text>().text = text;
            }
        }


        /**
         * Obtain a localized string for a language code.
         */
        private string GetLocaleString(string localeCode) {
            switch (localeCode) {
                case "ca": return Catalan;
                case "en": return English;
                case "es": return Spanish;
            }

            return string.Empty;
        }
    }
}
