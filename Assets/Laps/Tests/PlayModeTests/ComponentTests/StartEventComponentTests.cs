using System.Collections;
using System.Collections.Generic;
using LapsEditModeTests;
using LapsRuntime;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.TestTools;

namespace LapsPlayModeTests {
    public class StartEventComponentTests {
        [UnityTest]
        public IEnumerator StartEventComponentTestsWithEnumeratorPasses() {
            var start = new GameObject().AddComponent<StartEventComponent>();
            var test = new GameObject().AddComponent<TestComponent>();
            LapsEditor.LapsEditor.instance.lapsEditorLogicModule.Connect(start, 0, test, 0);
            Assert.AreEqual(0, test.inputCallCount);
            yield return null;
            Assert.AreEqual(1, test.inputCallCount);
            yield return null;
            Assert.AreEqual(1, test.inputCallCount);
        }
    }
}
