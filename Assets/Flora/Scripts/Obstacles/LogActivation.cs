using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;
namespace Flora.Scripts.Obstacles {

    [Serializable]
    public class LogActivation : Activation {

        public int logLength;

        [FormerlySerializedAs("randomLocation")]
        public bool randomDirection;

        [FormerlySerializedAs("location")]
        [HideIf(nameof(randomDirection))]
        public FloraDirection direction;

        public override IEnumerator Activate(GameManager gameManager, float speedMultiplier, HashSet<Obstacle> blacklist) {

            var length = GetLengthForLog(gameManager);

            FloraDirection tgtLocation;
            if (randomDirection) {
                tgtLocation = (FloraDirection) Random.Range(0, 4);
            } else {
                tgtLocation = direction;
            }

            var spawner = gameManager.GetAllObstaclesOfType(ObstacleType.Log)
                .Cast<LogSpawner>()
                .First(obstacle => obstacle.Location == tgtLocation);

            spawner.CurrentSize = length;

            if (spawner.TryActivate(speedMultiplier)) {
                blacklist.Add(spawner);
                yield return spawner.ActiveCoroutine;
            }
        }

        private int GetLengthForLog(GameManager gameManager) {
            int length;
            if (logLength == 0) {
                switch (direction) {

                    case FloraDirection.Top:
                    case FloraDirection.Bottom:
                        length = gameManager.worldManager.height;
                        break;
                    case FloraDirection.Left:
                    case FloraDirection.Right:
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
}