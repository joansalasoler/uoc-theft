using System;
using UnityEngine;
using UnityEngine.UI;


namespace Game.Shared {

    /**
     * Allows toggling on an off an audio group.
     */
    [RequireComponent(typeof(Dropdown))]
    public class VolumeDropdown : MonoBehaviour {

        /** Parameter of the mixer to control */
        [SerializeField] private string parameter = string.Empty;

        /** Preferences service */
        private Preferences preferences = null;

        /** Volume values */
        private float[] volumes = { 0.0f, -80f };

        /** Reference to the dropdown */
        private Dropdown dropdown = null;


        /**
         * Initialization.
         */
        private void Start() {
            dropdown = GetComponent<Dropdown>();
            preferences = Preferences.GetService();
            InitializeOptions(dropdown);
        }


        /**
         * Initialize the dropdown options.
         */
        private void InitializeOptions(Dropdown dropdown) {
            float volume = preferences.GetAudioVolume(parameter);
            dropdown.value = Array.IndexOf(volumes, volume);
            dropdown.onValueChanged.AddListener(OnDropdownValueChanged);
        }


        /**
         * Set the group's volume when the dropdown changes.
         */
        private void OnDropdownValueChanged(int value) {
            preferences.SetAudioVolume(parameter, volumes[value]);
        }
    }
}
