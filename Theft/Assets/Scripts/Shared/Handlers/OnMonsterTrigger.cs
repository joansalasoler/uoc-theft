using System;
using UnityEngine;

namespace Game.Shared {

    /**
     * Delegate trigger events for collisions with monsters.
     */
    [RequireComponent(typeof(Collider))]
    public class OnMonsterTrigger: MonoBehaviour {

        /** Invoked when the player enters the trigger */
        public Action<Collider> onTriggerEnter = null;

        /** Invoked when the player leaves the trigger */
        public Action<Collider> onTriggerExit = null;


        /**
         * Invoke the delegate on entering a player trigger.
         */
        private void OnTriggerEnter(Collider collider) {
            if (collider.gameObject.CompareTag("Monster")) {
                if (onTriggerEnter != null) {
                    onTriggerEnter.Invoke(collider);
                }
            }
        }


        /**
         * Invoke the delegate on leaving a player trigger.
         */
        private void OnTriggerExit(Collider collider) {
            if (collider.gameObject.CompareTag("Monster")) {
                if (onTriggerExit != null) {
                    onTriggerExit.Invoke(collider);
                }
            }
        }
    }
}
