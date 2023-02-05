using System;
using UnityEngine;
using UnityEngine.InputSystem;
namespace Flora.Scripts.Player {
    public class Player : MonoBehaviour {
        public PlayerInput input;
        public Pulla pullaPrefab;

        private Pulla _activePulla;

        public void Spawn(int id, Color color, Vector3 position) {
            if (_activePulla != null) {
                throw new Exception("There is an active pulla for this player");
            }

            _activePulla = Instantiate(pullaPrefab, position, Quaternion.identity);
            _activePulla.Attach(input);
            _activePulla.Setup(id, color);
        }

        public void Despawn() {
            if (_activePulla == null) {
                return;
            }
            Destroy(_activePulla);
            _activePulla = null;
        }
    }
}