using UnityEngine;
using UnityEngine.AI;

namespace Game.Shared {

    /**
     * Synchronize the agent movement with the animation.
     */
    [RequireComponent(typeof(Animator))]
    [RequireComponent(typeof(NavMeshAgent))]
    public class PedestrianLocomotion: MonoBehaviour {

        /** Maximum speed of the walking animation */
        public float maximumSpeed = 3.8f;

        /** Humanoid character animator */
        private Animator animator = null;

        /** Navigation agent reference */
        private NavMeshAgent agent = null;

        /** Velocity at which the agent is moving */
        private Vector3 velocity = Vector3.zero;


        /**
         * Initialization.
         */
        private void Start() {
            animator = GetComponent<Animator>();
            agent = GetComponent<NavMeshAgent>();

            agent.updateRotation = true;
            agent.updatePosition = false;
        }


        /**
         * Synchronize the walking animation with the agent velocity.
         */
        private void Update() {
            // Stop walking if the agent is not moving

            if (agent.enabled && agent.isStopped) {
                animator.SetBool("Walk", false);
                return;
            }

            // Update the speed of the walk animation when time passes

            Vector2 target = GetTargetVelocity();

            if (Time.deltaTime > float.Epsilon) {
                target = Vector2.ClampMagnitude(target, maximumSpeed);
                velocity = Vector2.Lerp(velocity, target, Time.deltaTime);
            }

            // Ensure the agent keeps following the animation

            if (velocity.magnitude > float.Epsilon) {
                agent.nextPosition = transform.position;
            }

            animator.SetBool("Walk", velocity.magnitude > 1.0f);
            animator.SetFloat("VelocityX", velocity.x);
            animator.SetFloat("VelocityY", velocity.y);
        }


        /**
         * Compute the target velocity of the animation.
         */
        private Vector2 GetTargetVelocity() {
            float dx = Vector3.Dot(transform.right, agent.velocity);
            float dy = Vector3.Dot(transform.forward, agent.velocity);

            return new Vector2(dx, Mathf.Max(0, dy));
        }


        /**
         * Snap the agent movement to the animation.
         */
        private void OnAnimatorMove() {
            Quaternion rotation = animator.rootRotation;
            transform.rotation = rotation;

            Vector3 position = animator.rootPosition;
            position.y = agent.nextPosition.y;
            transform.position = position;
        }
    }
}
