using System;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Shared {

    /**
     * A collection of audio clips.
     */
    [CreateAssetMenu]
    public class AudioTheme : ScriptableObject {
        public AudioEvent[] events = {
            new AudioEvent { name = "Ocean" },
            new AudioEvent { name = "Weapon Shot" },
            new AudioEvent { name = "Weapon Impact" },
        };
    }
}
