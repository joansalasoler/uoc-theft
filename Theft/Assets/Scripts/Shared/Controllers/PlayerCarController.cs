using System;
using UnityEngine;
using UnityEngine.AI;
using UnityStandardAssets.Vehicles.Car;

namespace Game.Shared {

    /**
     * Controller for player cars.
     */
    public class PlayerCarController : ActorController {

        /** Player that can drive the car */
        public PlayerController driver = null;

        /** Transform of the driving player character */
        public Transform character = null;

        /** Camera of the driver object */
        public GameObject driverCamera = null;

        /** Camera of the car object */
        public GameObject carCamera = null;

        /** True when the car is active */
        private bool isActive = false;

        /** NavMesh areas were the character can be spawned */
        private int areaMask = NavMesh.AllAreas;


        /**
         * Initialization.
         */
        private void Start() {
            areaMask = 1 << NavMesh.GetAreaFromName("Walkable");
        }


        /**
         * Handles the player input.
         */
        protected override void Update() {
            if (driver.isAlive && Input.GetButtonUp("Fire2")) {
                if (isActive || IsDriverNear()) {
                    this.isActive = !this.isActive;
                    SetCarActive(isActive);
                }
            }
        }


        /**
         * Disables the character and user controllers.
         */
        private void SetCarActive(bool active) {
            if (active && !driver.gameObject.activeSelf) {
                Debug.Log("Player is inside a car");
                return;
            }

            if (active == false && !SpawnDriverPlayer()) {
                Debug.Log("Car door is blocked");
                return;
            }

            gameObject.tag = active ? "Player" : "Untagged";

            driver.gameObject.SetActive(active == false);
            driverCamera.gameObject.SetActive(active == false);
            carCamera.gameObject.SetActive(active == true);

            GetComponent<CarController>().enabled = active;
            GetComponent<CarUserControl>().enabled = active;
            GetComponent<CarAudio>().enabled = active;

            Rigidbody body = GetComponent<Rigidbody>();
            body.isKinematic = (active == false);
            body.velocity = Vector3.zero;

            foreach (var audio in GetComponentsInChildren<AudioSource>()) {
                if (active) { audio.UnPause(); } else { audio.Pause(); }
            }
        }


        /**
         * Sample a navmesh position near the car door and spawn the
         * player there. Return false if the player cannot be spawned.
         */
        private bool SpawnDriverPlayer() {
            Vector3 target = transform.position - (4f * transform.right);
            Vector3 position = GetSpawnPosition(target);

            if (position == Vector3.zero) {
                return false;
            }

            character.position = position;
            character.rotation = transform.rotation;

            return true;
        }


        /**
         * Obtains a position where the driver can be spawned.
         */
        private Vector3 GetSpawnPosition(Vector3 target) {
            NavMeshHit hit;

            if (NavMesh.SamplePosition(target, out hit, 2f, areaMask)) {
                return hit.position;
            }

            return Vector3.zero;
        }


        /**
         * Checks if the driver is near this car.
         */
        private bool IsDriverNear() {
            float distance = Vector3.Distance(transform.position, character.position);
            return  distance < 4.5f;
        }
    }
}
