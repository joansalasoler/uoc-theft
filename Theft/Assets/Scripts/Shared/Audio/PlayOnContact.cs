using UnityEngine;
using System.Collections;

namespace Game.Shared {

    /**
     * Plays an audio clip when the player collides with an object.
     */
    public class PlayOnContact : MonoBehaviour {

        /** Audio source to play */
        private AudioSource audioSource = null;


        /**
         * Initialization.
         */
        private void OnEnable() {
            audioSource = GetComponent<AudioSource>();
        }


        /**
         * Play the sound.
         */
        private void OnCollisionEnter(Collision collision) {
            if (collision.collider.gameObject.CompareTag("Player")) {
                audioSource.PlayOneShot(audioSource.clip);
            }
        }
    }
}
