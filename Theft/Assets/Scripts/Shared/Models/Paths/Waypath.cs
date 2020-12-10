using System;
using UnityEngine;

namespace Game.Shared {

    /**
     * A collection of way points an agent may follow.
     */
    public class Waypath : MonoBehaviour {

        /** Wether it is a closed path */
        [SerializeField] private bool isCircular = true;

        /** Wether the path points are chosen at random */
        [SerializeField] private bool isRandom = false;

        /** Waypoints that conform the path */
        [SerializeField] private Waypoint[] points = null;


        /**
         * Check if this path is circular.
         */
        public bool IsCircular() {
            return this.isCircular;
        }


        /**
         * Check if a point is the last on the path.
         */
        public bool HasNextPoint(Direction direction, Waypoint point) {
            return IsCircular() || !IsEndPoint(direction, point);
        }


        /**
         * Check is a waypoint is the last for a direction.
         */
        public bool IsEndPoint(Direction direction, Waypoint point) {
            int index = Array.IndexOf(points, point);
            bool isEndPoint = (index == GetEndPointIndex(direction));

            return isEndPoint;
        }


        /**
         * Index of the last point of the path on a direction.
         */
        private int GetEndPointIndex(Direction direction) {
            return (direction == Direction.Forward) ? points.Length - 1 : 0;
        }


        /**
         * Obtain the next waypoint to follow.
         */
        public Waypoint NextPoint(Direction direction, Waypoint point) {
            if (isRandom == true) {
                return GetNextRandomWaypoint(point);
            }

            int index = Array.IndexOf(points, point);
            int nextIndex = (index + (int) direction) % points.Length;
            int firstIndex = GetEndPointIndex((Direction) (-((int) direction)));

            return points[nextIndex < 0 ? firstIndex : nextIndex];
        }


        /**
         * Obtain the point closest to a position.
         */
        public Waypoint ClosestPoint(Vector3 position) {
            float distance = Mathf.Infinity;
            Waypoint closestPoint = points[0];

            foreach (Waypoint point in points) {
                Vector3 n = point.transform.position - position;

                if (n.sqrMagnitude < distance) {
                    distance = n.sqrMagnitude;
                    closestPoint = point;
                }
            }

            return closestPoint;
        }


        /**
         * Obtains another random point on the path.
         */
        public Waypoint GetNextRandomWaypoint(Waypoint point) {
            int index = UnityEngine.Random.Range(0, points.Length);

            while (points[index] == point && points.Length > 0) {
                index = UnityEngine.Random.Range(0, points.Length);
            }

            return points[index];
        }
    }
}
