using System;
using UnityEngine;
using UnityEditor;
using UnityEngine.Tilemaps;

namespace Game.Editor {

    /**
     *
     */
    public class CreateTiles : MonoBehaviour {

        [MenuItem("GameObject/3D Object/CreateTiles")]
        public static void Create() {
            GameObject group = new GameObject("Sidewalks");

            var o = GameObject.FindGameObjectsWithTag("Tilemap")[0];
            var t = o.GetComponent<Tilemap>();
            var g = o.transform.parent.GetComponentInParent<GridLayout>();

            int count = 1;

            for (int y = t.cellBounds.yMin; y < t.cellBounds.yMax; y++) {
                for (int x = t.cellBounds.xMin; x < t.cellBounds.xMax; x++) {
                    var cell = new Vector3Int(x, y, 0);
                    var n = t.GetTile(cell);

                    if (n != null && n.name == "Placeholder") {
                        Debug.Log(n);
                        Debug.Log(g.CellToWorld(cell));
                        var p = AssetDatabase.LoadAssetAtPath("Assets/Prefabs/Elements/Sideway.prefab", typeof(GameObject));
                        var i = (GameObject) PrefabUtility.InstantiatePrefab(p);
                        i.name = $"Sideway ({count})";
                        count++;
                        Vector3 position = g.CellToWorld(cell);
                        position.x += 3.5f;
                        position.z += 3.5f;
                        i.transform.position = position;
                        i.transform.SetParent(group.transform);
                    }
                }
            }
        }


        private static string GetTileName(Tilemap t, int x, int y) {
            var a = t.GetTile(new Vector3Int(x, y, 0));
            return a == null ? "" : a.name;
        }
    }
}
