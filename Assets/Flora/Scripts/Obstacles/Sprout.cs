using System.Collections;
using UnityEngine;
namespace Flora.Scripts.Obstacles {
    public class Sprout : Obstacle {
        public float delay;
        public float stayDuration;
        public Animator animator;
        public Collider collider;
        private static readonly int BeginKey = Animator.StringToHash("Begin");
        private static readonly int ResetKey = Animator.StringToHash("Reset");
        private static readonly int SpikeKey = Animator.StringToHash("Spike");

        public override ObstacleType ObstacleType => ObstacleType.Sprout;

        public override IEnumerator Act() {
            animator.SetTrigger(BeginKey);
            yield return new WaitForSeconds(delay);
            animator.SetTrigger(SpikeKey);
            yield return new WaitForSeconds(stayDuration);
            animator.SetTrigger(ResetKey);
        }
    }
}