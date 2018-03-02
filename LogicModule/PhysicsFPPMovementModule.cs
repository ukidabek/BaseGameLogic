﻿using UnityEngine;

using BaseGameLogic.States;

namespace BaseGameLogic.LogicModule
{
    public abstract class PhysicsFPPMovementModule : BaseLogicModule
    {
        [Header("Require components")]
        [SerializeField]
        protected Rigidbody _playerRigidbody = null;

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
        protected GroundDetector groundDetector = new GroundDetector();

        [SerializeField]
        protected float _jumpVelocity = 5;

        protected override void Reset()
        {
            _playerRigidbody = GetComponent<Rigidbody>();
        }

        protected override void Awake()
        {
            bodyRotation.Initialize();
            eyesRotation.Initialize();
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
            _playerRigidbody.MovePosition(transform.position + move);
        }

        public virtual void HandleRotation()
        {
            bodyRotation.CalculateRotation(LookVector.x, Time.deltaTime);
            _playerRigidbody.MoveRotation(Quaternion.Euler(bodyRotation.Rotation));

            eyesRotation.Rotate(LookVector.y, Time.deltaTime);
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
