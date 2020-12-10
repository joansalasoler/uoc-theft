using System;
using UnityEngine;

namespace Game.Shared {

    /**
     * Base controller for gift boxes.
     */
    public class GiftboxController : MonoBehaviour {

        /**
         * Constantly rotate the box around its vertical.
         */
        private void Update() {
            transform.Rotate(0, 90.0f * Time.deltaTime, 0, Space.Self);
        }
    }
}
