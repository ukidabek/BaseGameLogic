using UnityEngine;

using BaseGameLogic.States;

namespace BaseGameLogic.LogicModule
{
    [RequireComponent(
    typeof(Rigidbody),
    typeof(BaseStateObject))]
    public abstract class PhysicsFPPMovementModule : BaseLogicModule
    {
        [Header("Require components")]
        [SerializeField]
        private BaseStateObject _localPlayerController = null;
        [SerializeField]
        private Rigidbody _playerRigidbody = null;
        [SerializeField]
        private Transform _eyesTransform = null;

        [Header("Inputs")]
        public Vector3 MovementVector = Vector3.zero;
        public Vector3 LookVector = Vector3.zero;

        [Header("Settings")]
        [SerializeField]
        private float _locomotionSpeed = 5;
        [SerializeField]
        private float _jumpVelocity = 5;
        [SerializeField, Range(0, 90)]
        private float _minEyesRotation = 90f;
        [SerializeField, Range(0, 90)]
        private float _maxEyesRotation = 90f;

        private Vector3 _currentBodyRotationVector = Vector3.zero;
        public float CurrentBodyRotationVector { get { return _currentBodyRotationVector.y; } }
        private Vector3 _currentEyesRotationVector = Vector3.zero;
        public float EurrentEyesRotationVector { get { return _currentEyesRotationVector.x; } }

        [Header("Ground check")]
        [SerializeField]
        private bool _isGrounded = true;
        public bool IsGrounded
        {
            get { return _isGrounded; }
        }
        [SerializeField]
        private Vector3 _groundedCheckOffset = new Vector3(0, 0.1f, 0);
        [SerializeField]
        private float _groundedCheckDistance = 0.1f;

        private void Reset()
        {
            _localPlayerController = GetComponent<BaseStateObject>();
            _playerRigidbody = GetComponent<Rigidbody>();
        }

        private void Awake()
        {
            _currentEyesRotationVector = _eyesTransform.rotation.eulerAngles;
            _currentBodyRotationVector = transform.rotation.eulerAngles;
        }

        private void Update()
        {
            HandleMovement();
            HandleRotation();
            GroundCheack();
        }

        public virtual void HandleMovement()
        {
            Vector3 forward = transform.forward;
            forward *= MovementVector.z;
            Vector3 right = transform.right;
            right *= MovementVector.x;

            Vector3 rigidBodyVelocity = _playerRigidbody.velocity;

            Vector3 movement = ((forward + right) * _locomotionSpeed) * Time.deltaTime;
            Vector3 newPosition = transform.position + movement;
            _playerRigidbody.MovePosition(newPosition);
        }

        public virtual void HandleRotation()
        {
            _currentBodyRotationVector.y += LookVector.x;
            if (_currentBodyRotationVector.y > 360)
            {
                _currentBodyRotationVector.y -= 360;
            }

            Quaternion rotation = Quaternion.Euler(_currentBodyRotationVector);
            _playerRigidbody.MoveRotation(rotation);

            _currentEyesRotationVector.x += LookVector.y;
            _currentEyesRotationVector.x = Mathf.Clamp(_currentEyesRotationVector.x, -_maxEyesRotation, _minEyesRotation);
            rotation = Quaternion.Euler(_currentEyesRotationVector);
            _eyesTransform.localRotation = rotation;
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
