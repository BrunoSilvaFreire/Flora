using System;
using System.Collections.Generic;
using Flora.Scripts.Obstacles;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Flora.Scripts {

    public class WorldManager : MonoBehaviour {
        public int width, height;
        public float flowerSpawnChance;

        public GameObject floor;
        public GameObject left;
        public GameObject right;
        public GameObject top;
        public GameObject bottom;
        public GameObject cornerTopLeft;
        public GameObject cornerTopRight;
        public GameObject cornerBottomLeft;
        public GameObject cornerBottomRight;

        public Sprout sproutPrefab;
        public LogSpawner logSpawnerPrefab;
        public BeeSpawner beeSpawnerPrefab;

        public float flowerHeightOffset;

        public GameObject flower;

        public bool Generated {
            get;
            private set;
        }

        private Dictionary<Vector2Int, Sprout> _sproutCache;

        private List<GameObject> _allocatedPieces;

        private void Awake() {
            Generated = false;
        }

        public void Clear() {
            if (_allocatedPieces != null) {
                foreach (var allocatedPiece in _allocatedPieces) {
#if UNITY_EDITOR
                    if (!EditorApplication.isPlaying) {
                        DestroyImmediate(allocatedPiece);
                        continue;
                    }
#endif
                    Destroy(allocatedPiece);
                }
            }
        }

        public void Generate(WorldPreset preset) {
            width = preset.width;
            height = preset.height;
        }

        public Sprout GetSproutAtLocation(Vector2Int position) {
            if (_sproutCache.TryGetValue(position, out var sprout)) {
                return sprout;
            }
            return null;
        }

        public void Generate() {
            Generated = true;
            Clear();

            _allocatedPieces = new List<GameObject>();
            _sproutCache = new Dictionary<Vector2Int, Sprout>();

            for (var x = -width; x <= width; x++) {
                for (var z = -height; z <= height; z++) {
                    var original = BuildPiece(x, z);
                    var position = new Vector3(x, 0, z);

                    var obj = Instantiate(original, position, Quaternion.identity, transform);
                    obj.name = $"{x}, {z}: {original.name}";
                    obj.hideFlags = HideFlags.DontSave | HideFlags.NotEditable;

                    _allocatedPieces.Add(obj);

                    HandleLogSpawner(x, z, obj);

                    if (IsCorner(x, z)) {
                        if (Random.value > flowerSpawnChance) {
                            SpawnFlower(x, z, obj);
                        }
                    } else {
                        SpawnSprout(position, obj, x, z);
                    }
                }
            }
        }

        private void SpawnSprout(Vector3 position, GameObject obj, int x, int z) {
            var sprout = Instantiate(sproutPrefab, position, Quaternion.identity, obj.transform);
            var posX = Mathf.FloorToInt(x);
            var posY = Mathf.FloorToInt(z);
            _sproutCache[new Vector2Int(posX, posY)] = sprout;
        }

        private void SpawnFlower(int x, int z, GameObject obj) {
            var rotation = Random.value * 360;
            var decoration = Instantiate(flower, new Vector3(x, flowerHeightOffset, z), Quaternion.Euler(0, rotation, 0), obj.transform);
            decoration.name = $"{x}, {z}: {flower.name}";
            decoration.hideFlags = HideFlags.DontSave | HideFlags.NotEditable;
            _allocatedPieces.Add(decoration);
        }

        private void HandleLogSpawner(int x, int z, GameObject obj) {
            if (x == 0 && Math.Abs(z) == height) {
                var axis = new Vector2 {
                    x = 0,
                    y = -Mathf.Sign(z)
                };
                
                var logSpawner = Instantiate(logSpawnerPrefab, new Vector3(0, 0, z), Quaternion.identity, obj.transform);
                logSpawner.MaxSize = width;
                logSpawner.MaxDistance = height;
                logSpawner.FacingDirection = axis;
                logSpawner.name = $"LogSpawner - {logSpawner.Location} - {axis}";

                var beeSpawner = Instantiate(beeSpawnerPrefab, new Vector3(0, 0, z), Quaternion.identity, obj.transform);
                beeSpawner.MaxDistance = height;
                beeSpawner.FacingDirection = axis;
                beeSpawner.name = $"BeeSpawner - {logSpawner.Location} - {axis}";

            } else if (z == 0 && Math.Abs(x) == width) {
                var axis = new Vector2 {
                    x = -Mathf.Sign(x),
                    y = 0
                };
                
                var logSpawner = Instantiate(logSpawnerPrefab, new Vector3(x, 0, 0), Quaternion.identity, obj.transform);
                logSpawner.MaxSize = height;
                logSpawner.MaxDistance = width;
                logSpawner.FacingDirection = axis;
                logSpawner.name = $"LogSpawner - {logSpawner.Location} - {axis}";
                
                
                
                var beeSpawner = Instantiate(beeSpawnerPrefab, new Vector3(x, 0, 0), Quaternion.identity, obj.transform);
                beeSpawner.MaxDistance = width;
                beeSpawner.FacingDirection = axis;
                beeSpawner.name = $"BeeSpawner - {logSpawner.Location} - {axis}";
            }
        }

        private bool IsCorner(int x, int z) {
            return x == -width || x == width || z == -height | z == height;
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
            using (new EditorGUILayout.HorizontalScope()) {
                if (GUILayout.Button("Generate")) {
                    ((WorldManager) target).Generate();
                }
                if (GUILayout.Button("Clear")) {
                    ((WorldManager) target).Clear();
                }
            }
        }
    }
#endif
}