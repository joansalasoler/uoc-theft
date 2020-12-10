using System;
using UnityEngine;

namespace Game.Shared {

    /**
     * A waypoint a navigation agent may follow.
     */
    public class Waypoint : MonoBehaviour {

        /** Paths where this waypoint belongs */
        public Waypath[] paths = null;


        /**
         * Check if this waypoint is a path intersection.
         */
        public bool IsIntersection() {
            return paths.Length > 0;
        }


        /**
         * Index of the last path on a direction.
         */
        private int GetEndPathIndex(Direction direction) {
            return (direction == Direction.Forward) ? paths.Length - 1 : 0;
        }


        /**
         * Obtain the next waypath to follow.
         */
        public Waypath NextPath(Direction direction, Waypath path) {
            int currentIndex = Array.IndexOf(paths, path);
            int nextIndex = (currentIndex + (int) direction) % paths.Length;
            int firstIndex = GetEndPathIndex((Direction) (-((int) direction)));

            return paths[nextIndex < 0 ? firstIndex : nextIndex];
        }


        /**
         * Obtain a random waypath to follow.
         */
        public Waypath RandomPath() {
            return paths[UnityEngine.Random.Range(0, paths.Length)];
        }
    }
}
