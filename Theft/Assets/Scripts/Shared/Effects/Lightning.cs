using UnityEngine;
using System.Collections;

namespace Game.Shared {

    /**
     * Switches a light at random intervals to simulate lightnings.
     */
    public class Lightning : MonoBehaviour {

        /** Light used to illuminate the scene on a lightning */
        private Light supportLight = null;


        /**
         * Initialization.
         */
        private void OnEnable() {
            supportLight = GetComponent<Light>();
            StartCoroutine(ThrowLightnings());
        }


        /**
         * Toggle the light on and off while this component is enabled.
         */
        private IEnumerator ThrowLightnings() {
            while (enabled) {
                supportLight.enabled = false;
                yield return new WaitForSeconds(Random.Range(2.0f, 6.0f));
                supportLight.enabled = true;
                supportLight.intensity = Random.Range(1.0f, 1.8f);
                yield return new WaitForSeconds(Random.Range(0.1f, 0.3f));
            }
        }
    }
}
