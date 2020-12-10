using UnityEngine;
using UnityEngine.AI;

namespace Game.Shared {

    /**
     * A actor was killed.
     */
    public class DragonDieState : BaseState {

        /**
         * Invoked when this state is activated.
         */
        public override void OnStateEnter(ActorController actor) {
            DragonController dragon = (DragonController) actor;

            dragon.StopMoving();
            AudioService.PlayOneShot(actor.gameObject, "Monster Die");
            dragon.animator.SetTrigger("die");
            dragon.navigator.enabled = false;
            EnableMonsterPhysics(dragon);
            EnableNavMeshObstacle(dragon);
        }


        /**
         * Enable the navmesh obstacle if it exists.
         */
        private void EnableNavMeshObstacle(ActorController actor) {
            var obstacle = actor.GetComponentInChildren<NavMeshObstacle>();
            if (obstacle != null) obstacle.enabled = true;
        }


        /**
         * Enables the physics of the actor when dead.
         */
        private void EnableMonsterPhysics(DragonController dragon) {
            CapsuleCollider collider = dragon.GetComponent<CapsuleCollider>();
            Rigidbody body = dragon.GetComponent<Rigidbody>();
            Vector3 center = collider.center;

            collider.center = new Vector3(-0.17f, 0.38f, -0.35f);
            body.isKinematic = false;
        }
    }
}
