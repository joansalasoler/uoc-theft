using System;
using UnityEngine;

namespace Game.Shared {

    /**
     * Handles collisions with munition box rewards.
     */
    public class OnMunitionBoxTrigger: MonoBehaviour {

        /**
         * Fill the player's munition reserve if empty.
         */
        private void OnTriggerEnter(Collider collider) {
            if (collider.gameObject.CompareTag("Player")) {
                PlayerController player = GetPlayerController(collider);

                if (player.status.RefillMunition()) {
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
