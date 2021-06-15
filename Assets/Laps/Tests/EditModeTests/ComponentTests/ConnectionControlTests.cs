using LapsEditor;
using LapsRuntime;
using NUnit.Framework;
using UnityEngine;

namespace LapsEditModeTests {
    public class ConnectionControlTests {
        public ConnectionControl _connectionControl;
        public TestComponent _testComponent;
        [SetUp]
        public void Setup() {
            _connectionControl = new GameObject().AddComponent<ConnectionControl>();
            _testComponent = new GameObject().AddComponent<TestComponent>();
            LogicModule.Connect(_connectionControl, 0, _testComponent, 0);
        }
        [Test]
        public void EnabledOnAwakeFalseWorks() {
            _connectionControl.enabledOnAwake = false;
            _connectionControl.Awake();
            _connectionControl.HandleInput(0, 5f, null);
            Assert.AreEqual(0, _testComponent.inputCallCount);
        }
        [Test]
        public void EnabledOnAwakeTrueWorks() {
            _connectionControl.enabledOnAwake = true;
            _connectionControl.Awake();
            _connectionControl.HandleInput(0, 5f, null);
            Assert.AreEqual(1, _testComponent.inputCallCount);
            Assert.AreEqual(5f, _testComponent.inputList[0]);
        }
        [Test]
        public void DisableInputWorks() {
            _connectionControl.enabledOnAwake = true;
            _connectionControl.Awake();
            _connectionControl.HandleInput(0, 5f, null);
            Assert.AreEqual(1, _testComponent.inputCallCount);
            Assert.AreEqual(5f, _testComponent.inputList[0]);
            _connectionControl.HandleInput(2, null, null);
            _connectionControl.HandleInput(0, 5f, null);
            Assert.AreEqual(1, _testComponent.inputCallCount);
        }
        [Test]
        public void EnableInputWorks() {
            _connectionControl.enabledOnAwake = false;
            _connectionControl.Awake();
            _connectionControl.HandleInput(0, 5f, null);
            Assert.AreEqual(0, _testComponent.inputCallCount);
            _connectionControl.HandleInput(1, null, null);
            _connectionControl.HandleInput(0, 5f, null);
            Assert.AreEqual(1, _testComponent.inputCallCount);
            Assert.AreEqual(5f, _testComponent.inputList[0]);
        }
    }
}
