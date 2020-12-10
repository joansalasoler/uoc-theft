using System;
using UnityEngine;

namespace Game.Shared {

    /**
     * Plays music when the game starts.
     */
    public class OnGameStart: MonoBehaviour {

        /**
         * Start the music.
         */
        private void Start() {
            AudioService.PlayLoop(gameObject, "Game Music");
        }


        /**
         * Stop the music.
         */
        private void OnDisable() {
            AudioService.StopLoop(gameObject);
        }
    }
}
