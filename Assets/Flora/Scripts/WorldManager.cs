using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Flora.Scripts {
    
    public class WorldManager : MonoBehaviour {
        public int width, height;

        public GameObject floor;
        public GameObject left;
        public GameObject right;
        public GameObject top;
        public GameObject bottom;
        public GameObject cornerTopLeft;
        public GameObject cornerTopRight;
        public GameObject cornerBottomLeft;
        public GameObject cornerBottomRight;
        private List<GameObject> allocatedPieces;

        public void Generate() {
            if (allocatedPieces != null) {
                foreach (var allocatedPiece in allocatedPieces) {
#if UNITY_EDITOR

                    if (!EditorApplication.isPlaying) {
                        DestroyImmediate(allocatedPiece);
                        continue;
                    }
#endif
                    Destroy(allocatedPiece);
                }
            }

            allocatedPieces = new List<GameObject>();

            for (var x = -width; x <= width; x++) {
                for (var z = -height; z <= height; z++) {
                    var obj = Instantiate(BuildPiece(x, z), new Vector3(x, 0, z), Quaternion.Euler(-90, 0, 0), transform);
                    allocatedPieces.Add(obj);
                }
            }
        }

        private GameObject BuildPiece(int x, int z) {
            if (x == -width) {
                if (z == -height) {
                    return cornerBottomLeft;
                }
                if (z == height) {
                    return cornerTopLeft;
                }
                return left;
            }
            if (x == width) {
                if (z == -height) {
                    return cornerBottomRight;

                }
                if (z == height) {
                    return cornerTopRight;
                }
                return right;
            }
            if (z == -height) {
                return bottom;

            }
            if (z == height) {
                return top;
            }
            return floor;
        }
    }
#if UNITY_EDITOR
    [CustomEditor(typeof(WorldManager))]
    public class WorldManagerEditor : Editor {
        public override void OnInspectorGUI() {
            base.OnInspectorGUI();
            if (GUILayout.Button("Generate")) {
                ((WorldManager) target).Generate();
            }
        }
    }
#endif
}