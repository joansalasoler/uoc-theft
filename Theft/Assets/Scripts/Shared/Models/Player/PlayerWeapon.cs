using System;
using System.ComponentModel;
using UnityEngine;

namespace Game.Shared {

    /**
     * Properties of a player weapon.
     */
    [CreateAssetMenu]
    public class PlayerWeapon : ScriptableObject {

        /** Weapon game object */
        public GameObject weaponPrefab = null;

        /** Weapon impact prefab */
        public GameObject impactPrefab = null;

        /** Weapon blood impact prefab */
        public GameObject bloodPrefab = null;

        /** Audio clip to play when the weapon is shot */
        public AudioClip shotSound = null;

        /** Audio clip to play when the weapon has no bullets */
        public AudioClip clickSound = null;

        /** Vertical local position from which to shut */
        public float shootHeight = 1.0f;

        /** Maximum distance reached by a shoot */
        [Range(1.0f, 100.0f)] public float shootDistance = 10.0f;

        /** Seconds between each shoot */
        [Range(0.35f, 10.0f)] public float rateOfFire = 1.0f;

        /** Deviation of the weapon */
        [Range(0.0f, 1.0f)] public float deviation = 1.0f;
    }
}
