using UnityEngine;

using System.Collections;
using System.Collections.Generic;

using NUnit.Framework;

namespace BaseGameLogic.LogicModule
{

    public class GroundDetectorTest
    {
        private GameObject gameObject = null;

        private GroundDetector GetGroundDetector()
        {
            gameObject = new GameObject();
            gameObject.transform.position = Vector3.zero;
            GroundDetector groundDetector = new GroundDetector(
                gameObject.transform,
                new Vector3(0f, .5f, 0f),
                0.6f);

            gameObject = GameObject.CreatePrimitive(PrimitiveType.Cube);

            gameObject.transform.position = new Vector3(0f, -.5f, 0f);

            return groundDetector;
        }

        [Test]
        public void Ground_Detection_Test()
        {
            GroundDetector groundDetector = GetGroundDetector();

            Assert.IsTrue(groundDetector.DetectGround());
            Assert.IsTrue(groundDetector.IsGrounded);
        }
    }
}