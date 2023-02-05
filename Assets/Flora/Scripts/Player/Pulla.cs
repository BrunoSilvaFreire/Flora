using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Flora.Scripts.Player {

    public class Pulla : MonoBehaviour {
        public Vector2 move;
        public EntityAction jump;
        public CharacterController controller;
        public float jumpHeight;
        public float moveSpeed;
        private bool _grounded;

        private bool _attached;
        private InputAction _move, _jump;

        public void Attach(PlayerInput input) {
            _attached = true;
            _move = input.actions["Move"];
            _jump = input.actions["Jump"];
        }

        public void Detach() {
            _attached = false;
        }

        private void OnDestroy() {
            Detach();
        }

        private void Update() {
            if (!_attached) {
                return;
            }

            move = _move.ReadValue<Vector2>();
            jump.Current = _jump.IsPressed();
        }

        private void FixedUpdate() {
            var velocity = new Vector3(move.x, 0, move.y);

            var shouldJump = jump.Consume() && _grounded;
            if (shouldJump) {
                velocity.y = jumpHeight;
            }

            _grounded = controller.SimpleMove(velocity * moveSpeed);
        }
    }
}