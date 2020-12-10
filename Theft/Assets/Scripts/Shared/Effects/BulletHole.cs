using UnityEngine;

namespace Game.Shared {

    /**
     * Controller for weapon impacts.
     */
    public class BulletHole : MonoBehaviour {

        /**
         * Set a random scale/rotation and autodestroy it.
         */
        private void OnEnable() {
            transform.localScale = Vector3.one * Random.Range(0.1f, 0.2f);
            transform.Rotate(Vector3.forward * Random.Range(0.0f, 360.0f));
            Destroy(gameObject, 10.5f);
        }
    }
}
