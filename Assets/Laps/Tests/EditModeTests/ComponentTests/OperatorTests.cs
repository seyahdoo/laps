using LapsRuntime;
using NUnit.Framework;
using UnityEngine;

namespace LapsEditModeTests {
    public class OperatorTests {
        private Operator _operator;
        private TestComponent _testComponent;
        [SetUp]
        public void Setup() {
            _operator = new GameObject().AddComponent<Operator>();
            _testComponent = new GameObject().AddComponent<TestComponent>();
            LapsEditor.LapsEditor.instance.lapsEditorLogicModule.Connect(_operator, 0, _testComponent, 0);
        }
        [Test]
        public void AddOperation() {
            _operator.a = 2f;
            _operator.b = 3f;
            _operator.operationType = Operator.OperationType.Add;
            _operator.Trigger();
            Assert.AreEqual(5f, _testComponent.inputList[0]);
            Assert.AreEqual(1, _testComponent.inputCallCount);
        }
        [Test]
        public void SubtractOperation() {
            _operator.a = 2f;
            _operator.b = 3f;
            _operator.operationType = Operator.OperationType.Subtract;
            _operator.Trigger();
            Assert.AreEqual(-1f, _testComponent.inputList[0]);
            Assert.AreEqual(1, _testComponent.inputCallCount);
        }
        [Test]
        public void MultipyOperation() {
            _operator.a = 2f;
            _operator.b = 3f;
            _operator.operationType = Operator.OperationType.Multiply;
            _operator.Trigger();
            Assert.AreEqual(6f, _testComponent.inputList[0]);
            Assert.AreEqual(1, _testComponent.inputCallCount);
        }
        [Test]
        public void DivideOperation() {
            _operator.a = 6f;
            _operator.b = 3f;
            _operator.operationType = Operator.OperationType.Divide;
            _operator.Trigger();
            Assert.AreEqual(2f, _testComponent.inputList[0]);
            Assert.AreEqual(1, _testComponent.inputCallCount);
        }
        [Test]
        public void TriggerOnAwake() {
            _operator.a = 6f;
            _operator.b = 3f;
            _operator.operationType = Operator.OperationType.Divide;
            _operator.triggerOnAwake = true;
            _operator.Awake();
            Assert.AreEqual(2f, _testComponent.inputList[0]);
            Assert.AreEqual(1, _testComponent.inputCallCount);
        }
        [Test]
        public void TriggerOnValueChange() {
            _operator.a = 6f;
            _operator.b = 3f;
            _operator.operationType = Operator.OperationType.Divide;
            _operator.HandleInput(1, 6f, null);
            Assert.AreEqual(1f, _testComponent.inputList[0]);
            Assert.AreEqual(1, _testComponent.inputCallCount);
        }
        [Test]
        public void TriggerOnResultChange() {
            _operator.a = 6f;
            _operator.b = 3f;
            _operator.operationType = Operator.OperationType.Divide;
            _operator.triggerOnResultChange = true;
            _operator.triggerOnValueChange = false;
            _operator.Awake();
            _operator.HandleInput(0, 6f, null);
            Assert.AreEqual(0, _testComponent.inputCallCount);
            _operator.HandleInput(0, 3f, null);
            Assert.AreEqual(1, _testComponent.inputCallCount);
            Assert.AreEqual(1f, _testComponent.inputList[0]);
        }
    }
}
