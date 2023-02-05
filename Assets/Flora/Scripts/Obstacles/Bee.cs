using System.Collections;
using UnityEngine;
namespace Flora.Scripts.Obstacles {
    public class Bee : MonoBehaviour {

        private bool _killing;

        public float appearAnimationDuration;
        public float speedMultiplier;
        public float waitDuration;
        public AnimationCurve dropHeightCurve;

        public IEnumerator Move(Vector3 origin, Vector3 direction, float maxDistance, float speedMultiplier) {
            name = $"Log - {origin} -> {direction}, maxDistance: {maxDistance}, speedMultiplier: {speedMultiplier}";
            var pos = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0, pos, 0);

            _killing = false;
            yield return Appear(origin, direction, speedMultiplier);
            yield return Wait(speedMultiplier);
            _killing = true;
            yield return Sweep(direction, maxDistance, speedMultiplier);
            _killing = false;
        }

        private IEnumerator Wait(float speedMultiplier) {
            yield return new WaitForSeconds(waitDuration * speedMultiplier);
        }

        private IEnumerator Sweep(Vector3 direction, float maxDistance, float speedMultiplier) {
            float travelled = 0;
            var speed = direction * (this.speedMultiplier * (1 / speedMultiplier));

            while (travelled < maxDistance) {
                transform.Translate(speed * Time.deltaTime, Space.World);
                travelled += speed.magnitude * Time.deltaTime;
                yield return null;
            }
        }

        private IEnumerator Appear(Vector3 origin, Vector3 direction, float speedMultiplier) {
            float elapsed = 0;

            var destination = origin + direction;
            while (elapsed < appearAnimationDuration) {
                var t = elapsed / appearAnimationDuration;

                var y = dropHeightCurve.Evaluate(t);
                var pos = Vector3.Lerp(origin, destination, t);
                pos.y = y;
                transform.position = pos;

                elapsed += Time.deltaTime * (1 / speedMultiplier);
                yield return null;
            }
            transform.position = destination;
        }
    }
}