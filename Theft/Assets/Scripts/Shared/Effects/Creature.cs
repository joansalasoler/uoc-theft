using UnityEngine;
using System.Collections;

namespace Game.Shared {

    /**
     * Plays a creature sound at random intervals.
     */
    public class Creature : MonoBehaviour {

        /** Minimum seconds between plays */
        public float minSeconds = 10.0f;

        /** Maximum seconds between plays */
        public float maxSeconds = 30.0f;

        /** Audio source reference */
        private AudioSource audioSource = null;


        /**
         * Initialization.
         */
        private void OnEnable() {
            audioSource = GetComponent<AudioSource>();
            StartCoroutine(PlaySounds());
        }


        /**
         * Plays an audio clip at random intervals.
         */
        private IEnumerator PlaySounds() {
            while (enabled) {
                float waitTime = Random.Range(minSeconds, maxSeconds);
                yield return new WaitForSeconds(waitTime);
                audioSource.PlayOneShot(audioSource.clip);
            }
        }
    }
}
