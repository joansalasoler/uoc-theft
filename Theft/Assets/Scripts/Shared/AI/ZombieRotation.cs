using System;
using UnityEngine;
using UnityEngine.AI;

namespace Game.Shared {

    /**
     * Rotates the zombie character toward the terrain normal. This prevents
     * the character from being over or below the surface.
     */
    public class ZombieRotation: MonoBehaviour {

        /** Layers to raycast */
        public LayerMask terainMask;

        /** Terrain hit */
        private RaycastHit hit;


        /**
         * Gradually rotate the character towards the surface normal.
         */
        private void Update() {
            if (Physics.Raycast(transform.position, -transform.up, out hit, 4f, terainMask)) {
                Quaternion target = Quaternion.FromToRotation(transform.up, hit.normal);
                Quaternion rotation = target * transform.parent.rotation;
                transform.rotation = Quaternion.Lerp(transform.rotation, rotation, Time.deltaTime);
            }
        }
    }
}
