using UnityEngine;

using System.Collections;
using System.Collections.Generic;

using NUnit.Framework;

namespace UnityEditor
{
    public class EditorGUIStackTest
    {
        [Test]
        public void EditorGUIStack_Base_Test()
        {
            EditorGUIStack<float> floatStack = new EditorGUIStack<float>();

            float value = 5f;

            floatStack.SetValue(ref value, 10f);

            Assert.AreEqual(10f, value);

            floatStack.RevertValue(ref value);

            Assert.AreEqual(5f, value);
        }

        [Test]
        public void EditorGUIStack_Stacking_Test_Test()
        {
            EditorGUIStack<Color> colorStack = new EditorGUIStack<Color>();

            Color color1 = Color.black;
            Color color2 = Color.blue;

            colorStack.SetValue(ref color1, Color.yellow);
            Assert.AreEqual(Color.yellow, color1);
            colorStack.SetValue(ref color2, Color.yellow);
            Assert.AreEqual(Color.yellow, color2);
            colorStack.RevertValue(ref color2);
            Assert.AreEqual(Color.blue, color2);
            colorStack.RevertValue(ref color1);
            Assert.AreEqual(Color.black, color1);
        }
    }
}
