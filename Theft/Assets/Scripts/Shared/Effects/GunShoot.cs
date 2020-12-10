using UnityEngine;
﻿using System.Collections;
using System.Collections.Generic;

/**
 * Gunshot muzzle flash effect.
 */
public class GunShoot : MonoBehaviour {

    /** Location at which to show the gun shot */
    [SerializeField] private Transform barrel = null;

    /** Gun shot flash prefab */
    [SerializeField] private GameObject firePrefab = null;


    /**
     * Invoken when the shot animation starts.
     */
    void Shoot() {
        Vector3 position = barrel.position;
        Quaternion rotation = barrel.rotation;
        GameObject flash = Instantiate(firePrefab, position, rotation);
        Destroy(flash, 2.0f);
    }


    /**
     * Invoken after the shot animation.
     */
    private void CasingRelease() {
        // pass
    }
}
