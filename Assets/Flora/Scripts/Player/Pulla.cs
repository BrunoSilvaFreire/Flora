using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace Flora.Scripts.Player {
    public static class PullaHelpers {
        public static void TryKillPulla(this Collider collider) {
            if (collider.TryGetComponent(out Pulla pulla)) {
                pulla.Kill();
            }
        }
    }
    public class Pulla : MonoBehaviour {
        public Vector2 move;
        public EntityAction jump;
        public CharacterController controller;
        public float jumpHeight;
        public float moveSpeed;
        public float gravityScale;
        public float gravityScaleWhenHoldingJump;
        public float maxSpeed;
        public float decelerationThreshold;
        public float decelerationSpeed;
        public int score;
        public int id;
        public Animator animator;
        public Text playerNamespace;
        public SpriteRenderer spriteRenderer;

        private bool _attached;
        private InputAction _move, _jump;
        private CollisionFlags _collisionFlags;
        private static readonly int Death = Animator.StringToHash("Death");
        private static readonly int Grounded = Animator.StringToHash("Grounded");
        private static readonly int XVelocity = Animator.StringToHash("XVelocity");
        private static readonly int YVelocity = Animator.StringToHash("YVelocity");
        private static readonly int ZVelocity = Animator.StringToHash("ZVelocity");
        private static readonly int Revived = Animator.StringToHash("Revived");
        public Color color;

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

            if (Dead) {
                move = Vector2.zero;
                jump.Current = false;
            } else {
                move = _move.ReadValue<Vector2>();
                jump.Current = _jump.IsPressed();
            }
        }

        public bool Dead {
            get;
            private set;
        }

        public void Kill() {
            Debug.Log($"{this} has been killed", this);
            Dead = true;
            animator.SetTrigger(Death);
            GameManager.Instance.NotifyDeath(this);
        }

        private void FixedUpdate() {
            var velocity = controller.velocity;

            var inputDirection = new Vector3(move.x, 0, move.y).normalized;
            if (inputDirection.magnitude > decelerationThreshold) {
                velocity += inputDirection * moveSpeed;
            } else {
                velocity.x = Mathf.Lerp(velocity.x, 0, decelerationSpeed);
                velocity.z = Mathf.Lerp(velocity.z, 0, decelerationSpeed);
            }

            var grounded = (_collisionFlags & CollisionFlags.Below) != 0;
            var gravityScale = jump.Current ? gravityScaleWhenHoldingJump : this.gravityScale;
            var shouldJump = jump.Consume() && grounded;

            velocity += Physics.gravity * gravityScale;

            if (shouldJump) {
                velocity.y = jumpHeight;
            }
            var horizontal = new Vector2(velocity.x, velocity.z);
            horizontal = Vector2.ClampMagnitude(horizontal, maxSpeed);
            velocity.x = horizontal.x;
            velocity.z = horizontal.y;

            animator.SetBool(Grounded, grounded);
            animator.SetFloat(XVelocity, velocity.x);
            animator.SetFloat(YVelocity, velocity.y);
            animator.SetFloat(ZVelocity, velocity.z);

            _collisionFlags = controller.Move(velocity * Time.fixedDeltaTime);
        }
        public void Revive() {
            Dead = false;
            animator.SetTrigger(Revived);
        }
        public void Setup(int NewID, Color color) {
            id = NewID;
            this.color = color;
            playerNamespace.text = $"P{NewID}";
            playerNamespace.color = color;
            spriteRenderer.color = color;
        }
        public void IncrementScore() {
            score++;
        }
    }
}