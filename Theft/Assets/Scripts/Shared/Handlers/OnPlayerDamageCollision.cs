using System;
using UnityEngine;

namespace Game.Shared {

    /**
     * Damage the player when a collision happens.
     */
    [RequireComponent(typeof(Collider))]
    public class OnPlayerDamageCollision: MonoBehaviour {

        /** Player controller instance */
        private PlayerController player = null;


        /**
         * Initialization.
         */
        private void Start() {
            player = GetPlayerController();
        }


        /**
         * Damage the player when entering in contact.
         */
        private void OnCollisionEnter(Collision collision) {
            if (enabled && IsPlayerCollider(collision.collider)) {
                player.Damage(collision.GetContact(0).point);
            }
        }


        /**
         * Check if the given collider is the player.
         */
        private bool IsPlayerCollider(Collider collider) {
            return player.gameObject == collider.gameObject;
        }


        /**
         * Obtain the player controller from a tagged player.
         */
        private PlayerController GetPlayerController() {
            GameObject playerObject = GameObject.FindWithTag("Player");
            return playerObject.GetComponent<PlayerController>();
        }
    }
}
