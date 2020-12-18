using System;
using UnityEngine;
using UnityEngine.Audio;
using static System.Globalization.CultureInfo;


namespace Game.Shared {

    /**
     * Utility class to manage the player preferences.
     */
    public class Preferences: MonoBehaviour {

        /** Invoked when the locale changed */
        public Action<string> localeChanged = null;

        /** Singleton service instance */
        private static Preferences service = null;

        /** Current user locale code */
        private string localeCode = "en";

        /** Namespace of the player preferences */
        public string Namespace = string.Empty;

        /** Audio mixer to control */
        public AudioMixer Mixer = null;


        /**
         * Makes the service a singleton.
         */
        private void Awake() {
            if (service == null) {
                service = this;
                DontDestroyOnLoad(gameObject);
                LoadLocalePreference("locale.code");
            } else if (service != this) {
                Destroy(gameObject);
            }
        }


        /**
         * Applies the game preferences.
         */
        private void Start() {
            LoadVolumePreference("music.volume");
            LoadVolumePreference("effects.volume");
        }


        /**
         * Obtains this service's singleton instance.
         */
        public static Preferences GetService() {
            return service;
        }


        /**
         * Obtain a qualified name for a preferences key.
         */
        public string GetKey(string key) {
            return $"{Namespace}.{key}";
        }


        /**
         * Current user locale code.
         */
        public string GetLocaleCode() {
            return localeCode;
        }


        /**
         * Sets the locale code.
         */
        public void SetLocaleCode(string code) {
            localeCode = code;
            PlayerPrefs.SetString(GetKey("locale.code"), code);
            if (localeChanged != null) localeChanged.Invoke(code);
        }


        /**
         * Get the fullscreen mode.
         */
        public FullScreenMode GetFullScreenMode() {
            int fallback = (int) Screen.fullScreenMode;
            int value = PlayerPrefs.GetInt(GetKey("screen.mode"), fallback);

            return (FullScreenMode) value;
        }


        /**
         * Set the fullscreen mode.
         */
        public void SetFullScreenMode(FullScreenMode mode) {
            Screen.fullScreenMode = mode;
            PlayerPrefs.SetInt(GetKey("screen.mode"), (int) mode);
        }


        /**
         * Gets the volume of an audio group.
         */
        public float GetAudioVolume(string key) {
            return PlayerPrefs.GetFloat(GetKey($"audio.{key}"), 0f);
        }


        /**
         * Sets the volume of an audio group.
         */
        public void SetAudioVolume(string key, float volume) {
            service.Mixer.SetFloat(key, volume);
            PlayerPrefs.SetFloat(GetKey($"audio.{key}"), volume);
        }


        /**
         * Get the screen width in pixels.
         */
        public int GetScreenWidth() {
            Resolution fallback = Screen.currentResolution;
            return PlayerPrefs.GetInt(GetKey("screen.width"), fallback.width);
        }


        /**
         * Get the screen height in pixels.
         */
        public int GetScreenHeight() {
            Resolution fallback = Screen.currentResolution;
            return PlayerPrefs.GetInt(GetKey("screen.height"), fallback.height);
        }


        /**
         * Get the screen refresh rate.
         */
        public int GetRefreshRate() {
            Resolution fallback = Screen.currentResolution;
            return PlayerPrefs.GetInt(GetKey("screen.refreshRate"), fallback.refreshRate);
        }


        /**
         * Returns a sorted array of supported screen resolutions.
         */
        public Resolution[] GetScreenResolutions() {
            Resolution[] resolutions = Screen.resolutions;

            Array.Sort(resolutions, (a, b) => {
                return (a.width == b.width) ?
                    (a.height < b.height ? 1 : -1) :
                    (a.width < b.width ? 1 : -1);
            });

            return resolutions;
        }


        /**
         * Get the screen resolution. Notice that Screen.currentResolution
         * always returns the same value on Unix systems, thus the resolution
         * is obtained here from the stored player preferences.
         */
        public Resolution GetResolution() {
            int width = GetScreenWidth();
            int height = GetScreenHeight();
            int refreshRate = GetRefreshRate();

            return Array.Find(Screen.resolutions, (r) => {
                return width == r.width &&
                       height == r.height &&
                       refreshRate == r.refreshRate;
            });
        }


        /**
         * Set the screen resolution.
         */
        public void SetResolution(Resolution value) {
            bool fullscreen = Screen.fullScreen;
            Screen.SetResolution(value.width, value.height, fullscreen);
            PlayerPrefs.SetInt(GetKey("screen.width"), value.width);
            PlayerPrefs.SetInt(GetKey("screen.height"), value.height);
            PlayerPrefs.SetInt(GetKey("screen.refreshRate"), value.refreshRate);
        }


        /**
         * Reads the language code preference.
         */
        private void LoadLocalePreference(string key) {
            string fallback = CurrentUICulture.TwoLetterISOLanguageName;
            localeCode = PlayerPrefs.GetString(GetKey(key), fallback);
        }


        /**
         * Applies an audio volume preference for a settings key.
         */
        private void LoadVolumePreference(string key) {
            service.Mixer.SetFloat(key, GetAudioVolume(key));
        }
    }
}
