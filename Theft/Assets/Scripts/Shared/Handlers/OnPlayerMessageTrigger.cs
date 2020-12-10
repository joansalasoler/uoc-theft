using System;
using System.Collections.Generic;
using UnityEngine;
using Game.Shared;

namespace Game.Shared {

    /**
     * Show a message to the player when a collision occurs whith the field
     * of view of the character. Each message will only be shown once,
     * regardless of the object where it is attached.
     */
    public class OnPlayerMessageTrigger : MonoBehaviour {

        /** Set of all the messages */
        private static Dictionary<string, bool> messages = new Dictionary<string, bool>();

        /** Text of the message */
        [SerializeField] private string text = string.Empty;


        /**
         * Initialization.
         */
        private void Awake() {
            if (!messages.ContainsKey(text)) {
                messages.Add(text, false);
            }
        }


        /**
         * Attach the event handlers.
         */
        private void OnEnable() {
            OnFieldOfViewTrigger.onTriggerEnter += OnFieldOfViewEnter;
        }


        /**
         * Dettach the event handlers.
         */
        private void OnDisable() {
            OnFieldOfViewTrigger.onTriggerEnter -= OnFieldOfViewEnter;
        }


        /**
         * Show a message when the collision happens.
         */
        private void OnFieldOfViewEnter(Collider collider) {
            if (enabled && gameObject == collider.gameObject) {
                if (messages[text] == false) {
                    MessageService.Push(text);
                    messages[text] = true;
                }

                Destroy(this);
            }
        }
    }
}
