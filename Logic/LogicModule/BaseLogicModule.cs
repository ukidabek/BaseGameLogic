using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BaseGameLogic.LogicModule
{
    public abstract class BaseLogicModule : MonoBehaviour
    {
        protected virtual void Awake() {}
        protected virtual void Start() {}
        protected virtual void Reset() {}
        protected virtual void OnValidate() {}
        protected virtual void Update() {}
        protected virtual void FixedUpdate() {}
        protected virtual void LateUpdate() {}
    }
}
