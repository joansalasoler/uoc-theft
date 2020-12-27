using System;
using System.Collections;
using UnityEngine;

namespace Game.Shared {

    /**
     * Stop a car from moving when an actor is in front.
     */
    public class OnCarTrigger: MonoBehaviour {

        /** Controller for the car */
        public AutoController controller = null;

        /** Layer mask to check for collisions */
        public int layerMask = LayerMask.GetMask("Car Dome");

        /** Radius of the sphere to check */
        public float radius = 3f;

        /** If triggers must be checked for collisions */
        private QueryTriggerInteraction hitTriggers = null;

        /** If the car was stopped */
        private bool isStopped = false;


        /**
         * Initialization.
         */
        private void Start() {
            hitTriggers = QueryTriggerInteraction.Collide;
            StartCoroutine(CheckTraffic());
        }


        /**
         * Checks for traffic periodically and starts/stops the car
         * accordingly.
         */
        private IEnumerator CheckTraffic() {
            while (enabled) {
                if (HasTrafficInFront()) {
                    if (!isStopped) {
                        isStopped = true;
                        controller.Break();
                    }
                } else if (isStopped) {
                    isStopped = false;
                    controller.Resume();
                }

                yield return new WaitForSeconds(0.2f);
            }
        }


        /**
         * Check if an actor is in front of the car.
         */
        private bool HasTrafficInFront() {
            Collider[] colliders = Physics.OverlapSphere(
                transform.position, radius, layerMask, hitTriggers);

            return colliders.Length > 0;
        }
    }
}
