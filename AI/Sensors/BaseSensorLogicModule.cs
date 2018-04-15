using UnityEngine;

using System.Collections;
using System.Collections.Generic;

using BaseGameLogic.LogicModule;

namespace BaseGameLogic.AI.Sensors
{
    public class BaseSensorLogicModule : BaseLogicModule
    {
        [SerializeField] private Queue<GameObject> _targetQueue = new Queue<GameObject>();
        [SerializeField] private GameObject _target = null;

        public void TargetDetected(GameObject target)
        {
            _targetQueue.Enqueue(target);
        }

        protected override void Update()
        {
            float distance = 0; 
            if (_targetQueue.Count > 0)
            {
                _target = _targetQueue.Dequeue();
                distance = Vector3.Distance(transform.position, _target.transform.position);
            }
            else
                _target = null;

            while (_targetQueue.Count > 0)
            {
                GameObject target = _targetQueue.Dequeue();
                float currentDistance = Vector3.Distance(transform.position, target.transform.position);

                if(currentDistance < distance)
                {
                    _target = target;
                }
            }
        }
    }
}
