using System;
using UnityEngine;

namespace Game.Shared {

    /**
     * Delegate trigger events for collisions with the player.
     */
    [RequireComponent(typeof(Collider))]
    public class OnPlayerTrigger: MonoBehaviour {

        /** Invoked when the player enters the trigger */
        public Action<Collider> onTriggerEnter = null;

        /** Invoked when the player leaves the trigger */
        public Action<Collider> onTriggerExit = null;


        /**
         * Invoke the delegate on entering a player trigger.
         */
        private void OnTriggerEnter(Collider collider) {
            if (collider.gameObject.CompareTag("Player")) {
                if (onTriggerEnter != null) {
                    onTriggerEnter.Invoke(collider);
                }
            }
        }


        /**
         * Invoke the delegate on leaving a player trigger.
         */
        private void OnTriggerExit(Collider collider) {
            if (collider.gameObject.CompareTag("Player")) {
                if (onTriggerExit != null) {
                    onTriggerExit.Invoke(collider);
                }
            }
        }
    }
}
