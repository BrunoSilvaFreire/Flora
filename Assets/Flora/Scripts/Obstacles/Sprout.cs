using System;
using System.Collections;
using Flora.Scripts.Player;
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

        private void OnTriggerEnter(Collider other) {
            other.TryKillPulla();
        }


        public override IEnumerator Activate(float speedMultiplier) {
            animator.SetTrigger(BeginKey);
            collider.enabled = false;
            yield return new WaitForSeconds(delay * speedMultiplier);
            animator.SetTrigger(SpikeKey);
            collider.enabled = true;
            yield return new WaitForSeconds(stayDuration * speedMultiplier);
            collider.enabled = false;
            animator.SetTrigger(ResetKey);
        }
    }
}