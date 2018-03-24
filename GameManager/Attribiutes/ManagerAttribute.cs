using System;

namespace BaseGameLogic.Management
{
    public class ManagerAttribute : Attribute 
    {
        private bool _isNecessary = false;
        public bool IsNecessary { get { return _isNecessary; } }


        private Type _managerType = null;
        public Type ManagerType { get { return _managerType; } }

        public ManagerAttribute(bool isNecessary, Type managerType)
        {
            _isNecessary = isNecessary;
            _managerType = managerType;
        }

    }
}