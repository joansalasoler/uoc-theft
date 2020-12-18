using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.FullScreenMode;


namespace Game.Shared {

    /**
     * Allows changing the fullscreen/windowed mode of the game.
     */
    [RequireComponent(typeof(Dropdown))]
    public class FullscreenDropdown : MonoBehaviour {

        /** Preferences service */
        private Preferences preferences = null;

        /** Supported full screen modes */
        private FullScreenMode[] modes = { FullScreenWindow, Windowed };

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
         * Initialize the dropdown options.
         */
        private void InitializeOptions(Dropdown dropdown) {
            FullScreenMode mode = preferences.GetFullScreenMode();
            dropdown.value = Array.IndexOf(modes, mode);
            dropdown.onValueChanged.AddListener(OnDropdownValueChanged);
        }


        /**
         * Set the fullscreen mode when an option is chosen.
         */
        private void OnDropdownValueChanged(int value) {
            preferences.SetFullScreenMode(modes[value]);
        }
    }
}
