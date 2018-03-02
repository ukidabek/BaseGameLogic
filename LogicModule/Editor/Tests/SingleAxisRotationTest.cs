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

        private SingleAxisRotation GetMovement()
        {
            gameObject = new GameObject();
            return new SingleAxisRotation(gameObject.transform, speed);
        }

        private void VerifyAxis(Axis axis, float expectedAngle)
        {
            switch (axis)
            {
                case Axis.x:
                    Assert.AreEqual(expectedAngle, Mathf.Floor(gameObject.transform.rotation.eulerAngles.x));
                    break;

                case Axis.y:
                    Assert.AreEqual(expectedAngle, Mathf.Floor(gameObject.transform.rotation.eulerAngles.y));
                    break;

                case Axis.z:
                    Assert.AreEqual(expectedAngle, Mathf.Floor(gameObject.transform.rotation.eulerAngles.z));
                    break;
            }
        }

        private SingleAxisRotation GetMovement(Axis axis)
        {
            gameObject = new GameObject();
            return new SingleAxisRotation(gameObject.transform, speed, axis);
        }

        public void Rotate_Axis_By_45_Degrees(Axis axis)
        {
            SingleAxisRotation axisRotation = GetMovement();
            float rotation = axisRotation.CalculateRotation(45, 1);

            Assert.AreEqual(45f, rotation);
        }

        [TestCase(45f, 45f)]
        [TestCase(-30f, 0f)]
        [TestCase(180f, 90f)]
        public void Rotate_Axis_Range_Test(float angle, float expectedAngle)
        {
            SingleAxisRotation axisRotation = GetMovement();
            float rotation = axisRotation.CalculateRotation(angle, 1);

            Assert.AreEqual(expectedAngle, rotation);
        }

        [TestCase(Axis.x, 45f, 45f)]
        [TestCase(Axis.y, -30f, 0f)]
        [TestCase(Axis.z, 180f, 90f)]
        public void Rotate_Rotate_Transform_On_Axis_With_Range_Test(Axis axis, float angle, float expectedAngle)
        {
            SingleAxisRotation axisRotation = GetMovement(axis);
            axisRotation.Rotate(angle, 1);
            VerifyAxis(axis, expectedAngle);
        }

        [Test]
        public void Move_Toword_45_Degrees_Calculate_Test()
        {
            SingleAxisRotation axisRotation = GetMovement();

            float angle = 0;
            for (int i = 0; i < 9; i++)
            {
                angle = axisRotation.CalculateRotationTowards(45f, 5);
            }

            Assert.AreEqual(45f, angle);
        }

        [TestCase(Axis.x)]
        [TestCase(Axis.y)]
        [TestCase(Axis.z)]
        public void Rotate_Toword_45_Degrees_Test(Axis axis)
        {
            SingleAxisRotation axisRotation = GetMovement(axis);

            for (int i = 0; i < 9; i++)
            {
               axisRotation.RotateTowards(45f, 5);
            }

            VerifyAxis(axis, 45f);
        }

        [TestCase(Axis.x)]
        [TestCase(Axis.y)]
        [TestCase(Axis.z)]
        public void Rotate_Toword_45_Degrees_Using_Current_Test(Axis axis)
        {
            SingleAxisRotation axisRotation = GetMovement(axis);

            axisRotation.CurrentRotation = 45;

            for (int i = 0; i < 9; i++)
            {
                axisRotation.RotateTowards(5);
            }

            VerifyAxis(axis, 45f);
        }

    }
}