using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;

namespace Game.Shared {

    /**
     * An actor is chasing a moving point. Invokes the 'targetReached'
     * action on each update while the actor is on the target point and
     * the 'targetLost' action when no longer at the waypoint.
     */
    [Serializable, RequireComponent(typeof(NavMeshAgent))]
    public class ChaseState : BaseState {

        /** Invoken when the waypoint is reached */
        public Action targetReached = null;

        /** Invoken when a reached waypoint is lost */
        public Action targetLost = null;

        /** Current waypoint we are moving towards */
        public Waypoint waypoint = null;

        /** Wether to automatically chase the player */
        public bool followPlayer = true;

        /** If the target was reached */
        private bool wasReached = false;

        /** Navigation agent of the actor */
        private NavMeshAgent agent = null;

        /** Corroutine used to update the agent's destination */
        private IEnumerator updateCoroutine = null;


        /**
         * Checks if the actor reached the waypoint.
         */
        private bool IsAtWaypoint() {
            bool isPending = agent.pathPending;
            bool isAtPoint = agent.remainingDistance <= agent.stoppingDistance;

            return isAtPoint && !isPending;
        }


        /**
         * Makes the actor move torwards a point.
         */
        private void MoveTowards(Waypoint waypoint) {
            if (agent.enabled) {
                agent.SetDestination(waypoint.transform.position);
                agent.isStopped = false;
                this.waypoint = waypoint;
            }
        }


        /**
         * Makes the actor stop from moving.
         */
        private void StopMoving() {
            if (agent.enabled) {
                agent.isStopped = true;
            }
        }


        /**
         * Waypoint towards which the actor must move.
         */
        private Waypoint GetWaypointToChase() {
            return followPlayer ? GetPlayerWaypoint() : waypoint;
        }


        /**
         * Waypoint attached to the player's object.
         */
        private Waypoint GetPlayerWaypoint() {
            GameObject player = GameObject.FindWithTag("Player");
            Waypoint waypoint = player.GetComponentInChildren<Waypoint>();

            return waypoint;
        }


        /**
         * State activation handler.
         */
        public override void OnStateEnter(ActorController actor) {
            agent = actor.GetComponent<NavMeshAgent>();
            waypoint = GetWaypointToChase();
            updateCoroutine = UpdateDestination(actor);
            actor.StartCoroutine(updateCoroutine);
        }


        /**
         * State deactivation handler.
         */
        public override void OnStateExit(ActorController actor) {
            actor.StopCoroutine(updateCoroutine);
            StopMoving();
        }


        /**
         * Move to the next waypoint when a target is reached. Notice that
         * this method may also change the path or the direction.
         */
        public override void OnUpdate(ActorController actor) {
            if (agent.enabled && IsAtWaypoint()) {
                if (targetReached != null) {
                    targetReached.Invoke();
                    wasReached = true;
                }
            } else if (wasReached) {
                targetLost.Invoke();
                wasReached = false;
            }
        }


        /**
         * Updates the destination position for the actor.
         */
        private IEnumerator UpdateDestination(ActorController actor) {
            while (actor.isAlive && agent.enabled) {
                if (waypoint != null) MoveTowards(waypoint);
                yield return new WaitForSeconds(1.0f);
            }
        }
    }
}
