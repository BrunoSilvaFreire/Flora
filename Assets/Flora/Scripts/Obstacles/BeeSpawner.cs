using System;
using System.Collections;
using UnityEngine;

namespace Flora.Scripts.Obstacles {
    public class BeeSpawner : Obstacle {
        public float MaxDistance {
            get;
            set;
        }

        public Vector2 FacingDirection {
            get;
            set;
        }

        public Bee beePrefab;

        public float CurrentRelativePosition {
            get;
            set;
        }

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

        private IEnumerator LogMovementWrapper(Bee bee, float speedMultiplier) {
            yield return bee.Move(
                transform.position,
                new Vector3(FacingDirection.x, 0, FacingDirection.y),
                MaxDistance + 1,
                speedMultiplier
            );
            Destroy(bee.gameObject);
        }

        public override IEnumerator Activate(float speedMultiplier) {
            var bee = Instantiate(beePrefab);

            yield return StartCoroutine(LogMovementWrapper(bee, speedMultiplier));
        }
    }
}