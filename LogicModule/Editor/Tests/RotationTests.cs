using UnityEngine;

using System.Collections;
using System.Collections.Generic;

using NUnit.Framework;

namespace BaseGameLogic.LogicModule
{
    public class RotationTests
    {
        private Rotator GetRotator(float speed = 10)
        {
            GameObject gameObject = new GameObject();
            return new Rotator(gameObject.transform, speed);
        }

        [Test]
        public void Rotation_By_EulerAngles_Set_Test()
        {
            Rotator rotation = GetRotator();

            Vector3 newRotation = new Vector3(100, 100, 100);
            Quaternion newQuaternion = Quaternion.Euler(newRotation);
            rotation.EulerAngles = newRotation;

            Assert.AreEqual(newQuaternion, rotation.Rotation);
        }

        [Test]
        public void Rotation_RotateTowords_Test()
        {
            Rotator rotation = GetRotator();

            Vector3 newRotation = new Vector3(10, 10, 10);
            Quaternion newQuaternion = Quaternion.Euler(newRotation);
            rotation.EulerAngles = newRotation;

            for (int i = 0; i < 10; i++)
            {
                rotation.RotateTowards(1);
            }

            Assert.AreEqual(Mathf.Floor(newRotation.magnitude), Mathf.Floor(rotation.Rotation.eulerAngles.magnitude));
            Assert.AreEqual(newQuaternion, rotation.Rotation);
        }
    }
}