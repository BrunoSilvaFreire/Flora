using System;
using UnityEngine;
namespace Flora.Scripts.Obstacles {
    public abstract class ObstacleEvent : MonoBehaviour {
        public RandomizedActivation[] random;

        [Serializable]
        public struct RandomizedActivation {
            public int min, max;
            public ObstacleType type;
        }
    }
}