using System;
using UnityEngine;

namespace Game.Shared {

    /**
     * Delegate triggers for the player's field of view.
     */
    [RequireComponent(typeof(Collider))]
    public class OnFieldOfViewTrigger: MonoBehaviour {

        /** Invoked when an object enters the trigger */
        public static Action<Collider> onTriggerEnter = null;


        /**
         * Invoke the delegate when an object enters this trigger.
         */
        private void OnTriggerEnter(Collider collider) {
            if (onTriggerEnter != null) {
                onTriggerEnter.Invoke(collider);
            }
        }
    }
}
