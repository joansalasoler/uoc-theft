using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;

namespace Game.Shared {

    /**
     * When actor is death disables the navigation agent and animator
     * and then activates the physics on its rigidbodies.
     */
    [Serializable]
    public class DieState : BaseState {

        /** Delay before the ragdoll is activated in seconds */
        public float ragdollDelay = 3.5f;


        /**
         * State activation handler.
         */
        public override void OnStateEnter(ActorController actor) {
            DisableNavMeshAgent(actor);
            actor.StartCoroutine(EnableRagdoll(actor));
        }


        /**
         * Disable the navmesh agent if it exists.
         */
        private void DisableNavMeshAgent(ActorController actor) {
            var agent = actor.GetComponentInChildren<NavMeshAgent>();
            if (agent != null) agent.enabled = false;
        }


        /**
         * Disable the animator agent if it exists.
         */
        private void DisableAnimator(ActorController actor) {
            var animator = actor.GetComponentInChildren<Animator>();
            if (animator != null) animator.enabled = false;
        }


        /**
         * Disable the animator and set the rigidbodies as non-kinematic
         * after the configured ragdoll delay time.
         */
        private IEnumerator EnableRagdoll(ActorController actor) {
            yield return new WaitForSeconds(ragdollDelay);

            DisableAnimator(actor);

            foreach (var body in actor.GetComponentsInChildren<Rigidbody>()) {
                body.isKinematic = false;
            }
        }
    }
}
