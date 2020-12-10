using System;
using UnityEngine;

namespace Game.Shared {

    /**
     * Base controller for the actors.
     */
    [Serializable]
    public class ActorController : MonoBehaviour {

        /** Current state of the actor */
        protected ActorState state = new BaseState();

        /** Wether the actor is dead or alive */
        [HideInInspector] public bool isAlive = true;


        /**
         * Kills this actor.
         */
        public virtual void Kill() {
            isAlive = false;
        }


        /**
         * Damage this actor (kills it by default).
         */
        public virtual void Damage(Vector3 point) {
            Kill();
        }


        /**
         * Sets a new state for the actor.
         */
        protected virtual void SetState(ActorState state) {
            this.state.OnStateExit(this);
            this.state = state;
            this.state.OnStateEnter(this);
        }


        /**
         * Invoked on each frame update.
         */
        protected virtual void Update() {
            state.OnUpdate(this);
        }


        /**
         * Invoked on each physics update.
         */
        protected virtual void FixedUpdate() {
            state.OnFixedUpdate(this);
        }


        /**
         * An object entered this actor's action radius.
         */
        protected virtual void OnTriggerEnter(Collider collider) {
            state.OnTriggerEnter(this, collider);
        }


        /**
         * An object is on this actor's action radius.
         */
        protected virtual void OnTriggerStay(Collider collider) {
            state.OnTriggerStay(this, collider);
        }


        /**
         * An object left this actor's action radius.
         */
        protected virtual void OnTriggerExit(Collider collider) {
            state.OnTriggerExit(this, collider);
        }
    }
}
