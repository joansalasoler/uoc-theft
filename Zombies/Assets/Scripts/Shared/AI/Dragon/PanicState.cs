using UnityEngine;
using System.Collections;

namespace Game.Shared {

    /**
     * A actor is in panic.
     */
    public class PanicState : BaseState {

        /**
         * Invoked when this state is activated.
         */
        public override void OnStateEnter(ActorController actor) {
            DragonController dragon = (DragonController) actor;

            AudioService.PlayOneShot(actor.gameObject, "Monster Panic");
            AudioService.PlayLoop(actor.gameObject, "Monster Walk");
            Vector3 origin = actor.transform.position;
            Vector3 direction = actor.transform.forward;
            Vector3 target = -25.0f * direction + origin;

            dragon.MoveTowards(target);
            dragon.animator.SetTrigger("panic");
            dragon.navigator.speed *= 2.0f;
            actor.StartCoroutine(ExitPanicState(dragon));
        }


        /**
         * Invoked when this state is deactivated.
         */
        public override void OnStateExit(ActorController actor) {
            DragonController dragon = (DragonController) actor;
            AudioService.StopLoop(actor.gameObject);
            dragon.navigator.speed *= 0.5f;
        }


        /**
         * Moves back to the patrol state after 5 seconds.
         */
        private IEnumerator ExitPanicState(DragonController dragon) {
            yield return new WaitForSeconds(5.0f);
            dragon.SetDragonState(dragon.PATROL);
        }
    }
}
