using System;
using UnityEngine;

namespace Game.Shared {

    /**
     * Stop a car from moving when an actor is in front.
     */
    [RequireComponent(typeof(Collider))]
    public class OnCarTrigger: MonoBehaviour {

        /** Controller for the car */
        public AutoController controller = null;


        /**
         * Invoked on collision trigger.
         */
        private void OnTriggerEnter(Collider collider) {
            controller.Break();
        }


        /**
         * Invoked on collision trigger exit.
         */
        private void OnTriggerExit(Collider collider) {
            controller.Resume();
        }
    }
}
