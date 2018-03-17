using UnityEngine;

using System;
using System.Collections;
using System.Collections.Generic;

using NUnit.Framework;

namespace BaseGameLogic.Inputs
{
    public class InputCollectorTests
    {
        private TestInputCollector GetCollector()
        {
            GameObject gameObject = new GameObject();
            return gameObject.AddComponent<TestInputCollector>();
        }

        [Test]
        public void Add_New_Input_Source_Test()
        {
            TestInputCollector collector = GetCollector();

            collector.AddInputSource(typeof(AInputSource));

            Assert.AreEqual(1, collector.Count);
            Assert.AreEqual(1, collector.transform.childCount);

            Assert.IsTrue(collector[0] is AInputSource);
        }

        [Test]
        public void Wrong_Type_Throw_Exception_Test()
        {
            TestInputCollector collector = GetCollector();

            Assert.Throws(
                typeof(TypeIsNotInputSourceException), 
                () => { collector.AddInputSource(typeof(TestInputCollector)); });
        }

        [Test]
        public void Add_New_Input_By_Template_Source_Test()
        {
            TestInputCollector collector = GetCollector();

            collector.AddInputSource<AInputSource>();

            Assert.AreEqual(1, collector.Count);
            Assert.AreEqual(1, collector.transform.childCount);

            Assert.IsTrue(collector[0] is AInputSource);
        }

        [Test]
        public void Remove_Input_Source_Test()
        {
            TestInputCollector collector = GetCollector();

            collector.AddInputSource<AInputSource>();

            Assert.AreEqual(1, collector.Count);
            Assert.AreEqual(1, collector.transform.childCount);

            collector.RemoveAt(0);

            Assert.AreEqual(0, collector.Count);
            Assert.AreEqual(0, collector.transform.childCount);
        }
    }

    internal class TestInputCollector : BaseInputCollector {}
    internal class AInputSource : BaseInputSource {}
    internal class BInputSource : BaseInputSource {}

}