using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Flora.Scripts.Obstacles {
    [Serializable]
    public abstract class Activation {
        public bool block;
        public float delay;

        public abstract IEnumerator Activate(GameManager gameManager, float speedMultiplier, HashSet<Obstacle> blacklist);
    }


    [CreateAssetMenu(menuName = "Flora/ObstacleEvent")]
    public class ObstacleEvent : ScriptableObject {

        public float minTimeToActivate, maxTimeToActivate;

        [SerializeReference]
        public List<Activation> activations;

        public IEnumerator Activate(GameManager gameManager, float speedMultiplier) {
            var blacklist = new HashSet<Obstacle>();

            foreach (var activation in activations) {
                var block = activation.Activate(gameManager, speedMultiplier, blacklist);
                if (activation.block) {
                    yield return block;
                } else {
                    gameManager.StartCoroutine(block);
                }
            }
        }
    }
}