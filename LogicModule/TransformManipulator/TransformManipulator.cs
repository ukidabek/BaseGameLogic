using UnityEngine;

namespace BaseGameLogic.LogicModule
{
    public abstract class TransformManipulator
    {
        [SerializeField]
        protected Transform _transform;

        public TransformManipulator()
        {
            _transform = null;
        }

        public TransformManipulator(Transform transform)
        {
            _transform = transform;
        }
    }
}