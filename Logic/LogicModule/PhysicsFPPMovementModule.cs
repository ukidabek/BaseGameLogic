using UnityEngine;

using BaseGameLogic.States;

namespace BaseGameLogic.LogicModule
{
    public class PhysicsFPPMovementModule : MonoBehaviour
    {
        [Header("Require components")]
        [SerializeField] protected Rigidbody _playerRigidbody = null;

        [Header("Inputs")]
        public Vector3 MovementVector = Vector3.zero;
        public Vector3 LookVector = Vector3.zero;

        [Header("Settings")]
        [SerializeField] protected Movement movement = new Movement();
        [SerializeField] protected SingleAxisRotation bodyRotation = new SingleAxisRotation();
        public float CurrentBodyRotation
        {
            get { return bodyRotation.CurrentRotation; }
            set { bodyRotation.CurrentRotation = value; }
        }

        [SerializeField] protected SingleAxisRotation eyesRotation = new SingleAxisRotation();
        public float CurrentEyesRotation
        {
            get { return eyesRotation.CurrentRotation; }
            set { eyesRotation.CurrentRotation = value; }
        }

        [SerializeField] protected GroundDetector groundDetector = new GroundDetector();

        public bool IsGrounded {  get { return groundDetector.IsGrounded; } }

        [SerializeField] protected float _jumpVelocity = 5;

        protected void Reset()
        {
            _playerRigidbody = GetComponent<Rigidbody>();
        }

        protected void Awake()
        {
            bodyRotation.Initialize();
            eyesRotation.Initialize();
        }

        public void Calculate(float deltaTime)
        {
            HandleMovement(deltaTime);
            HandleRotation(deltaTime);
            GroundCheack();
        }

        public virtual void HandleMovement(float deltaTime)
        {
            Vector3 move = movement.CalculatMove(MovementVector, deltaTime);
            _playerRigidbody.MovePosition(transform.position + move);
        }

        public virtual void HandleRotation(float deltaTime)
        {
            bodyRotation.CalculateRotation(LookVector.x, deltaTime);
            _playerRigidbody.MoveRotation(Quaternion.Euler(bodyRotation.Rotation));

            eyesRotation.Rotate(LookVector.y, deltaTime);
        }

        public virtual void HandleJump()
        {
            if (groundDetector.IsGrounded)
            {
                _playerRigidbody.velocity += Vector3.up * _jumpVelocity;
            }
        }

        public virtual void GroundCheack()
        {
            groundDetector.DetectGround();
        }
    }
}
