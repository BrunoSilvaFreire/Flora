using System;
using System.Collections;
using UnityEngine;
namespace Flora.Scripts.Obstacles {


    public class LogSpawner : Obstacle {
        public int MaxSize {
            get;
            set;
        }

        public float MaxDistance {
            get;
            set;
        }

        public int CurrentSize {
            get;
            set;
        }

        public Vector2 FacingDirection {
            get;
            set;
        }

        public Log logPrefab;

        public FloraDirection Location {
            get {
                switch (FacingDirection.x) {
                    case > 0:
                        return FloraDirection.Right;
                    case < 0:
                        return FloraDirection.Left;
                }

                switch (FacingDirection.y) {
                    case > 0:
                        return FloraDirection.Top;
                    case < 0:
                        return FloraDirection.Bottom;
                }

                throw new ArgumentOutOfRangeException();
            }
        }

        public override ObstacleType ObstacleType => ObstacleType.Log;

        private IEnumerator LogMovementWrapper(Log log, float speedMultiplier) {
            yield return log.Move(
                transform.position,
                new Vector3(FacingDirection.x, 0, FacingDirection.y),
                MaxDistance + 1,
                speedMultiplier
            );
            Destroy(log.gameObject);
        }

        public override IEnumerator Activate(float speedMultiplier) {
            var log = Instantiate(logPrefab);

            log.SetLength(CurrentSize);
            yield return StartCoroutine(LogMovementWrapper(log, speedMultiplier));
        }
    }
}