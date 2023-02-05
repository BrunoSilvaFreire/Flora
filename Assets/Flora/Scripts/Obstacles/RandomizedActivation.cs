using System;
using System.Collections;
using System.Collections.Generic;
using Random = UnityEngine.Random;
namespace Flora.Scripts.Obstacles {
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
}