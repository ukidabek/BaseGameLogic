using UnityEngine;

using System;

using System.Collections;
using System.Collections.Generic;

namespace BaseGameLogic.LogicModule
{
    [Serializable]
    public class GroundDetector : TransformManipulator
    {
        [SerializeField]
        private LayerMask _groundMask;

        [SerializeField]
        protected Vector3 _groundedCheckOffset = new Vector3(0, -0.1f, 0);

        [SerializeField]
        protected float _groundedCheckDistance = 0.1f;

        [SerializeField]
        private bool _isGrounded = false;
        public bool IsGrounded { get { return _isGrounded; } }

        public GroundDetector() : base() {}

        public GroundDetector(string layerName) : base()
        {
            _groundMask = LayerMask.NameToLayer(layerName);
        }

        public GroundDetector(
            Transform transform, 
            Vector3 groundedCheckOffset, 
            float groundedCheckDistance) : base(transform)
        {
            _groundedCheckOffset = groundedCheckOffset;
            _groundedCheckDistance = groundedCheckDistance;
        }

        public bool DetectGround()
        {
            Vector3 startPosition = _transform.position + _groundedCheckOffset;
            Ray ray = new Ray(startPosition, -_transform.up);
            return _isGrounded = Physics.Raycast(ray, _groundedCheckDistance);
        }
    }
}