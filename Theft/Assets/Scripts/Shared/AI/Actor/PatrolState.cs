using System;
using System.Collections;
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
                actor.StartCoroutine(Break());
            }
        }


        /**
         * Makes the actor continue moving.
         */
        public void ResumeMoving(ActorController actor) {
            if (agent.enabled && agent.isStopped) {
                agent.isStopped = false;
                MoveTowards(waypoint);
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
            StopMoving(actor);
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


        /**
         * Slowly stops the agent from moving.
         */
        private IEnumerator Break() {
            float elapsedTime = 0f;

            while (agent.isStopped && elapsedTime < 2f) {
                elapsedTime += Time.deltaTime;
                agent.velocity = Vector3.Lerp(agent.velocity, Vector3.zero, elapsedTime / 2f);
                yield return null;
            }

            yield return null;
        }
    }
}
