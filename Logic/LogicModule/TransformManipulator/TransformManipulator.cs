using UnityEngine;

namespace BaseGameLogic.LogicModule
{
    public abstract class TransformManipulator
    {
        [SerializeField]
        protected Transform _transform;

        [SerializeField]
        protected float speed = 10f;
        public float Speed
        {
            get { return speed; }
            set { speed = value; }
        }

        public TransformManipulator()
        {
            _transform = null;
        }

        public TransformManipulator(Transform transform)
        {
            _transform = transform;
        }

        public TransformManipulator(Transform transform, float speed) : this(transform)
        {
            this.speed = speed;
        }

    }
}