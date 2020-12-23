using System;
using UnityEngine;
using UnityEngine.AI;

namespace Game.Shared {

    /**
     * A actor is wandering the scene.
     */
    [Serializable, RequireComponent(typeof(NavMeshAgent))]
    public class WanderState : BaseState {

        /** Wander radius */
        public float radius = 15f;

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
        private void MoveTowards(Vector3 position) {
            if (agent.enabled) {
                agent.SetDestination(position);
                agent.isStopped = false;
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
         * Obtains a random point on the scene.
         */
        private Vector3 GetRandomPoint() {
            NavMeshHit hit;

            Transform transform = agent.transform;
            Vector3 direction = UnityEngine.Random.insideUnitSphere * radius;
            Vector3 origin = radius * transform.forward.normalized;
            Vector3 target = origin + direction + transform.position;

            if (NavMesh.SamplePosition(target, out hit, radius, agent.areaMask)) {
                return hit.position;
            }

            return Vector3.zero;
        }


        /**
         * State activation handler.
         */
        public override void OnStateEnter(ActorController actor) {
            agent = actor.GetComponent<NavMeshAgent>();
            MoveTowards(agent.transform.position);
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
                Vector3 target = GetRandomPoint();

                if (target != Vector3.zero) {
                    MoveTowards(target);
                }
            }
        }
    }
}
