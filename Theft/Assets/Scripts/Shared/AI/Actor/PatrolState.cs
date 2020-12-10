using System;
using UnityEngine;
using UnityEngine.AI;

namespace Game.Shared {

    /**
     * A actor is moving around a path.
     */
    [Serializable, RequireComponent(typeof(NavMeshAgent))]
    public class PatrolState : BaseState {

        /** Current waypoint direction to follow */
        public Direction direction = Direction.Forward;

        /** Current path we are moving on */
        public Waypath waypath = null;

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
         * Waypoint towards which the actor must move or the closest to
         * the actor if no current waypoint is set.
         */
        private Waypoint GetStartWaypoint(ActorController actor) {
            return waypoint ?? waypath.ClosestPoint(actor.transform.position);
        }


        /**
         * State activation handler.
         */
        public override void OnStateEnter(ActorController actor) {
            agent = actor.GetComponent<NavMeshAgent>();
            MoveTowards(GetStartWaypoint(actor));
        }


        /**
         * State deactivation handler.
         */
        public override void OnStateExit(ActorController actor) {
            StopMoving();
        }


        /**
         * Move to the next waypoint when a target is reached. Notice that
         * this method may also change the path or the direction.
         */
        public override void OnUpdate(ActorController actor) {
            if (actor.isAlive && agent.enabled && IsAtWaypoint()) {
                if (waypoint.IsIntersection()) {
                    waypath = waypoint.RandomPath();
                }

                if (!waypath.HasNextPoint(direction, waypoint)) {
                    direction = (Direction) (-((int) direction));
                }

                MoveTowards(waypath.NextPoint(direction, waypoint));
            }
        }
    }
}
