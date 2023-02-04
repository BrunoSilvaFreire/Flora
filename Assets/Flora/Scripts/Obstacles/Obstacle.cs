using System.Collections;
using UnityEngine;
namespace Flora.Scripts.Obstacles {
    public abstract class Obstacle : MonoBehaviour {
        public abstract ObstacleType ObstacleType {
            get;
        }

        public abstract IEnumerator Act();
    }

}