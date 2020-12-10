using System;
using UnityEngine;

namespace Game.Shared {

    /**
     * Makes the ocean move to simulate waves.
     */
    public class Ocean : MonoBehaviour {

        /** Maximum position on the Y axis */
        public float topPosition = 1.24f;

        /** Minimum position on the Y axis */
        public float bottomPosition = 1.14f;

        /** Base increment on the Y axis */
        private float verticalStep = 0.02f;

        /** Base increment on the X axis */
        private float horizontalStep = -40.0f;

        /** Current move direction */
        private float direction = 0.02f;


        /**
         * Initialization.
         */
        private void Start() {
            AudioService.PlayLoop(gameObject, "Ocean");
        }


        /**
         * Moves the ocean to create a wave effect.
         */
        private void FixedUpdate() {
            Vector3 position = transform.position;

            if (position.y >= topPosition) {
                direction = -verticalStep;
            } else if (position.y <= bottomPosition) {
                direction = 2.0f * verticalStep;
            }

            position.y += direction * Time.fixedDeltaTime;
            position.x += horizontalStep * direction * Time.fixedDeltaTime;
            position.z += horizontalStep * direction * Time.fixedDeltaTime;

            transform.position = position;
        }
    }
}
