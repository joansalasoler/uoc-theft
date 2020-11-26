using System;
using UnityEngine;
using UnityEngine.AI;

namespace Game.Shared {

    /**
     * Controller for zombies.
     */
    [RequireComponent(typeof(Animator))]
    public class ZombieController : ActorController {

        /** Delegate triggered when the player is near the zombie */
        [SerializeField] private OnPlayerTrigger PlayerListener = null;

        /** Minimum delay between attacks to the player */
        [SerializeField, Range(1, 20)] private int healthPoints = 1;

        /** Minimum delay between attacks to the player */
        [SerializeField, Range(0.5f, 10.0f)] private float attackDelay = 2.5f;

        /** If the zombie start state is patroling */
        [SerializeField] private bool patrolOnStart = false;

        /** Navigating to a concrete target */
        [HideInInspector] public BaseState IdleState = new BaseState();

        /** Navigating to a concrete target */
        public ChaseState ChaseState = new ChaseState();

        /** Something killed the zombie */
        public DieState DieState = new DieState();

        /** Navigating from on waypoint to another */
        public PatrolState PatrolState = new PatrolState();

        /** Handlers for the colliders tha damage the player */
        private OnPlayerDamageCollision[] damagers = null;

        /** Actor animator reference */
        private Animator animator = null;

        /** Last time an attack was triggered */
        private float lastAttackTime = 0.0f;

        /** Type of the last attack */
        private int attackType = 0;


        /**
         * Activate a random attack animation.
         */
        public void AnimateAttack() {
            attackType = UnityEngine.Random.Range(0, 4);
            animator.SetInteger("AttackType", attackType);
            animator.SetTrigger("Attack");
        }


        /**
         * Set the enabled status of the damaging colliders.
         */
        private void SetDamagersEnabled(bool enabled) {
            foreach (var damager in damagers) {
                damager.enabled = enabled;
            }
        }


        /**
         * Initialization.
         */
        private void Start() {
            animator = GetComponent<Animator>();
            damagers = GetComponentsInChildren<OnPlayerDamageCollision>();

            PlayerListener.onTriggerEnter += OnPlayerTriggerEnter;
            PlayerListener.onTriggerExit += OnPlayerTriggerExit;
            ChaseState.targetReached += OnChaseTargetReached;
            ChaseState.targetLost += OnChaseTargetLost;

            SetState(patrolOnStart ? PatrolState : IdleState);
            SetDamagersEnabled(false);
        }


        /**
         * Cause damage to this actor.
         */
        public override void Damage(Vector3 point) {
            if (healthPoints <= 1) {
                this.Kill();
            } else if (isAlive) {
                healthPoints -= 1;
                animator.SetFloat("DamageX", point.x);
                animator.SetFloat("DamageZ", point.z);
                animator.SetTrigger("Damage");
            }
        }


        /**
         * Kills this actor.
         */
        public override void Kill() {
            if (isAlive) {
                base.Kill();
                SetState(DieState);
                animator.SetTrigger("Die");
                SetDamagersEnabled(false);
            }
        }


        /**
         * This zombie reached the player.
         */
        private void OnChaseTargetReached() {
            if (isAlive && attackDelay < Time.time - lastAttackTime) {
                lastAttackTime = Time.time;
                SetDamagersEnabled(true);
                AnimateAttack();
            }
        }


        /**
         * The player is out of the zombie's reach.
         */
        private void OnChaseTargetLost() {
            SetDamagersEnabled(false);
        }


        /**
         * A player entered this zombie's action radius.
         */
        private void OnPlayerTriggerEnter(Collider collider) {
            if (isAlive) SetState(ChaseState);
        }


        /**
         * A player left this zombie's action radius.
         */
        private void OnPlayerTriggerExit(Collider collider) {
            if (isAlive) SetState(PatrolState);
        }
    }
}
