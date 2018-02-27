using UnityEngine;

using System;
using System.Collections;
using System.Collections.Generic;

using NUnit.Framework;

namespace BaseGameLogic.LogicModule
{
    public class LogicModulesContainerTests
    {
        private class TestLogicModule : BaseLogicModule {}

        private class NullTestLogicModule : BaseLogicModule {}

        [Test]
        public void Adding_New_Module_Test()
        {
            TestLogicModule module = new GameObject().AddComponent<TestLogicModule>();

            LogicModulesContainer container = new LogicModulesContainer();

            container.AddModule(module);

            Assert.AreEqual(container.ModuleListCount, 1);
        }

        [Test]
        public void Passed_Module_Reference_In_Null_Exception_Throw()
        {
            LogicModulesContainer container = new LogicModulesContainer();
            Assert.Throws<NullReferenceException>(() => container.AddModule(null));
        }

        [Test]
        public void Get_Module_Of_Type()
        {
            TestLogicModule module = new GameObject().AddComponent<TestLogicModule>();

            LogicModulesContainer container = new LogicModulesContainer();

            container.AddModule(module);

            TestLogicModule receivedModule = container.GetModule<TestLogicModule>();

            Assert.IsNotNull(receivedModule);
            Assert.IsInstanceOf(typeof(TestLogicModule), receivedModule);
        }

        [Test]
        public void GetModule_Return_Null_If_Dont_Contains_Module_Of_Type_And_List_Is_Empty()
        {
            LogicModulesContainer container = new LogicModulesContainer();
            TestLogicModule receivedModule = container.GetModule<TestLogicModule>();

            Assert.AreEqual(container.ModuleListCount, 0);
            Assert.IsNull(receivedModule);
        }

        [Test]
        public void GetModule_Return_Null_If_Dont_Contains_Module_Of_Type_And_List_Is_Not_Empty()
        {
            LogicModulesContainer container = new LogicModulesContainer();
            container.AddModule(new GameObject().AddComponent<NullTestLogicModule>());
            TestLogicModule receivedModule = container.GetModule<TestLogicModule>();

            Assert.Greater(container.ModuleListCount, 0);
            Assert.IsNull(receivedModule);
        }

    }
}