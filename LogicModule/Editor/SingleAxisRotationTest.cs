using UnityEngine;

using System;
using System.Collections;
using System.Collections.Generic;

using NUnit.Framework;

namespace BaseGameLogic.LogicModule
{
    public class SingleAxisRotationTest
    {
        private GameObject gameObject = null;
        private float speed = 1f;

        private SingleAxisRotation GetMovement(Axis axis)
        {
            gameObject = new GameObject();
            return new SingleAxisRotation(gameObject.transform, speed, axis);
        }

        [TestCase(Axis.x)]
        [TestCase(Axis.y)]
        [TestCase(Axis.z)]
        public void Rotate_Axis_By_45_Degrees(Axis axis)
        {
            SingleAxisRotation axisRotation = GetMovement(axis);
            float rotation = axisRotation.CalculateRotation(45, 1);

            Assert.AreEqual(45f, rotation);
        }
    }
}