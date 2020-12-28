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

        /** Delegate triggered when the pedestrian is near a zombie */
        [SerializeField] private OnMonsterTrigger MonsterListener = null;

        /** Clip to play when the actor is damaged */
        [SerializeField] private AudioClip damageClip = null;

        /** If the actor start state is patroling */
        [SerializeField] private bool patrolOnStart = false;

        /** Navigating to a concrete target */
        [HideInInspector] public BaseState IdleState = new BaseState();

        /** Something killed the actor */
        public DieState DieState = new DieState();

        /** Running to a random waypoint */
        public PanicState PanicState = new PanicState();

        /** Navigating from on waypoint to another */
        public PatrolState PatrolState = new PatrolState();

        /** Actor animator reference */
        private Animator animator = null;


        /**
         * Initialization.
         */
        private void Start() {
            animator = GetComponent<Animator>();

            PanicState.safepointReached += OnSafePointReached;
            MonsterListener.onTriggerEnter += OnMonsterTriggerEnter;
            SetInitialState();
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
        public override void Damage(Vector3 point) {
            this.Kill();
        }


        /**
         * Kills this actor.
         */
        public override void Kill() {
            base.Kill();
            SetState(DieState);
            AudioService.PlayClip(gameObject, damageClip);
            animator.SetTrigger("Die");
        }


        /**
         * Sets the initial state of the actor.
         */
        private void SetInitialState() {
            SetState(IdleState);

            if (patrolOnStart) {
                StartCoroutine(StartPatroling());
            }
        }


        /**
         * A zombie entered this pedestrian's sight radius.
         */
        private void OnMonsterTriggerEnter(Collider collider) {
            if (isAlive) {
                animator.SetBool("Run", true);
                SetState(PanicState);
            }
        }


        /**
         * Reset the state when the actor reaches a safe point.
         */
        private void OnSafePointReached(Waypoint waypoint) {
            if (isAlive) {
                animator.SetBool("Run", false);
                PatrolState.waypoint = waypoint;
                SetInitialState();
            }
        }
    }
}
