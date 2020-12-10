using UnityEngine;

namespace Game.Shared {

    /**
     * A dragon is waiting.
     */
    public class DragonWaitState : BaseState {

        /**
         * Invoked when this state is activated.
         */
        public override void OnStateEnter(ActorController actor) {
            DragonController dragon = (DragonController) actor;
            dragon.SetDragonState(dragon.PATROL);
        }
    }
}
