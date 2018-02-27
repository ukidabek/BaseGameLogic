using UnityEngine;

using System.Collections;
using System.Collections.Generic;

namespace BaseGameLogic.LogicModule
{
    public class SingleAxisRotation : TransformManipulator
    {
        [SerializeField]
        private float rotationSpeed = 10f;

        [SerializeField, Range(-360, 360)]
        private float _minRotation = 0f;

        [SerializeField, Range(-360, 360)]
        private float _maxRotation = 90f;

        [SerializeField]
        private float _currentRotation = 0f;
        public float CurrentRotation { get { return _currentRotation; } }

        [SerializeField]
        private Axis _axis = Axis.x;

        public SingleAxisRotation() : base()
        {
            rotationSpeed = 10f;
        }

        public SingleAxisRotation(Transform _transform, float _rotationSpeed) : base(_transform)
        {
            rotationSpeed = _rotationSpeed;
        }

        public SingleAxisRotation(Transform _transform, float _rotationSpeed, Axis _axis) : base(_transform)
        {
            rotationSpeed = _rotationSpeed;
            this._axis = _axis;
        }


        public float CalculateRotation(float input, float deltaTime)
        {
            float modifyRotationBy = rotationSpeed * input * deltaTime;
            _currentRotation = Mathf.Clamp(_currentRotation + modifyRotationBy, _minRotation, _maxRotation);
            return _currentRotation;
        }
    }

    public enum Axis { x, y, z }
}