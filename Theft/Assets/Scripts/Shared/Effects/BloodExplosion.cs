using UnityEngine;

namespace Game.Shared {

    /**
     * Controller for blood explosions.
     */
    public class BloodExplosion : MonoBehaviour {

        /**
         * Autodestroy it after a predefined delay.
         */
        private void OnEnable() {
            Destroy(gameObject, 2.0f);
        }
    }
}
