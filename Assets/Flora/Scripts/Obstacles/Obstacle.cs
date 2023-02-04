using System.Collections;
using UnityEngine;
namespace Flora.Scripts.Obstacles {
    public abstract class Obstacle : MonoBehaviour {
        public abstract ObstacleType ObstacleType {
            get;
        }

        public abstract IEnumerator Activate(float speedMultiplier);

        private Coroutine _coroutine;

        private IEnumerator ActionWrapper(float speedMultiplier) {
            yield return Activate(speedMultiplier);
            _coroutine = null;
        }

        public bool TryAct(float speedMultiplier) {
            if (_coroutine == null) {
                return false;
            }

            _coroutine = StartCoroutine(ActionWrapper(speedMultiplier));
            return true;
        }
    }

}