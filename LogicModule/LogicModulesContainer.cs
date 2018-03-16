﻿using UnityEngine;

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace BaseGameLogic.LogicModule
{
    [Serializable]
    public class LogicModulesContainer
    {
        [SerializeField]
        private List<BaseLogicModule> _modulesList = new List<BaseLogicModule>();
        public int ModuleListCount { get { return _modulesList.Count; } }

        public void AddModule(BaseLogicModule module)
        {
            if (module == null)
                throw new NullReferenceException();

            BaseLogicModule onListModule = GetModule(module.GetType());

            if (onListModule != null)
                if (onListModule != module)
                    throw new LogicModuleOnListException(module);
                else
                    return;

            _modulesList.Add(module);
        }

        public BaseLogicModule GetModule(Type type)
        {
            for (int i = 0; i < _modulesList.Count; i++)
            {
                if (_modulesList[i] != null && _modulesList[i].GetType() == type)
                    return _modulesList[i];
            }

            return null;
        }

        public T GetModule<T>() where T : BaseLogicModule
        {
            for (int i = 0; i < _modulesList.Count; i++)
            {
                if (_modulesList[i] is T)
                    return _modulesList[i] as T;
            }

            return null;
        }
    }
}