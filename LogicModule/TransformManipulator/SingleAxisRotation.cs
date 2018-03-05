using UnityEngine;

using System;
using System.Collections;
using System.Collections.Generic;

namespace BaseGameLogic.LogicModule
{
    [Serializable]
    public class SingleAxisRotation : TransformManipulator
    {
        [SerializeField]
        private bool _isConstrained = true;
        public bool IsConstrained
        {
            get { return _isConstrained; }
            set { _isConstrained = value; }
        }

        [SerializeField, Range(-180, 180)]
        private float _minRotation = 0f;
        public float MinRotation
        {
            get { return _minRotation; }
            set { _minRotation = ClampToFullAngle(value); }
        }

        [SerializeField, Range(-180, 180)]
        private float _maxRotation = 90f;
        public float MaxRotation
        {
            get { return _maxRotation; }
            set { _maxRotation = ClampToFullAngle(value); }
        }


        [SerializeField]
        private float _currentRotation = 0f;
        public float CurrentRotation
        {
            get { return _currentRotation; }
            set { _currentRotation = value; }
        }

        [SerializeField]
        private Axis _axis = Axis.x;
        public Axis Axis
        {
            get { return _axis; } 
            set { _axis = value; }
        }

        public Vector3 Rotation
        {
            get
            {
                return new Vector3(
                _axis == Axis.x ? _currentRotation : 0f,
                _axis == Axis.y ? _currentRotation : 0f,
                _axis == Axis.z ? _currentRotation : 0f);
            }
        }

        private float AxisRotation
        {
            get
            {
                switch (_axis)
                {
                    case Axis.x:
                        return _transform.localRotation.eulerAngles.x;
                    case Axis.y:
                        return _transform.localRotation.eulerAngles.y;
                    case Axis.z:
                        return _transform.localRotation.eulerAngles.z;
                }

                return 0f;
            }

            set { _transform.localRotation = GenerateRotation(value); }
        }

        public SingleAxisRotation() : base()
        {
            speed = 10f;
        }

        public SingleAxisRotation(Transform _transform, float _rotationSpeed) : base(_transform)
        {
            speed = _rotationSpeed;
        }

        public SingleAxisRotation(Transform _transform, float _rotationSpeed, Axis _axis) : base(_transform)
        {
            speed = _rotationSpeed;
            this._axis = _axis;
        }

        public void Initialize()
        {
            _currentRotation = AxisRotation;
        }

        public float CalculateRotation(float input, float deltaTime)
        {
            float modifyRotationBy = speed * input * deltaTime;
            float newRotation = _currentRotation + modifyRotationBy;

            return _currentRotation = _isConstrained ? Mathf.Clamp(newRotation, _minRotation, _maxRotation) : newRotation; ;
        }

        public float CalculateRotationTowards(float newAngle, float deltaTime)
        {
            return _currentRotation = Mathf.MoveTowards(_currentRotation, newAngle, speed * deltaTime);
        }

        public float CalculateRotationTowards(float deltaTime)
        {
            return Mathf.MoveTowards(AxisRotation, _currentRotation, speed * deltaTime);
        }

        public void Rotate(float input, float deltaTime)
        {
            CalculateRotation(input, deltaTime);
            ApplyRotation();
        }

        public void RotateTowards(float input, float deltaTime)
        {
            CalculateRotationTowards(input, deltaTime);
            ApplyRotation();
        }

        public void RotateTowards(float deltaTime)
        {
            AxisRotation = CalculateRotationTowards(deltaTime);
        }

        private void ApplyRotation()
        {
            _transform.localRotation = Quaternion.Euler(Rotation);
        }

        private float ClampToFullAngle(float angle)
        {
            return Mathf.Clamp(angle, -180, 180);
        }

        private Quaternion GenerateRotation(float angle)
        {
            return Quaternion.Euler(
                _axis == Axis.x ? angle : _transform.localRotation.eulerAngles.x,
                _axis == Axis.y ? angle : _transform.localRotation.eulerAngles.y,
                _axis == Axis.z ? angle : _transform.localRotation.eulerAngles.z); 
        }
    }

    public enum Axis { x, y, z }
}