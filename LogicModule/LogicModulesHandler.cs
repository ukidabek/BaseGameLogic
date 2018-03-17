using UnityEngine;

using System;

namespace BaseGameLogic.LogicModule
{
    public class LogicModulesHandler : MonoBehaviour
    {
        [SerializeField, HideInInspector]
        protected LogicModulesContainer logicModulesContainer = new LogicModulesContainer();
        public LogicModulesContainer LogicModulesContainer { get { return logicModulesContainer; } }

        public T GetModule<T>() where T : BaseLogicModule 
        {
            return LogicModulesContainer.GetModule<T>();
        }

        public void AddModule(BaseLogicModule module)
        {
            LogicModulesContainer.AddModule(module);
        }

        public BaseLogicModule GetModule(Type type)
        {
            return LogicModulesContainer.GetModule(type);
        }
    }
}
