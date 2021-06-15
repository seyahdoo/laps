using System.Collections;
using System.Collections.Generic;
using LapsEditModeTests;
using LapsEditor;
using LapsRuntime;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace LapsPlayModeTests {
    public class TimerTests {
        public Timer _timer;
        public TestComponent _test;
        [SetUp]
        public void Setup() {
            _timer = new GameObject().AddComponent<Timer>();
            _test = new GameObject().AddComponent<TestComponent>();
            LogicModule.Connect(_timer, 0, _test, 0);
        }
        [UnityTest]
        public IEnumerator BasicSetup() {
            _timer.times = new List<float>() {.05f};
            _timer.startOnAwake = true;
            _timer.Awake();
            Assert.AreEqual(0, _test.inputCallCount);
            yield return new WaitForSeconds(.1f);
            Assert.AreEqual(1, _test.inputCallCount);
        }
        [UnityTest]
        public IEnumerator MoreThanOneTime() {
            _timer.times = new List<float>() {.05f, .10f};
            _timer.startOnAwake = true;
            _timer.Awake();
            Assert.AreEqual(0, _test.inputCallCount);
            yield return new WaitForSeconds(.07f);
            Assert.AreEqual(1, _test.inputCallCount);
            yield return new WaitForSeconds(.05f);
            Assert.AreEqual(2, _test.inputCallCount);
        }
        [UnityTest]
        public IEnumerator TimesListUnordered() {
            _timer.times = new List<float>() {.15f, .05f, .10f};
            _timer.startOnAwake = true;
            _timer.Awake();
            Assert.AreEqual(0, _test.inputCallCount);
            yield return new WaitForSeconds(.07f);
            Assert.AreEqual(1, _test.inputCallCount);
            yield return new WaitForSeconds(.04f);
            Assert.AreEqual(2, _test.inputCallCount);
            yield return new WaitForSeconds(.05f);
            Assert.AreEqual(3, _test.inputCallCount);
        }
        [UnityTest]
        public IEnumerator SetTime() {
            _timer.times = new List<float>() {.15f, .05f, .10f};
            _timer.startOnAwake = true;
            _timer.Awake();
            _timer.HandleInput(2, .03f, null);
            Assert.AreEqual(0, _test.inputCallCount);
            yield return new WaitForSeconds(.04f);
            Assert.AreEqual(1, _test.inputCallCount);
            yield return new WaitForSeconds(.10f);
            Assert.AreEqual(1, _test.inputCallCount);
        }
        [UnityTest]
        public IEnumerator SetTimeStartsTimer() {
            _timer.times = new List<float>() {.15f, .05f, .10f};
            _timer.startOnAwake = false;
            _timer.Awake();
            _timer.HandleInput(2, .03f, null);
            Assert.AreEqual(0, _test.inputCallCount);
            yield return new WaitForSeconds(.04f);
            Assert.AreEqual(1, _test.inputCallCount);
            yield return new WaitForSeconds(.10f);
            Assert.AreEqual(1, _test.inputCallCount);
        }
        [UnityTest]
        public IEnumerator ManualStart() {
            _timer.times = new List<float>() {.15f, .05f, .10f};
            _timer.startOnAwake = false;
            _timer.Awake();
            _timer.HandleInput(0, null, null);
            Assert.AreEqual(0, _test.inputCallCount);
            yield return new WaitForSeconds(.07f);
            Assert.AreEqual(1, _test.inputCallCount);
            yield return new WaitForSeconds(.04f);
            Assert.AreEqual(2, _test.inputCallCount);
            yield return new WaitForSeconds(.05f);
            Assert.AreEqual(3, _test.inputCallCount);
        }
        [UnityTest]
        public IEnumerator StopHalfway() {
            _timer.times = new List<float>() {.15f, .05f, .10f};
            _timer.startOnAwake = false;
            _timer.Awake();
            _timer.HandleInput(0, null, null);
            Assert.AreEqual(0, _test.inputCallCount);
            yield return new WaitForSeconds(.07f);
            Assert.AreEqual(1, _test.inputCallCount);
            _timer.HandleInput(1, null, null);
            yield return new WaitForSeconds(.04f);
            Assert.AreEqual(1, _test.inputCallCount);
            yield return new WaitForSeconds(.05f);
            Assert.AreEqual(1, _test.inputCallCount);
        }
    }
}

