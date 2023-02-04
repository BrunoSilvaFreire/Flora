using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Flora.Scripts.Obstacles {
    [Serializable]
    public struct RandomizedActivation {
        public int min, max;
        public ObstacleType type;

        public void Activate(GameManager gameManager, float speedMultiplier, HashSet<Obstacle> blacklist) {
            var numToActivate = Random.Range(min, max + 1);
            var remaining = numToActivate;

            var obstacles = gameManager.GetAllObstaclesOfType(type);

            var i = 0;

            while (remaining > 0) {

                if (i > obstacles.Count) {
                    break;
                }

                var obstacle = obstacles[i++];
                if (blacklist.Contains(obstacle)) {
                    continue;
                }

                if (!obstacle.TryAct(speedMultiplier)) {
                    continue;
                }

                blacklist.Add(obstacle);
                remaining--;
            }
        }
    }
    [Serializable]
    public abstract class Activation {
        public float delay;

        public abstract IEnumerator Activate(GameManager gameManager, float speedMultiplier);
    }

    [Serializable]
    public class SproutActivation : Activation {
        [Serializable]
        public struct SproutOffset {
            public Vector2Int offset;
            public float delay;
        }

        public List<SproutOffset> sproutOffsets;

        public override IEnumerator Activate(GameManager gameManager, float speedMultiplier) {
            yield break;
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

        public LogLocation location;
        public int logLength;

        public override IEnumerator Activate(GameManager gameManager, float speedMultiplier) {
            yield break;
        }
    }


    [CreateAssetMenu(menuName = "Flora/ObstacleEvent")]
    public class ObstacleEvent : ScriptableObject {

        public float minTimeToActivate, maxTimeToActivate;

        [SerializeReference]
        public List<Activation> predefinedActivations;
        public RandomizedActivation[] randomActivations;


        public void Activate(GameManager gameManager, float speedMultiplier) {
            HashSet<Obstacle> blacklist = new HashSet<Obstacle>();
            foreach (var randomizedActivation in randomActivations) {
                randomizedActivation.Activate(gameManager, speedMultiplier, blacklist);
            }
        }
    }
}