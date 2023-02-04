using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

namespace Flora.Scripts.Obstacles {

    [CreateAssetMenu(menuName = "Flora/ObstacleEvent")]
    public class ObstacleEvent : ScriptableObject {

        public float minTimeToActivate, maxTimeToActivate;

        public RandomizedActivation[] randomActivations;

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

        public void Activate(GameManager gameManager, float speedMultiplier) {
            HashSet<Obstacle> blacklist = new HashSet<Obstacle>();
            foreach (var randomizedActivation in randomActivations) {
                randomizedActivation.Activate(gameManager, speedMultiplier, blacklist);
            }
        }
    }
}