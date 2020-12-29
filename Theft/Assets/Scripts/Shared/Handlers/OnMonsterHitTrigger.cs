using System;
using UnityEngine;

namespace Game.Shared {

    /**
     * Delegate trigger events for collisions with monsters.
     */
    [RequireComponent(typeof(Collider))]
    public class OnMonsterHitTrigger: MonoBehaviour {

        /** Actor's blood template */
        [SerializeField] private GameObject bloodPrefab = null;

        /** Controller for this actor */
        private ActorController actor = null;


        /**
         * Initialization-
         */
        private void Awake() {
            actor = GetComponent<ActorController>();
        }


        /**
         * Invoke the delegate on entering a player trigger.
         */
        private void OnTriggerEnter(Collider collider) {
            if (collider.gameObject.CompareTag("Car Body")) {
                if (actor.isAlive) {
                    actor.Kill();
                    EmbedBloodExplosion(actor.transform.position);

                    Vector3 origin = collider.gameObject.transform.position;
                    Vector3 target = actor.transform.position;
                    Vector3 direction = (origin - target);

                    Rigidbody body = actor.GetComponent<Rigidbody>();
                    body.isKinematic = false;
                    body.AddForce(700f * direction.normalized);
                }
            }
        }


        /**
         * Embed an impact decal into a hit position.
         */
        private void EmbedBloodExplosion(Vector3 point) {
            Instantiate(bloodPrefab, point, actor.transform.rotation);
        }
    }
}
