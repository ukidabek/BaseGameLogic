using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UnityEditor
{
    public class EditorGUIStack<T>
    {
        private Stack<T> stack = new Stack<T>();

        public void SetValue(ref T valueToSet, T newValue)
        {
            stack.Push(valueToSet);
            valueToSet = newValue;
        }

        public void RevertValue(ref T valueTorevert)
        {
            valueTorevert = stack.Pop();
        }
    }
}
