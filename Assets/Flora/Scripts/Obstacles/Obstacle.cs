using System.Collections;
using UnityEngine;
namespace Flora.Scripts.Obstacles {
    public abstract class Obstacle : MonoBehaviour {
        public abstract ObstacleType ObstacleType {
            get;
        }

        public abstract IEnumerator Activate(float speedMultiplier);

        public Coroutine ActiveCoroutine {
            get;
            private set;
        }

        private IEnumerator ActionWrapper(float speedMultiplier) {
            yield return Activate(speedMultiplier);
            ActiveCoroutine = null;
        }

        public bool TryActivate(float speedMultiplier) {
            if (ActiveCoroutine != null) {
                return false;
            }

            ActiveCoroutine = StartCoroutine(ActionWrapper(speedMultiplier));
            return true;
        }
    }

}