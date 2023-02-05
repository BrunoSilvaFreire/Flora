using System.Collections;
using UnityEngine;
namespace Flora.Scripts.Obstacles {


    public class LogSpawner : Obstacle {
        public int MaxSize {
            get;
            set;
        }

        public int CurrentSize {
            get;
            set;
        }

        public Vector2 Axis {
            get;
            set;
        }

        public Log logPrefab;

        public LogLocation Location {
            get;
            private set;
        }

        public override ObstacleType ObstacleType => ObstacleType.Log;

        public override IEnumerator Activate(float speedMultiplier) {
            var log = Instantiate(logPrefab);
            yield return log.Move(Axis, speedMultiplier);
        }
    }
}