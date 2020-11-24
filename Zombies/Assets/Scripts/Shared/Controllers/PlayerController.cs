using System;
using System.Collections;
using UnityEngine;
using UnityStandardAssets.Characters.ThirdPerson;

namespace Game.Shared {

    /**
     * Controller for player characters.
     */
    public class PlayerController : ActorController {

        /** Weapon controller instance */
        [SerializeField] private WeaponController weaponController = null;

        /** Animator for the player's character */
        private Animator animator = null;

        /** Invoked when the player is damaged */
        public Action<PlayerController> playerDamaged;

        /** Invoked when the player is killed */
        public Action<PlayerController> playerKilled;

        /** Current player status */
        public PlayerStatus status = null;


        /**
         * Initialization.
         */
        private void Start() {
            animator = GetComponent<Animator>();
            weaponController.onImpact += OnShotImpact;
        }


        /**
         * Handles the player input.
         */
        private void Update() {
            if (isAlive && Input.GetButtonUp("Fire1")) {
                animator.SetTrigger("Fire");
            }
        }


        /**
         * Invoked on the weapon animation on the shot frame.
         */
        private void OnWeaponShot() {
            if (!weaponController.CanShootWeapon()) {
                return;
            }

            if (!status.HasMunition()) {
                weaponController.Click();
                return;
            }

            Vector3 origin = transform.position;
            Vector3 direction = transform.forward;

            weaponController.Shoot(origin, direction);
            status.DecreaseMunition();
        }


        /**
         * Cause damage to this player.
         */
        public override void Damage() {
            if (isAlive == false) {
                return;
            }

            if (!status.DamagePlayer()) {
                this.Kill();
                return;
            }

            AudioService.PlayOneShot(gameObject, "Damage Player");

            if (playerDamaged != null) {
                playerDamaged.Invoke(this);
            }
        }


        /**
         * Kills this player.
         */
        public override void Kill() {
            if (isAlive == false) {
                return;
            }

            base.Kill();
            DisableCharacter();
            animator.SetTrigger("Die");
            AudioService.PlayOneShot(gameObject, "Player Die");

            if (playerKilled != null) {
                playerKilled.Invoke(this);
            }
        }


        /**
         * Disables the character and user controllers.
         */
        private void DisableCharacter() {
            GetComponent<ThirdPersonUserControl>().enabled = false;
            GetComponent<ThirdPersonCharacter>().enabled = false;
        }


        /**
         * Damage the dragons when a shot impacts them.
         */
        public void OnShotImpact(RaycastHit hit) {
            if (hit.collider.CompareTag("Monster")) {
                var actor = hit.collider.GetComponent<ActorController>();
                actor.Damage();
            }
        }
    }
}
