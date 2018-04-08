using UnityEngine;

using System;
using System.Collections;
using System.Collections.Generic;

namespace BaseGameLogic.LogicModule
{
    public class LogicModuleOnListException : Exception
    {
        public BaseLogicModule AdditionalModule { get; private set; }

        public LogicModuleOnListException(BaseLogicModule additionalModule)
        {
            AdditionalModule = additionalModule;
        }

        public override string Message
        {
            get
            {
                return string.Format("Logic module of type {0} is already on the list.", AdditionalModule.GetType().Name);
            }
        }

    }
}