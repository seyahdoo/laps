using System.Collections;
using LapsEditModeTests;
using LapsEditor;
using LapsRuntime;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.TestTools;

namespace LapsPlayModeTests {
    public class DelayTests {
        private Delay _delay;
        private TestComponent _test;
        [SetUp]
        public void Setup() {
            _delay = new GameObject().AddComponent<Delay>();
            _test = new GameObject().AddComponent<TestComponent>();
            LogicModule.Connect(_delay, 0, _test, 0);
        }
        [UnityTest]
        public IEnumerator ZeroAmountDelayDelaysOneFrame() {
            _delay.delayAmount = 0f;
            _delay.HandleInput(0, null, null);
            Assert.AreEqual(0, _test.inputCallCount);
            yield return new Update();
            Assert.AreEqual(1, _test.inputCallCount);
            yield return new Update();
            Assert.AreEqual(1, _test.inputCallCount);
        }
        [UnityTest]
        public IEnumerator SetDelayAmountWorks() {
            _delay.HandleInput(1, .1f, null);
            Assert.AreEqual(0, _test.inputCallCount);
            _delay.HandleInput(0, null, null);
            Assert.AreEqual(0, _test.inputCallCount);
            yield return new WaitForSeconds(.08f);
            Assert.AreEqual(0, _test.inputCallCount);
            yield return new WaitForSeconds(.03f);
            Assert.AreEqual(1, _test.inputCallCount);
            yield return new Update();
            Assert.AreEqual(1, _test.inputCallCount);
        }
        [UnityTest]
        public IEnumerator DelayCanQueueInputs() {
            _delay.HandleInput(1, .10f, null);
            _delay.HandleInput(0, "one", null);
            _delay.HandleInput(1, .05f, null);
            _delay.HandleInput(0, "two", null);
            Assert.AreEqual(0, _test.inputCallCount);
            yield return new WaitForSeconds(.07f);
            Assert.AreEqual("two", _test.inputList[0]);
            yield return new WaitForSeconds(.04f);
            Assert.AreEqual("one", _test.inputList[1]);
            Assert.AreEqual(2, _test.inputCallCount);
        }
    }
}
