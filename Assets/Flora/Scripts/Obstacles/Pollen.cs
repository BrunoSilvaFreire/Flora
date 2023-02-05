using Flora.Scripts.Player;
using UnityEngine;

namespace Flora.Scripts.Obstacles {
    public class Pollen : MonoBehaviour {
        private void OnTriggerEnter(Collider other) {
            if (other.TryGetComponent(out Pulla pulla)) {
                pulla.IncrementScore();
            }
        }
    }
}