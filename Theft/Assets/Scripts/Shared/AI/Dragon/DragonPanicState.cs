using UnityEngine;
using System.Collections;

namespace Game.Shared {

    /**
     * A actor is in panic.
     */
    public class DragonPanicState : BaseState {

        /**
         * Invoked when this state is activated.
         */
        public override void OnStateEnter(ActorController actor) {
            DragonController dragon = (DragonController) actor;

            AudioService.PlayOneShot(actor.gameObject, "Monster Panic");
            Vector3 origin = actor.transform.position;
            Vector3 direction = actor.transform.forward;
            Vector3 target = -40.0f * direction + origin;

            dragon.MoveTowards(target);
            dragon.animator.SetTrigger("panic");
            dragon.navigator.speed *= 2.0f;
            actor.StartCoroutine(ExitDragonPanicState(dragon));
        }


        /**
         * Invoked when this state is deactivated.
         */
        public override void OnStateExit(ActorController actor) {
            DragonController dragon = (DragonController) actor;
            dragon.navigator.speed *= 0.5f;
        }


        /**
         * Moves back to the patrol state after 5 seconds.
         */
        private IEnumerator ExitDragonPanicState(DragonController dragon) {
            yield return new WaitForSeconds(10.0f);
            dragon.SetDragonState(dragon.PATROL);
        }
    }
}
