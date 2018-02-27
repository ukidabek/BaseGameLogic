﻿using UnityEngine;

using System;
using System.Collections;
using System.Collections.Generic;

namespace BaseGameLogic.LogicModule
{
    [Serializable]
    public class Movement: TransformManipulator
    {
        [SerializeField]
        private float _locomotionSpeed = 5;
        public float LocomotionSpeed { get { return _locomotionSpeed; } }

        public Movement()
        {
            _transform = null;
            _locomotionSpeed = 5;
        }

        public Movement(Transform transform, float locomotionSpeed)
        {
            _transform = transform;
            _locomotionSpeed = locomotionSpeed;
        }


        public Vector3 CalculatMove(Vector3 input, float deltaTime)
        {
            Vector3 forward = _transform.forward;
            forward *= input.z;
            Vector3 right = _transform.right;
            right *= input.x;

            return ((forward + right) * _locomotionSpeed) * deltaTime;
        }

        public void Move(Vector3 input, float deltaTime)
        {
            Vector3 move = CalculatMove(input, deltaTime);
            _transform.position += move;
        }

        public Vector3 CalculatMoveTowards(Vector3 position, float deltaTime)
        {
            return Vector3.MoveTowards(_transform.position, position, _locomotionSpeed * deltaTime);
        }

        public void MoveTowards(Vector3 position, float deltaTime)
        {
            Vector3 move = CalculatMoveTowards(position, deltaTime);
            _transform.position = move;
        }
    }
}