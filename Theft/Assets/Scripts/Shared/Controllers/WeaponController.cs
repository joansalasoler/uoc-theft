using System;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Shared {

    /**
     * Controller for the player weapons. Instantiates the set of available
     * weapons and allows toggling between them.
     */
    public class WeaponController : MonoBehaviour {

        /** Invoked when the shot impacts an object */
        public Action<RaycastHit> onImpact = null;

        /** Hand of the character */
        [SerializeField] private Transform hand = null;

        /** Layers affected by player shoots */
        [SerializeField] private LayerMask shootLayers = Physics.DefaultRaycastLayers;

        /** Layers that bleed when impacted by shoots */
        [SerializeField] private LayerMask bleedingLayers = Physics.DefaultRaycastLayers;

        /** Layers that receive an impact when they are hit */
        [SerializeField] private LayerMask impactLayers = Physics.DefaultRaycastLayers;

        /** Force of the bullets impacts on objects */
        [SerializeField] private float impactForce = 50.0f;

        /** Available weapons for the player */
        [SerializeField] private List<PlayerWeapon> weapons = null;

        /** Active weapon or null while rearming */
        [HideInInspector] public PlayerWeapon activeWeapon = null;

        /** If shots must hit triggers */
        [HideInInspector] public QueryTriggerInteraction hitTriggers;

        /** Weapon game object instances */
        private List<GameObject> instances = null;

        /** Weapon animator instance */
        private Animator animator = null;

        /** Last time the weapon was shot */
        private float lastShotTime = 0.0f;

        /** Active weapon index */
        private int activeIndex = -1;


        /**
         * Switch to the first weapon.
         */
        private void Start() {
            transform.parent = hand;
            ToggleWeapon();
        }


        /**
         * Instantiate the weapons.
         */
        private void Awake() {
            hitTriggers = QueryTriggerInteraction.Collide;
            instances = new List<GameObject>();

            foreach (PlayerWeapon weapon in weapons) {
                GameObject prefab = weapon.weaponPrefab;
                instances.Add(Instantiate(prefab, transform));
            }
        }


        /**
         * Check if a weapon is currently active.
         */
        public bool HasActiveWeapon() {
            return activeWeapon != null;
        }


        /**
         * Checks if the weapon can be shot.
         */
        public bool CanShootWeapon() {
            return HasActiveWeapon() && IsInsideShotRate();
        }


        /**
         * Checks if the rate of fire period elapsed.
         */
        public bool IsInsideShotRate() {
            float rateOfFire = activeWeapon.rateOfFire;
            float timeSinceShot = Time.time - lastShotTime;
            bool inShotRate = rateOfFire < timeSinceShot;

            return inShotRate;
        }


        /**
         * Vertical position of the shot.
         */
        public Vector3 GetShootHeight(PlayerWeapon weapon) {
            return weapon.shootHeight * Vector3.up;
        }


        /**
         * Deviation direction of the shot.
         */
        public Vector3 GetShootDeviation(PlayerWeapon weapon) {
            Vector3 random = UnityEngine.Random.insideUnitCircle.normalized;
            Vector3 deviation = weapon.deviation * random;

            return deviation;
        }


        /**
         * Aram a weapon given its index.
         */
        private void ArmWeapon(int index = 0) {
            lastShotTime = 0.0f;
            animator = instances[index].GetComponentInChildren<Animator>();
            instances[index].SetActive(true);
            activeWeapon = weapons[index];
            activeIndex = index;
        }


        /**
         * Disarm the current weapon.
         */
        private void DisarmWeapon() {
            instances[activeIndex].SetActive(false);
            activeWeapon = null;
            activeIndex = -1;
        }


        /**
         * Switch to the next weapon.
         */
        public void ToggleWeapon() {
            int nextIndex = (1 + activeIndex) % instances.Count;

            if (activeWeapon != null) {
                DisarmWeapon();
            }

            ArmWeapon(nextIndex);
        }


        /**
         * Makes a click sound and resets the shot time.
         */
        public void Click() {
            AudioService.PlayClip(gameObject, activeWeapon.clickSound);
            lastShotTime = Time.time;
        }


        /**
         * Shoots the active weapon on the given direction.
         */
        public bool Shoot(Vector3 position, Vector3 direction) {
            if (CanShootWeapon() == false) {
                return false;
            }

            AudioService.PlayClip(gameObject, activeWeapon.shotSound);
            animator.SetTrigger("Fire");
            lastShotTime = Time.time;

            RaycastHit hit;
            RaycastHit head;

            if (RaycastShot(position, direction, out hit) == false) {
                return true;
            }

            int layer = hit.collider.gameObject.layer;

            if (onImpact != null) {
                onImpact.Invoke(hit);
            }

            // Embed a gunshot on the impact layers

            if (impactLayers == (impactLayers | (1 << layer))) {
                PushShotCollider(hit);
                EmbedImpact(hit);
            }

            // Embed a blood explosion on the bleeding layers. A new
            // ray is cast because we want to aim for the head.

            if (bleedingLayers == (bleedingLayers | (1 << layer))) {
                bool isHeadShot = RaycastShot(position, direction, out head, 0.5f);

                if (isHeadShot && head.collider == hit.collider) {
                    EmbedBloodExplosion(head);
                } else {
                    EmbedBloodExplosion(hit);
                }
            }

            return true;
        }


        /**
         * Raycast a shot for the current weapon. The last parameter allows
         * incrementing the weapon shot height on a given percentage.
         */
        private bool RaycastShot(Vector3 position, Vector3 direction, out RaycastHit hit, float increment = 0.0f) {
            Vector3 height = GetShootHeight(activeWeapon);
            Vector3 deviation = GetShootDeviation(activeWeapon);
            Vector3 origin = position + height + deviation + increment * height;

            Ray ray = new Ray(origin, direction);
            float distance = activeWeapon.shootDistance;
            Debug.DrawRay(origin, distance * direction, Color.red);

            return Physics.Raycast(ray, out hit, distance, shootLayers, hitTriggers);
        }


        /**
         * Embed an impact decal into a hit position.
         */
        private void EmbedImpact(RaycastHit hit) {
            Vector3 position = 0.01f * hit.normal + hit.point;
            Quaternion rotation = Quaternion.FromToRotation(Vector3.forward, -hit.normal);
            GameObject prefab = activeWeapon.impactPrefab;
            GameObject decal = Instantiate(prefab, position, rotation, hit.transform);

            Vector3 s = hit.transform.lossyScale;
            decal.transform.localScale = Vector3.one * (1f / s.x);
        }


        /**
         * Embed an impact decal into a hit position.
         */
        private void EmbedBloodExplosion(RaycastHit hit) {
            GameObject prefab = activeWeapon.bloodPrefab;
            Instantiate(prefab, hit.point, hit.collider.transform.rotation);
        }


        /**
         * Push a moveable object if it was shot.
         */
        private void PushShotCollider(RaycastHit hit) {
            Rigidbody body = hit.collider.attachedRigidbody;

            if (body != null) {
                Vector3 force = impactForce * Vector3.one;
                body.AddForceAtPosition(force, hit.point);
            }
        }
    }
}
