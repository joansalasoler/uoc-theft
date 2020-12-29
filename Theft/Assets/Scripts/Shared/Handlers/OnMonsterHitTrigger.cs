using System;
using UnityEngine;

namespace Game.Shared {

    /**
     * Delegate trigger events for collisions with monsters.
     */
    [RequireComponent(typeof(Collider))]
    public class OnMonsterHitTrigger: MonoBehaviour {

        /** Controller for this actor */
        ActorController actor = null;


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

                    Vector3 origin = collider.gameObject.transform.position;
                    Vector3 target = actor.transform.position;
                    Vector3 direction = (origin - target);

                    Rigidbody body = actor.GetComponent<Rigidbody>();
                    body.isKinematic = false;
                    body.AddForce(700f * direction.normalized);
                }
            }
        }
    }
}
