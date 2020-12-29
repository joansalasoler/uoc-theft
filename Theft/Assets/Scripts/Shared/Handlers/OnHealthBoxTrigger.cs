using System;
using UnityEngine;

namespace Game.Shared {

    /**
     * Handles collisions with health box rewards.
     */
    public class OnHealthBoxTrigger: MonoBehaviour {

        /**
         * Fill the player's health reserve if empty.
         */
        private void OnTriggerEnter(Collider collider) {
            if (collider.gameObject.CompareTag("Player Grab")) {
                PlayerController player = GetPlayerController(collider);

                if (player.status.IncreaseHealth()) {
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
            return collider.transform.parent.GetComponent<PlayerController>();
        }
    }
}
