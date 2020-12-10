using UnityEngine;

namespace Game.Shared {

    /**
     * Actor state machine interface.
     */
    public interface ActorState {

        /** Invoked on state enter */
        void OnStateEnter(ActorController actor);

        /** Invoked on state exit */
        void OnStateExit(ActorController actor);

        /** Invoked each frame update */
        void OnUpdate(ActorController actor);

        /** Invoked on each physics update */
        void OnFixedUpdate(ActorController actor);

        /** Trigger enter event on the actor */
        void OnTriggerEnter(ActorController actor, Collider collider);

        /** Trigger stay event on the actor */
        void OnTriggerStay(ActorController actor, Collider collider);

        /** Trigger leave event on the actor */
        void OnTriggerExit(ActorController actor, Collider collider);
    }
}
