using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;

namespace Game.Shared {

    /**
     * Controller for pedestrians.
     */
    [RequireComponent(typeof(Animator))]
    [RequireComponent(typeof(NavMeshAgent))]
    public class PedestrianController : ActorController {

        /** If the actor start state is patroling */
        [SerializeField] private bool patrolOnStart = false;

        /** Navigating to a concrete target */
        [HideInInspector] public BaseState IdleState = new BaseState();

        /** Navigating from on waypoint to another */
        public PatrolState PatrolState = new PatrolState();

        /** Actor animator reference */
        private Animator animator = null;


        /**
         * Initialization.
         */
        private void Start() {
            animator = GetComponent<Animator>();

            SetState(IdleState);

            if (patrolOnStart) {
                StartCoroutine(StartPatroling());
            }
        }


        /**
         * Start patroling after a random delay if the actor is idle.
         */
        private IEnumerator StartPatroling() {
            float delay = UnityEngine.Random.Range(0.5f, 5.0f);
            yield return new WaitForSeconds(delay);
            if (state == IdleState) SetState(PatrolState);
        }


        /**
         * Cause damage to this actor.
         */
        public override void Damage(Vector3 point) {}


        /**
         * Kills this actor.
         */
        public override void Kill() {}
    }
}
