using UnityEngine;

using System;
using System.Reflection;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;

namespace BaseGameLogic.logicElements
{
    [Serializable]
    public class GenericComparisonLogicElement
    {
        [SerializeField] private UnityEngine.Object _selectedObject = null;
        [SerializeField] private string _fieldName = string.Empty;
        [SerializeField] private float _value = 0f;

        [SerializeField] private ComparisionType _comparisionType = ComparisionType.Equals;

        private void GetValue(out object value, out Type fieldType)
        {
            Type type = _selectedObject.GetType();
            string[] fieldsNames = _fieldName.Split('.');
            FieldInfo info = null;
            
            for (int i = 0; i < fieldsNames.Length; i++)
            {
                info = type.GetField(_fieldName, BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance);
                type = info.FieldType;
            }

            fieldType = info.FieldType;
            value = info.GetValue(_selectedObject);        
        }

        public bool Compare()
        {
            object value = null;
            Type valueType = null;; 
            GetValue(out value, out valueType);

            if(valueType == typeof(int))
            {
                return CompareValueInt(value, _comparisionType);
            }

            if(valueType == typeof(float))
            {
                return CompareValueFloat(value, _comparisionType);
            }
            
            return false;
        }

        private bool CompareValueFloat(object value, ComparisionType comparisionType)
        {
            switch(comparisionType)
            {
                case ComparisionType.Equals:
                    return (float)value == _value;
                case ComparisionType.Smaller:
                    return (float)value < _value;
                case ComparisionType.Greather:
                    return (float)value > _value;
                case ComparisionType.GreatherOrEqual:
                    return (float)value >= _value;
                case ComparisionType.SmallerOrEqual:
                    return (float)value <= _value;
            }

            return false;
        }

        private bool CompareValueInt(object value, ComparisionType comparisionType)
        {
            switch(comparisionType)
            {
                case ComparisionType.Equals:
                    return (int)value == (int)_value;
                case ComparisionType.Smaller:
                    return (int)value < (int)_value;
                case ComparisionType.Greather:
                    return (int)value > (int)_value;
                case ComparisionType.GreatherOrEqual:
                    return (int)value >= (int)_value;
                case ComparisionType.SmallerOrEqual:
                    return (int)value <= (int)_value;
            }

            return false;
        }

    }

    internal enum ComparisionType
    {
        Equals,
        Greather,
        Smaller,
        GreatherOrEqual,
        SmallerOrEqual
    }
}
