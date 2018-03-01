using UnityEngine;

using BaseGameLogic.States;

namespace BaseGameLogic.LogicModule
{
    public abstract class PhysicsFPPMovementModule : BaseLogicModule
    {
        [Header("Require components")]
        [SerializeField]
        protected Rigidbody _playerRigidbody = null;
        [SerializeField]
        protected Transform _eyesTransform = null;

        [Header("Inputs")]
        public Vector3 MovementVector = Vector3.zero;
        public Vector3 LookVector = Vector3.zero;

        [Header("Settings")]
        [SerializeField]
        protected Movement movement = new Movement();
        [SerializeField]
        protected SingleAxisRotation bodyRotation = new SingleAxisRotation();
        public float CurrentBodyRotation { get { return bodyRotation.CurrentRotation; } }

        [SerializeField]
        protected SingleAxisRotation eyesRotation = new SingleAxisRotation();
        public float CurrentEyesRotation { get { return eyesRotation.CurrentRotation; } }

        [SerializeField]
        protected float _jumpVelocity = 5;

        [Header("Ground check")]
        [SerializeField]
        protected bool _isGrounded = true;
        public bool IsGrounded
        {
            get { return _isGrounded; }
        }

        [SerializeField]
        protected Vector3 _groundedCheckOffset = new Vector3(0, 0.1f, 0);
        [SerializeField]
        protected float _groundedCheckDistance = 0.1f;

        protected override void Reset()
        {
            _playerRigidbody = GetComponent<Rigidbody>();
        }

        protected override void Awake()
        {
            bodyRotation.CurrentRotation = _eyesTransform.rotation.eulerAngles.x;
            eyesRotation.CurrentRotation = transform.rotation.eulerAngles.y;
        }

        protected override void Update()
        {
            HandleMovement();
            HandleRotation();
            GroundCheack();
        }

        public virtual void HandleMovement()
        {
            Vector3 move = movement.CalculatMove(MovementVector, Time.deltaTime);
            Vector3 newPosition = transform.position + move;
            _playerRigidbody.MovePosition(newPosition);
        }

        public virtual void HandleRotation()
        {
            bodyRotation.CalculateRotation(LookVector.x, Time.deltaTime);
            _playerRigidbody.MoveRotation(Quaternion.Euler(bodyRotation.Rotation));

            eyesRotation.Rotate(LookVector.y, Time.deltaTime);
        }

        public virtual void HandleJump()
        {
            if (_isGrounded)
            {
                _playerRigidbody.velocity += Vector3.up * _jumpVelocity;
            }
        }

        public virtual void GroundCheack()
        {
            Ray ray = new Ray(transform.position + _groundedCheckOffset, Vector3.down);
            _isGrounded = Physics.Raycast(ray, _groundedCheckDistance);

            Debug.DrawRay(ray.origin, ray.direction * _groundedCheckDistance, _isGrounded ? Color.green : Color.red);
        }
    }
}
