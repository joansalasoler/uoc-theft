using UnityEngine;
using System.Collections;

namespace Game.Shared {

    /**
     * Simulates a streetlight that turns on after a delay.
     */
    public class Streetlight : MonoBehaviour {

        /** Delay after wich the light will turn on */
        public float delay = 510.0f;

        /** Material for when the light is off */
        public Material offMaterial = null;

        /** Material for when the light is on */
        public Material onMaterial = null;

        /** Mesh renderer of the object */
        private MeshRenderer meshRenderer = null;

        /** Spot light of the object */
        private Light spotLight = null;


        /**
         * Initialization.
         */
        private void OnEnable() {
            meshRenderer = GetComponentInChildren<MeshRenderer>();
            spotLight = GetComponentInChildren<Light>();
            StartCoroutine(TurnOnLight());
        }


        /**
         * Turn on the light.
         */
        private IEnumerator TurnOnLight() {
            yield return new WaitForSeconds(delay);

            spotLight.enabled = true;
            meshRenderer.material = onMaterial;
            yield return new WaitForSeconds(Random.Range(0.1f, 0.3f));

            spotLight.enabled = false;
            meshRenderer.material = offMaterial;
            yield return new WaitForSeconds(Random.Range(0.1f, 0.3f));

            spotLight.enabled = true;
            meshRenderer.material = onMaterial;
        }
    }
}
