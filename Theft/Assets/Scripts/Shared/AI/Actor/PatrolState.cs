using System;
using UnityEngine;
using UnityEngine.AI;

namespace Game.Shared {

    /**
     * A actor is moving around a path.
     */
    [Serializable, RequireComponent(typeof(NavMeshAgent))]
    public class PatrolState : BaseState {

        /** Current waypoint we are moving towards */
        public Waypoint waypoint = null;

        /** Navigation agent of the actor */
        private NavMeshAgent agent = null;


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
         * State activation handler.
         */
        public override void OnStateEnter(ActorController actor) {
            agent = actor.GetComponent<NavMeshAgent>();
            MoveTowards(waypoint);
        }


        /**
         * State deactivation handler.
         */
        public override void OnStateExit(ActorController actor) {
            StopMoving();
        }


        /**
         * Move to the next waypoint when a target is reached.
         */
        public override void OnUpdate(ActorController actor) {
            if (actor.isAlive && agent.enabled && IsAtWaypoint()) {
                waypoint = waypoint.Next();
                MoveTowards(waypoint);
            }
        }
    }
}
