using UnityEngine;

namespace Game.Shared {

    /**
     * A actor was alerted by the player presence.
     */
    public class DragonAlertState : BaseState {

        /** Maximum seconds the dragon can be on this state */
        private float maximumTime = 5.0f;

        /** Time this state was entered */
        private float startTime = 0.0f;


        /**
         * Invoked when this state is activated.
         */
        public override void OnStateEnter(ActorController actor) {
            DragonController dragon = (DragonController) actor;

            startTime = Time.time;
            dragon.LookTowards(dragon.player.transform.position);
            dragon.StopMoving();
        }


        /**
         * Invoked when this state is deactivated.
         */
        public override void OnStateExit(ActorController actor) {
            DragonController dragon = (DragonController) actor;
            dragon.StopLooking();
        }


        /**
         * Search for a player and change to the attack state if found.
         */
        public override void OnUpdate(ActorController actor) {
            if (actor.isAlive == false) {
                return;
            }

            DragonController dragon = (DragonController) actor;
            bool timeIsElapsed = (Time.time - startTime) > maximumTime;

            if (timeIsElapsed || dragon.IsLookingAtTarget()) {
                bool playerOnSight = dragon.IsPlayerOnSight();
                dragon.StopLooking();

                if (playerOnSight) {
                    dragon.SetDragonState(dragon.PANIC);
                } else {
                    dragon.SetDragonState(dragon.PATROL);
                }
            }
        }
    }
}
