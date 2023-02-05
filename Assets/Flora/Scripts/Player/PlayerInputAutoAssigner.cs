using UnityEngine;
using UnityEngine.InputSystem;

namespace Flora.Scripts.Player {
    public class PlayerInputAutoAssigner : MonoBehaviour {
        public PlayerInputManager manager;
        public GameManager gameManager;
        public bool autoSpawn;
        private void OnEnable() {
            manager.onPlayerJoined += OnJoined;
            manager.onPlayerLeft += OnLeft;
        }

        private void OnDisable() {
            manager.onPlayerJoined -= OnJoined;
            manager.onPlayerLeft -= OnLeft;
        }

        private void OnLeft(PlayerInput input) {
            if (!input.TryGetComponent(out Player pulla)) {
                return;
            }
            Debug.Log($"Player @ {input} left");
        }

        private void OnJoined(PlayerInput input) {
            if (!input.TryGetComponent(out Player pulla)) {
                return;
            }
            Debug.Log($"Player @ {input} joined");
            if (autoSpawn) {
                pulla.Spawn(transform.position);
            }
        }
    }
}