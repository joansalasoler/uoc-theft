using UnityEngine;
using UnityEngine.AI;

namespace Game.Shared {

    /**
     * Synchronize the agent movement with the animation.
     *
     * This component uses two animation blend trees to make the agent follow
     * the animation and thus prevent foot-sliding of the zombie. Works well
     * in ensuring the zombie is always on the agent's path but causes the
     * zombies to get stuck for a while when trying to avoid each other.
     *
     * Inspired by a solution found on the Unity manual:
     * https://docs.unity3d.com/Manual/nav-CouplingAnimationAndNavigation.html
     */
    [RequireComponent(typeof(Animator))]
    [RequireComponent(typeof(NavMeshAgent))]
    public class ZombieLocomotion: MonoBehaviour {

        /** Maximum speed of the walking animation */
        public float maximumWalkSpeed = 0.54f;

        /** Maximum rotation angle while walking */
        public float turnAngle = 65.0f;

        /** Seconds of the rotation animation */
        public float turnTime = 3.2f;

        /** Humanoid character animator */
        private Animator animator = null;

        /** Navigation agent reference */
        private NavMeshAgent agent = null;

        /** Velocity at which the agent is moving */
        private Vector3 velocity = Vector3.zero;

        /** Time to wait for the rotation animation to finish */
        private float restingTime = 0.0f;


        /**
         * Initialization.
         */
        private void Start() {
            animator = GetComponent<Animator>();
            agent = GetComponent<NavMeshAgent>();

            agent.updateRotation = true;
            agent.updatePosition = false;
            NavMesh.avoidancePredictionTime = 5.0f;
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

            // Wait in the spot while a rotation animation is in progress

            if (restingTime > 0) {
                agent.nextPosition = transform.position;
                restingTime -= Time.deltaTime;
                return;
            }

            // Update the speed of the walk animation when time passes

            Vector2 target = GetTargetVelocity();
            float angle = Vector2.SignedAngle(velocity, target);

            if (Time.deltaTime > float.Epsilon) {
                target = Vector2.ClampMagnitude(target, maximumWalkSpeed);
                velocity = Vector2.Lerp(velocity, target, Time.deltaTime);
            }

            // Ensure the agent keeps following the animation

            if (velocity.magnitude > float.Epsilon) {
                agent.nextPosition = transform.position;
            }

            // To prevent foot sliding trigger the turn animation when the
            // velocity change is too big. Otherwise, update the speed of
            // the walking animation's blend tree.

            if (Mathf.Abs(angle) >= turnAngle) {
                animator.SetFloat("TurnAngle", angle);
                animator.SetTrigger("Turn");
                restingTime = turnTime;
            } else {
                animator.SetBool("Walk", velocity.magnitude > 0.1f);
                animator.SetFloat("VelocityX", velocity.x);
                animator.SetFloat("VelocityY", velocity.y);
            }
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
