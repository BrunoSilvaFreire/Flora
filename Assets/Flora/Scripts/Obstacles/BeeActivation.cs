using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;
namespace Flora.Scripts.Obstacles {
    [Serializable]
    public class BeeActivation : Activation {
        public float relativePosition;
        [FormerlySerializedAs("randomLocation")]
        public bool randomDirection;

        [FormerlySerializedAs("location")]
        [HideIf(nameof(randomDirection))]
        public FloraDirection direction;
       
        public override IEnumerator Activate(GameManager gameManager, float speedMultiplier, HashSet<Obstacle> blacklist) {
            FloraDirection tgtLocation;
            if (randomDirection) {
                tgtLocation = (FloraDirection) Random.Range(0, 4);
            } else {
                tgtLocation = direction;
            }

            var spawner = gameManager.GetAllObstaclesOfType(ObstacleType.Bee)
                .Cast<BeeSpawner>()
                .First(obstacle => obstacle.Location == tgtLocation);
            spawner.CurrentRelativePosition = relativePosition;
            if (spawner.TryActivate(speedMultiplier)) {
                blacklist.Add(spawner);
                yield return spawner.ActiveCoroutine;
            }
        }
    }
}