using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;

namespace Game.Shared {

    /**
     * Controller for cars.
     */
    [RequireComponent(typeof(NavMeshAgent))]
    public class AutoController : ActorController {

        /** If the car start state is patroling */
        [SerializeField] private bool patrolOnStart = false;

        /** Navigating to a concrete target */
        [HideInInspector] public BaseState IdleState = new BaseState();

        /** Navigating from on waypoint to another */
        public PatrolState PatrolState = new PatrolState();


        /**
         * Initialization.
         */
        private void Start() {
            SetState(IdleState);

            if (patrolOnStart) {
                StartCoroutine(StartPatroling());
            }
        }


        /**
         * Start patroling after a random delay if the car is idle.
         */
        private IEnumerator StartPatroling() {
            float delay = UnityEngine.Random.Range(0.5f, 5.0f);
            yield return new WaitForSeconds(delay);
            if (state == IdleState) SetState(PatrolState);
        }


        /**
         * Stops the car if it was patroling.
         */
        public void Break() {
            if (state == PatrolState) {
                PatrolState.StopMoving(this);
            }
        }


        /**
         * Resumes the car movement if it was patroling.
         */
        public void Resume() {
            if (state == PatrolState) {
                PatrolState.ResumeMoving(this);
            }
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
