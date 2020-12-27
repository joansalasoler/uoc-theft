using UnityEditor;
using UnityEngine;
using System.Collections.Generic;

namespace Game.Shared {

    /**
     * Encapsulates a list of waypoints.
     */
    public class WaypointList : MonoBehaviour {

        /** Initialize waypoints from child objects */
        public bool collectFromChildren = true;

        /** List of waypoints */
        public Waypoint[] wayPoints = null;


        /**
         * Initialization.
         */
        private void Start() {
            if (collectFromChildren) {
                wayPoints = GetComponentsInChildren<Waypoint>();
            }
        }


        /**
         * Obtains a random point on the list.
         */
        public Waypoint NextRandom() {
            return wayPoints[Random.Range(0, wayPoints.Length)];
        }
    }
}
