using System;
using UnityEngine;

namespace Game.Shared {

    /**
     * Handles collisions with shield box rewards.
     */
    public class OnShieldBoxTrigger: MonoBehaviour {

        /**
         * Fill the player's shield reserve if empty.
         */
        private void OnTriggerEnter(Collider collider) {
            if (collider.gameObject.CompareTag("Player")) {
                PlayerController player = GetPlayerController(collider);

                if (player.status.IncreaseShield()) {
                    AudioService.PlayOneShot(collider.gameObject, "Collect Reward");
                    GetComponentInChildren<Renderer>().enabled = false;
                    Destroy(gameObject, 0.5f);
                }
            }
        }


        /**
         * Obtain the player's controller from the collider.
         */
        private PlayerController GetPlayerController(Collider collider) {
            return collider.GetComponent<PlayerController>();
        }
    }
}
