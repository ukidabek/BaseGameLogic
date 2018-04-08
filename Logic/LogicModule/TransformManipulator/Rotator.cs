using UnityEngine;

using System;
using System.Collections;
using System.Collections.Generic;

namespace BaseGameLogic.LogicModule
{
    [Serializable]
    public class Rotator : TransformManipulator
    {
        private Quaternion _rotation = Quaternion.identity; 
        public Vector3 EulerAngles
        {
            get { return _rotation.eulerAngles; }
            set { _rotation = Quaternion.Euler(value); }
        }

        public Quaternion Rotation
        {
            get { return _rotation; } 
            set { _rotation = value; }
        }

        public Rotator() {}

        public Rotator(Transform transform, float speed) 
            : base(transform, speed) {}

        public Quaternion CalculateRotationTowards(float deltaTime)
        {
            return Quaternion.RotateTowards(_transform.localRotation, _rotation, speed * deltaTime); 
        }

        public void RotateTowards(float deltaTime)
        {
            _transform.rotation = CalculateRotationTowards(deltaTime);
        }
    }
}