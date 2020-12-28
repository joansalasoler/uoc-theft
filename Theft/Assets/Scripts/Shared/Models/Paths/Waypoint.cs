using UnityEditor;
using UnityEngine;
using System.Collections.Generic;

namespace Game.Shared {

    /**
     * A waypoint a navigation agent may follow.
     */
    [ExecuteInEditMode]
    public class Waypoint : MonoBehaviour {

        /** Color to draw the gizmos */
        public Color color = Color.green;

        /** Next points on the path */
        public List<Waypoint> children = new List<Waypoint>();


        /**
         * Obtain the next point to follow.
         */
        public Waypoint Next() {
            return children[Random.Range(0, children.Count)];
        }


#if UNITY_EDITOR

        /**
         * Set the parent when the object is duplicated.
         */
        [MenuItem("GameObject/3D Object/Create Child Waypoint #&d")]
        public static void DuplicateWaypoint() {
            GameObject o = Selection.activeTransform.gameObject;
            Waypoint parent = (o != null) ? o.GetComponent<Waypoint>() : null;

            if (parent is Waypoint) {
                GameObject prefab = PrefabUtility.GetCorrespondingObjectFromSource(o);
                GameObject clone = (GameObject) PrefabUtility.InstantiatePrefab(prefab);
                Waypoint child = clone.GetComponent<Waypoint>();

                parent.children.Add(child);
                child.color = parent.color;
                clone.transform.position = o.transform.position;
                Selection.activeGameObject = clone;
            }
        }


        /**
         * Obtain a waypoint given its instance identifier.
         */
        private Waypoint GetWaypointByID(int instanceID) {
            return (Waypoint) EditorUtility.InstanceIDToObject(instanceID);
        }


        /**
         * Draw a gizmo for the waypoint.
         */
        private void OnDrawGizmos() {
            Gizmos.color = color;
            Gizmos.DrawCube(transform.position, 2.5f * Vector3.one);

            if (children != null) {
                foreach (Waypoint child in children) {
                    if (child != null) {
                        Vector3 target = child.transform.position;
                        Gizmos.DrawLine(transform.position, target);
                        DrawDirection(transform.position, target);
                    }
                }
            }
        }


        /**
         * Draws an arrow indicating a direction between two points.
         */
        private void DrawDirection(Vector3 origin, Vector3 target) {
            if (origin == target) {
                return;
            }

            Vector3 middle = (target + origin) / 2f;
            Vector3 direction = (target - origin).normalized;
            Quaternion rotation = Quaternion.LookRotation(direction);

            Quaternion left = rotation * Quaternion.Euler(0, 150, 0);
            Quaternion right = rotation * Quaternion.Euler(0, 210, 0);
            Gizmos.DrawLine(middle, middle + left * Vector3.forward * 5f);
            Gizmos.DrawLine(middle, middle + right * Vector3.forward * 5f);
        }
#endif
    }
}
