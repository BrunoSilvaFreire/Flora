using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Flora.Scripts.Obstacles {
    [Serializable]
    public abstract class Activation {
        public bool block;
        public float delay;

        public abstract IEnumerator Activate(GameManager gameManager, float speedMultiplier, HashSet<Obstacle> blacklist);
    }

    [Serializable]
    public class RandomizedActivation : Activation {
        public int min, max;
        public ObstacleType type;

        public override IEnumerator Activate(GameManager gameManager, float speedMultiplier, HashSet<Obstacle> blacklist) {
            var numToActivate = Random.Range(min, max + 1);
            var remaining = numToActivate;

            var obstacles = gameManager.GetAllObstaclesOfType(type);

            int i;

            while (obstacles.Count > 0 && remaining > 0) {
                i = Random.Range(0, obstacles.Count);

                var obstacle = obstacles[i];
                if (blacklist.Contains(obstacle)) {
                    continue;
                }

                if (!obstacle.TryActivate(speedMultiplier)) {
                    continue;
                }

                obstacles.RemoveAt(i);
                blacklist.Add(obstacle);
                remaining--;
            }
            yield break;
        }
    }

    [Serializable]
    public class SproutActivation : Activation {
        [Serializable]
        public struct SproutOffset {
            public Vector2Int offset;
            public float delay;
        }

        public List<SproutOffset> sproutOffsets;

        public override IEnumerator Activate(GameManager gameManager, float speedMultiplier, HashSet<Obstacle> blacklist) {
            var worldManager = gameManager.worldManager;

            var origin = new Vector2Int(
                Random.Range(-worldManager.width, worldManager.width),
                Random.Range(-worldManager.height, worldManager.height)
            );

            foreach (var sproutOffset in sproutOffsets) {
                if (sproutOffset.delay > 0) {
                    yield return new WaitForSeconds(sproutOffset.delay * speedMultiplier);
                }

                var sprout = gameManager.worldManager.GetSproutAtLocation(origin + sproutOffset.offset);

                if (blacklist.Contains(sprout)) {
                    continue;
                }

                if (sprout == null) {
                    continue;
                }

                if (sprout.TryActivate(speedMultiplier)) {
                    blacklist.Add(sprout);
                }
            }
        }

        private Vector2Int GetSize() {
            Vector2Int size = Vector2Int.zero;
            foreach (var sproutOffset in sproutOffsets) {
                var offsetX = Mathf.Abs(sproutOffset.offset.x);
                var offsetY = Mathf.Abs(sproutOffset.offset.y);

                if (offsetX > size.x) {
                    size.x = offsetX;
                }

                if (offsetY > size.y) {
                    size.y = offsetY;
                }
            }
            return size;
        }
    }

    public enum LogLocation {
        Top,
        Bottom,
        Left,
        Right
    }

    [Serializable]
    public class LogActivation : Activation {

        public int logLength;
        public bool randomLocation;

        [HideIf(nameof(randomLocation))]
        public LogLocation location;

        public override IEnumerator Activate(GameManager gameManager, float speedMultiplier, HashSet<Obstacle> blacklist) {

            var length = GetLengthForLog(gameManager);

            LogLocation tgtLocation;
            if (randomLocation) {
                tgtLocation = (LogLocation) Random.Range(0, 4);
            } else {
                tgtLocation = location;
            }
            var spawner = gameManager.GetAllObstaclesOfType(ObstacleType.Log)
                .Cast<LogSpawner>()
                .First(obstacle => obstacle.Location == tgtLocation);

            spawner.CurrentSize = length;

            if (spawner.TryActivate(speedMultiplier)) {
                blacklist.Add(spawner);
            }

            yield break;
        }
        private int GetLengthForLog(GameManager gameManager) {
            int length;
            if (logLength == 0) {
                switch (location) {

                    case LogLocation.Top:
                    case LogLocation.Bottom:
                        length = gameManager.worldManager.height;
                        break;
                    case LogLocation.Left:
                    case LogLocation.Right:
                        length = gameManager.worldManager.width;
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            } else {
                length = logLength;
            }
            return length;
        }
    }


    [CreateAssetMenu(menuName = "Flora/ObstacleEvent")]
    public class ObstacleEvent : ScriptableObject {

        public float minTimeToActivate, maxTimeToActivate;

        [SerializeReference]
        public List<Activation> activations;

        public IEnumerator Activate(GameManager gameManager, float speedMultiplier) {
            var blacklist = new HashSet<Obstacle>();

            foreach (var activation in activations) {
                var block = activation.Activate(gameManager, speedMultiplier, blacklist);
                if (activation.block) {
                    yield return block;
                } else {
                    gameManager.StartCoroutine(block);
                }
            }
        }
    }
}