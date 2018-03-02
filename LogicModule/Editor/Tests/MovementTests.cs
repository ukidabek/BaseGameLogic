using UnityEngine;

using System;
using System.Collections;
using System.Collections.Generic;

using NUnit.Framework;

namespace BaseGameLogic.LogicModule
{
    public class MovementTests 
    {
        GameObject gameObject = null;
        float speed = 5f;

        private Movement GetMovement()
        {
            gameObject = new GameObject();
            return new Movement(gameObject.transform, speed);
        }

        [Test]
        public void Calculat_Move_Vector_By_1_Forward()
        {
            Movement movement = GetMovement();

            Vector3 move = movement.CalculatMove(Vector3.forward, 1);

            Assert.AreEqual(Vector3.forward * speed, move);
        }

        [Test]
        public void Calculat_Move_Towards_To_Point_Vector()
        {
            Movement movement = GetMovement();

            Vector3 move = movement.CalculatMoveTowards(Vector3.forward, 1);

            Assert.AreEqual(Vector3.forward, move);
        }

        [Test]
        public void Move_Transform_Forward_By_1()
        {
            Movement movement = GetMovement();

            movement.Move(Vector3.forward, 1);

            Assert.AreEqual(gameObject.transform.position, Vector3.forward * speed);
        }

        [Test]
        public void Move_Transform_Towords()
        {
            Movement movement = GetMovement();

            movement.MoveTowards(Vector3.forward, 1);

            Assert.AreEqual(gameObject.transform.position, Vector3.forward);

        }
    }
}