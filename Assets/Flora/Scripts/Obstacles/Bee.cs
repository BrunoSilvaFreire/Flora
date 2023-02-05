using System;
using System.Collections;
using UnityEngine;
namespace Flora.Scripts.Obstacles {
    public class Bee : MonoBehaviour {

        private bool _killing;

        public float appearAnimationDuration;
        public float speedMultiplier;
        public float waitDuration;
        public AnimationCurve dropHeightCurve;
        public SpriteRenderer renderer;
        public Animator animator;

        private static readonly int Appearing = Animator.StringToHash("Appearing");
        private static readonly int Waiting = Animator.StringToHash("Waiting");
        private static readonly int Sweeping = Animator.StringToHash("Sweeping");
        private static readonly int Done = Animator.StringToHash("Done");

        public IEnumerator Move(Vector3 origin, Vector3 direction, float maxDistance, float speedMultiplier) {
            name = $"Log - {origin} -> {direction}, maxDistance: {maxDistance}, speedMultiplier: {speedMultiplier}";
            renderer.flipX = direction.x > 0;

            _killing = false;

            animator.SetTrigger(Appearing);
            yield return Appear(origin, direction, speedMultiplier);

            animator.SetTrigger(Waiting);
            yield return Wait(speedMultiplier);

            _killing = true;
            animator.SetTrigger(Sweeping);

            yield return Sweep(direction, maxDistance, speedMultiplier);
            animator.SetTrigger(Done);
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

        private IEnumerator Appear(Vector3 destination, Vector3 direction, float speedMultiplier) {
            float elapsed = 0;

            var origin = destination - direction;
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