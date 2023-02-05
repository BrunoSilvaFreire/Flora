using System.Collections;
using UnityEngine;

namespace Flora.Scripts.Obstacles {

    public class Log : MonoBehaviour {
        public float appearAnimationDuration;
        public AnimationCurve dropHeightCurve;
        public Transform logVisualTransform;
        private bool killing;

        public void SetLength(int length) {
            var scale = transform.localScale;
            scale.y = length;
            logVisualTransform.localScale = scale;
        }

        public IEnumerator Move(Vector3 origin, Vector3 direction, float maxDistance, float speedMultiplier) {
            name = $"Log - {origin} -> {direction}, maxDistance: {maxDistance}, speedMultiplier: {speedMultiplier}";
            var pos = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0, pos, 0);

            killing = false;
            yield return Appear(origin, direction, speedMultiplier);
            killing = true;
            yield return Sweep(direction, maxDistance, speedMultiplier);
            killing = false;
        }

        private IEnumerator Sweep(Vector3 direction, float maxDistance, float speedMultiplier) {
            float travelled = 0;
            var speed = direction * (1 / speedMultiplier);

            while (travelled < maxDistance) {
                transform.Translate(speed * Time.deltaTime, Space.World);
                travelled += speed.magnitude * Time.deltaTime;
                yield return null;
            }
        }

        private IEnumerator Appear(Vector3 origin, Vector3 direction, float speedMultiplier) {
            float elapsed = 0;

            var destination = origin + direction;
            int i = 0;
            while (elapsed < appearAnimationDuration) {
                var t = elapsed / appearAnimationDuration;

                var y = dropHeightCurve.Evaluate(t);
                var pos = Vector3.Lerp(origin, destination, t);
                pos.y = y;
                transform.position = pos;

                elapsed += Time.deltaTime * (1 / speedMultiplier);
                i++;
                yield return null;
            }
            transform.position = destination;
        }
    }
}