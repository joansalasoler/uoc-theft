using UnityEngine;
using System.Collections;

namespace Game.Shared {

    /**
     * A actor is patroling a path.
     */
    public class DragonPatrolState : BaseState {

        /** Current waypoint direction to follow */
        private Direction direction = Direction.Forward;

        /** Current path we are moving on */
        private Waypath waypath = null;

        /** Current waypoint we are moving towards */
        private Waypoint waypoint = null;

        /** Animation loop */
        private IEnumerator animation = null;

        /** Maximum seconds the actor can stay without moving */
        private float maxStuckTime = 5.0f;

        /** Last time the monser's waypoint was changed */
        private float lastChangeTime = 0.0f;


        /**
         * Invoked when this state is activated.
         */
        public override void OnStateEnter(ActorController actor) {
            DragonController dragon = (DragonController) actor;

            waypath = waypath ?? dragon.waypath;
            waypoint = waypath.ClosestPoint(actor.transform.position);
            dragon.MoveTowards(waypoint.transform.position);
            lastChangeTime = Time.time;

            animation = AnimatePatrol(dragon);
            actor.StartCoroutine(animation);
        }


        /**
         * Invoked when this state is deactivated.
         */
        public override void OnStateExit(ActorController actor) {
            actor.StopCoroutine(animation);
        }


        /**
         * Move to the next waypoint when a target is reached.
         */
        public override void OnUpdate(ActorController actor) {
            if (actor.isAlive == false) {
                return;
            }

            DragonController dragon = (DragonController) actor;

            if (IsStuckWithoutMoving(dragon)) {
                direction = (Direction) (-((int) direction));
            } else if (!dragon.IsAtWaypoint()) {
                return;
            }

            if (waypoint.IsIntersection()) {
                waypath = waypoint.RandomPath();
            }

            if (!waypath.HasNextPoint(direction, waypoint)) {
                direction = (Direction) (-((int) direction));
            }

            waypoint = waypath.NextPoint(direction, waypoint);
            dragon.MoveTowards(waypoint.transform.position);
            lastChangeTime = Time.time;
        }


        /**
         * Check if the actor has been stuck without moving.
         */
        private bool IsStuckWithoutMoving(DragonController dragon) {
            bool isTimeElapsed = (Time.time - lastChangeTime >= maxStuckTime);
            bool isSlowedDown = dragon.navigator.velocity.magnitude < 0.5f;
            bool isPending = dragon.navigator.pathPending;

            return isTimeElapsed && !isPending && isSlowedDown;
        }


        /**
         * Plays animations randomly while the actor walks.
         */
        private IEnumerator AnimatePatrol(DragonController dragon) {
            while (dragon.gameObject.activeSelf) {
                yield return new WaitForSeconds(Random.Range(5.0f, 20.0f));
                dragon.animator.SetTrigger("search");
            }
        }
    }
}
