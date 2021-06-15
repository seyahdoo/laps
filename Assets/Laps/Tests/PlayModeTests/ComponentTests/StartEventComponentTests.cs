using System.Collections;
using LapsEditModeTests;
using LapsEditor;
using LapsRuntime;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace LapsPlayModeTests {
    public class StartEventComponentTests {
        [UnityTest]
        public IEnumerator StartEventComponentTestsWithEnumeratorPasses() {
            var start = new GameObject().AddComponent<StartEventComponent>();
            var test = new GameObject().AddComponent<TestComponent>();
            LogicModule.Connect(start, 0, test, 0);
            Assert.AreEqual(0, test.inputCallCount);
            yield return null;
            Assert.AreEqual(1, test.inputCallCount);
            yield return null;
            Assert.AreEqual(1, test.inputCallCount);
        }
    }
}
