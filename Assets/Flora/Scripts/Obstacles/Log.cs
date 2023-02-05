using System.Collections;
using UnityEngine;
namespace Flora.Scripts.Obstacles {
    public class Log : MonoBehaviour {
        public float appearAnimationDuration;
        private bool killing;
        public void SetLength(int length) {
            var scale = transform.localScale;
            scale.y = length;
            transform.localScale = scale;
        }

        public IEnumerator Move(Vector2 direction, float speedMultiplier) {
            killing = false;
            yield return Appear(speedMultiplier);
            killing = true;
            //yield return Sweep(direction, speedMultiplier);
        }
        private IEnumerator Appear(float speedMultiplier) {
            throw new System.NotImplementedException();
        }
    }
}