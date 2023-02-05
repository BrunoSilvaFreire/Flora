using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Flora.Scripts.Player {
    public class PlayerInputAutoAssigner : MonoBehaviour {
        public PlayerInputManager manager;
        public GameManager gameManager;
        public bool autoSpawn;
        public Color[] colors;

        private void Update() {
            if (Keyboard.current?.rKey?.isPressed ?? false) {
                gameManager.Restart();
            }
        }

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
                var index = input.playerIndex;
                pulla.Spawn(index+1, colors[index], transform.position);
            }
        }
    }
}