using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;

namespace Game.Shared {

    /**
     * A actor is running to a random waypoint.
     */
    [Serializable, RequireComponent(typeof(NavMeshAgent))]
    public class PanicState : BaseState {

        /** Invoken when the agent reaches its waypoint */
        public Action<Waypoint> safepointReached = null;

        /** Waypoint were the actor may run */
        public WaypointList waypoints = null;

        /** Agent speed while panicing */
        public float panicSpeed = 3.8f;

        /** Agent speed befor this state was entered */
        private float previousSpeed = 0f;

        /** Current waypoint we are moving towards */
        private Waypoint waypoint = null;

        /** Navigation agent of the actor */
        private NavMeshAgent agent = null;


        /**
         * Checks if the actor reached the waypoint.
         */
        public bool IsAtWaypoint() {
            bool isPending = agent.pathPending;
            bool isAtPoint = agent.remainingDistance <= agent.stoppingDistance;

            return isAtPoint && !isPending;
        }


        /**
         * Makes the actor move torwards a point.
         */
        public void MoveTowards(Waypoint waypoint) {
            if (agent.enabled) {
                agent.SetDestination(waypoint.transform.position);
                agent.isStopped = false;
                this.waypoint = waypoint;
            }
        }


        /**
         * Makes the actor stop from moving.
         */
        public void StopMoving(ActorController actor) {
            if (agent.enabled && !agent.isStopped) {
                agent.isStopped = true;
            }
        }


        /**
         * State activation handler.
         */
        public override void OnStateEnter(ActorController actor) {
            agent = actor.GetComponent<NavMeshAgent>();
            previousSpeed = agent.speed;
            MoveTowards(GetRandomWaypoint(actor));
            UpdateAgentVelocity();
        }


        /**
         * Picks a random waypoint to run towards. Prefers waypoints that are
         * on the oposite direction the actor was moving towards.
         */
        private Waypoint GetRandomWaypoint(ActorController actor) {
            for (int i = 0; i < 20; i++) {
                Waypoint w = waypoints.NextRandom();
                Vector3 x = w.transform.position - actor.transform.position;
                float dot = Vector3.Dot(x.normalized, actor.transform.forward);
                float angle = Mathf.Acos(dot) * Mathf.Rad2Deg;

                if (angle > 90) return w;
            }

            return waypoints.NextRandom();
        }


        /**
         * State deactivation handler.
         */
        public override void OnStateExit(ActorController actor) {
            agent.speed = previousSpeed;
            StopMoving(actor);
        }


        /**
         * Move to the next waypoint when a target is reached.
         */
        public override void OnUpdate(ActorController actor) {
            if (actor.isAlive && agent.enabled && IsAtWaypoint()) {
                if (safepointReached != null) {
                    safepointReached.Invoke(waypoint);
                }
            }
        }


        /**
         * Makes the agent start running at its panic speed.
         */
        private void UpdateAgentVelocity() {
            agent.speed = panicSpeed;
            agent.velocity = panicSpeed * agent.velocity.normalized;
        }
    }
}
