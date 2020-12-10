using System;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Shared {

    /**
     * Shows an indicator sprite according to a state.
     */
    public class StatusIndicator : MonoBehaviour {

        /** Sprite for when the state is full */
        [SerializeField] private Sprite full = null;

        /** Sprite for when the state is half */
        [SerializeField] private Sprite half = null;

        /** Sprite for when the state is empty */
        [SerializeField] private Sprite empty = null;

        /** Image of the indicator */
        private Image image;


        /**
         * Initialization.
         */
        private void Awake() {
            image = GetComponent<Image>();
            ChangeToEmpty();
        }


        /**
         * Set the indicator as full.
         */
        public void ChangeToFull() {
            SetSprite(full);
        }


        /**
         * Set the indicator as half.
         */
        public void ChangeToHalf() {
            SetSprite(half);
        }


        /**
         * Set the indicator as empty.
         */
        public void ChangeToEmpty() {
            SetSprite(empty);
        }


        /**
         * Set this indicator's sprite.
         */
        private void SetSprite(Sprite sprite) {
            if (image != null) {
                image.sprite = sprite;
                image.SetAllDirty();
            }
        }
    }
}
