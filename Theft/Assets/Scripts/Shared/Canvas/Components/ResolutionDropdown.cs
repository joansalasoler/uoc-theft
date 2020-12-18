using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


namespace Game.Shared {

    /**
     * Allows changing the screen resolution from a dropdown.
     */
    [RequireComponent(typeof(Dropdown))]
    public class ResolutionDropdown : MonoBehaviour {

        /** Preferences service */
        private Preferences preferences = null;

        /** Supported screen resolutions */
        private Resolution[] resolutions = null;

        /** Dropdown options list */
        private List<string> options = new List<string>();

        /** Reference to the dropdown */
        private Dropdown dropdown = null;


        /**
         * Initialization.
         */
        private void Start() {
            if (Screen.resolutions.Length < 1) {
                gameObject.SetActive(false);
            } else {
                dropdown = GetComponent<Dropdown>();
                preferences = Preferences.GetService();
                InitializeOptions(dropdown);
            }
        }


        /**
         * Initialize the array of resolutions an dropdown options.
         */
        private void InitializeOptions(Dropdown dropdown) {
            Resolution current = preferences.GetResolution();
            resolutions = preferences.GetScreenResolutions();

            foreach (Resolution resolution in resolutions) {
                options.Add(resolution.ToString());
            }

            dropdown.AddOptions(options);
            dropdown.value = Array.IndexOf(resolutions, current);
            dropdown.onValueChanged.AddListener(OnDropdownValueChanged);
        }


        /**
         * Set the screen resolution when an option is chosen.
         */
        private void OnDropdownValueChanged(int index) {
            preferences.SetResolution(resolutions[index]);
        }
    }
}
