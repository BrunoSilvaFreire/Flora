using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine.Serialization;
namespace Flora.Scripts.Obstacles {
    public class BeeActivation : Activation {
        public float relativePosition;
        [FormerlySerializedAs("randomLocation")]
        public bool randomDirection;

        [FormerlySerializedAs("location")]
        [HideIf(nameof(randomDirection))]
        public FloraDirection direction;

        public override IEnumerator Activate(GameManager gameManager, float speedMultiplier, HashSet<Obstacle> blacklist) {
            yield break;
        }
    }
}