using System;
using UnityEngine;

namespace Game.Shared {

    /**
     * Delegate animation events of the player shootings.
     */
    [RequireComponent(typeof(Animator))]
    public class OnWeaponListener: MonoBehaviour {

        /** Invoked when the player shots a weapon */
        public static Action onShotAction = null;


        /**
         * Invoked on the shot frame of the shooting animation.
         */
        private void OnWeaponShot() {
            if (onShotAction != null) {
                onShotAction.Invoke();
            }
        }
    }
}
