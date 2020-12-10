using System;
using UnityEngine;

namespace Game.Shared {

    /**
     * A playable audio event.
     */
    [Serializable]
    public class AudioEvent {

        /** Event name */
        public string name;

        /** Clip to play */
        public AudioClip clip;
    }
}
