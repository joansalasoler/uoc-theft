using System;
using UnityEngine;

namespace Game.Shared {

    /**
     * Base actor state that does nothing.
     */
    [Serializable]
    public class BaseState : ActorState {

        /** Invoked on state enter */
        public virtual void OnStateEnter(ActorController actor) {}

        /** Invoked on state exit */
        public virtual void OnStateExit(ActorController actor) {}

        /** Invoked each frame update */
        public virtual void OnUpdate(ActorController actor) {}

        /** Invoked on each physics update */
        public virtual void OnFixedUpdate(ActorController actor) {}

        /** Trigger enter event on the actor */
        public virtual void OnTriggerEnter(ActorController actor, Collider collider) {}

        /** Trigger stay event on the actor */
        public virtual void OnTriggerStay(ActorController actor, Collider collider) {}

        /** Trigger leave event on the actor */
        public virtual void OnTriggerExit(ActorController actor, Collider collider) {}
    }
}
