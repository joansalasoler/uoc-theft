using UnityEngine;
using System.Collections;

namespace Game.Shared {

    /**
     * Simulates the sun rotation on the scene by rotating a light on
     * the X axis and darkening the fog over time.
     */
    public class Sunlight : MonoBehaviour {

        /** Duration of the sun rotation in seconds */
        public float duration = 600.0f;

        /** Target angle of sun */
        public float targetAngle = 2.0f;

        /** Color gradient for the fog */
        public Gradient fogGradient = null;

        /** Start angle of the rotation */
        private float startAngle = 35.0f;

        /** Elapsed time in seconds */
        private float elapsedTime = 0.0f;

        /** Light used to illuminate the scene */
        private Light sunLight = null;


        /**
         * Initialization.
         */
        private void OnEnable() {
            sunLight = GetComponent<Light>();
            startAngle = sunLight.transform.localRotation.eulerAngles.x;
        }


        /**
         * Rotates the sun until it reaches its final angle.
         */
        private void Update() {
            Transform transform = sunLight.transform;
            Vector3 eulerAngles = transform.localRotation.eulerAngles;

            if(eulerAngles.x > targetAngle) {
                float angle = (targetAngle - startAngle) / duration;
                transform.Rotate(angle * Time.deltaTime, 0, 0);

                float colorStep = elapsedTime / duration;
                float colorValue = Mathf.Lerp(0.0f, 1.0f, colorStep);
                Color color = fogGradient.Evaluate(colorValue);
                RenderSettings.fogColor = color;

                elapsedTime += Time.deltaTime;
            }
        }
    }
}
