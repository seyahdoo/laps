using LapsEditor;
using LapsRuntime;
using NUnit.Framework;
using UnityEngine;

namespace LapsEditModeTests {
    public class ComparatorTests {
        private Comparator _comparator;
        private TestComponent _testerTrue;
        private TestComponent _testerFalse;
        [SetUp]
        public void Setup() {
            _comparator = new GameObject().AddComponent<Comparator>();
            _testerTrue = new GameObject().AddComponent<TestComponent>();
            _testerFalse = new GameObject().AddComponent<TestComponent>();
            LogicModule.Connect(_comparator, 0, _testerTrue, 0);
            LogicModule.Connect(_comparator, 1, _testerFalse, 0);
        }
        [Test]
        public void ComparatorGraterThan() {
            _comparator.a = 10f;
            _comparator.b = 5f;
            _comparator.comparisonMode = Comparator.ComparisonMode.GraterThan;
            _comparator.Trigger();
            Assert.AreEqual(1, _testerTrue.inputCallCount);
            Assert.AreEqual(0, _testerFalse.inputCallCount);
        }
        [Test]
        public void ComparatorGraterEquals() {
            _comparator.a = 10f;
            _comparator.b = 5f;
            _comparator.comparisonMode = Comparator.ComparisonMode.GraterEqual;
            _comparator.Trigger();
            Assert.AreEqual(1, _testerTrue.inputCallCount);
            Assert.AreEqual(0, _testerFalse.inputCallCount);
            
            _comparator.a = 10f;
            _comparator.b = 10f;
            _comparator.Trigger();
            Assert.AreEqual(2, _testerTrue.inputCallCount);
            Assert.AreEqual(0, _testerFalse.inputCallCount);

            _comparator.a = 10f;
            _comparator.b = 11f;
            _comparator.Trigger();
            Assert.AreEqual(2, _testerTrue.inputCallCount);
            Assert.AreEqual(1, _testerFalse.inputCallCount);
        }
        [Test]
        public void ComparatorLessThan() {
            _comparator.a = 10f;
            _comparator.b = 5f;
            _comparator.comparisonMode = Comparator.ComparisonMode.LessThan;
            _comparator.Trigger();
            Assert.AreEqual(0, _testerTrue.inputCallCount);
            Assert.AreEqual(1, _testerFalse.inputCallCount);
            
            _comparator.a = 10f;
            _comparator.b = 10f;
            _comparator.comparisonMode = Comparator.ComparisonMode.LessThan;
            _comparator.Trigger();
            Assert.AreEqual(0, _testerTrue.inputCallCount);
            Assert.AreEqual(2, _testerFalse.inputCallCount);
        }
        [Test]
        public void ComparatorLessEquals() {
            _comparator.a = 10f;
            _comparator.b = 5f;
            _comparator.comparisonMode = Comparator.ComparisonMode.LessEqual;
            _comparator.Trigger();
            Assert.AreEqual(0, _testerTrue.inputCallCount);
            Assert.AreEqual(1, _testerFalse.inputCallCount);
            
            _comparator.a = 10f;
            _comparator.b = 10f;
            _comparator.comparisonMode = Comparator.ComparisonMode.LessEqual;
            _comparator.Trigger();
            Assert.AreEqual(1, _testerTrue.inputCallCount);
            Assert.AreEqual(1, _testerFalse.inputCallCount);
        }
        [Test]
        public void ComparatorEquals() {
            _comparator.a = 10f;
            _comparator.b = 10f;
            _comparator.comparisonMode = Comparator.ComparisonMode.Equals;
            _comparator.Trigger();
            Assert.AreEqual(1, _testerTrue.inputCallCount);
            Assert.AreEqual(0, _testerFalse.inputCallCount);
            
            _comparator.a = 10f;
            _comparator.b = 5f;
            _comparator.comparisonMode = Comparator.ComparisonMode.Equals;
            _comparator.Trigger();
            Assert.AreEqual(1, _testerTrue.inputCallCount);
            Assert.AreEqual(1, _testerFalse.inputCallCount);
        }
        [Test]
        public void ComparatorEqualsTolerance() {
            _comparator.a = 10f;
            _comparator.b = 10.0001f;
            _comparator.comparisonMode = Comparator.ComparisonMode.Equals;
            _comparator.Trigger();
            Assert.AreEqual(1, _testerTrue.inputCallCount);
            Assert.AreEqual(0, _testerFalse.inputCallCount);
        }
        [Test]
        public void ComparatorNotEquals() {
            _comparator.a = 10f;
            _comparator.b = 5f;
            _comparator.comparisonMode = Comparator.ComparisonMode.NotEquals;
            _comparator.Trigger();
            Assert.AreEqual(1, _testerTrue.inputCallCount);
            Assert.AreEqual(0, _testerFalse.inputCallCount);
            
            _comparator.a = 10f;
            _comparator.b = 10f;
            _comparator.comparisonMode = Comparator.ComparisonMode.NotEquals;
            _comparator.Trigger();
            Assert.AreEqual(1, _testerTrue.inputCallCount);
            Assert.AreEqual(1, _testerFalse.inputCallCount);
        }
        [Test]
        public void ComparatorNotEqualsTolerance() {
            _comparator.a = 10f;
            _comparator.b = 10.0001f;
            _comparator.comparisonMode = Comparator.ComparisonMode.NotEquals;
            _comparator.Trigger();
            Assert.AreEqual(0, _testerTrue.inputCallCount);
            Assert.AreEqual(1, _testerFalse.inputCallCount);
        }
        [Test]
        public void ComparatorTriggerOnAwake() {
            _comparator.a = 10f;
            _comparator.b = 5f;
            _comparator.comparisonMode = Comparator.ComparisonMode.GraterThan;
            _comparator.triggerOnAwake = true;
            _comparator.Awake();
            Assert.AreEqual(1, _testerTrue.inputCallCount);
            Assert.AreEqual(0, _testerFalse.inputCallCount);
        }
        [Test]
        public void ComparatorTriggerOnResultChange() {
            _comparator.a = 10f;
            _comparator.b = 5f;
            _comparator.comparisonMode = Comparator.ComparisonMode.GraterThan;
            _comparator.triggerOnResultChange = true;
            _comparator.triggerOnValueChange = false;
            _comparator.Trigger();
            Assert.AreEqual(1, _testerTrue.inputCallCount);
            Assert.AreEqual(0, _testerFalse.inputCallCount);
            _comparator.b = 7f;
            _comparator.OnValueChange();
            Assert.AreEqual(1, _testerTrue.inputCallCount);
            Assert.AreEqual(0, _testerFalse.inputCallCount);
            _comparator.b = 15f;
            _comparator.OnValueChange();
            Assert.AreEqual(1, _testerTrue.inputCallCount);
            Assert.AreEqual(1, _testerFalse.inputCallCount);
        }
        [Test]
        public void ComparatorTriggerOnValueChange() {
            _comparator.a = 10f;
            _comparator.b = 5f;
            _comparator.comparisonMode = Comparator.ComparisonMode.GraterThan;
            _comparator.triggerOnResultChange = false;
            _comparator.triggerOnValueChange = true;
            _comparator.Trigger();
            Assert.AreEqual(1, _testerTrue.inputCallCount);
            Assert.AreEqual(0, _testerFalse.inputCallCount);
            _comparator.b = 5f;
            _comparator.OnValueChange();
            Assert.AreEqual(2, _testerTrue.inputCallCount);
            Assert.AreEqual(0, _testerFalse.inputCallCount);
            _comparator.b = 15f;
            _comparator.OnValueChange();
            Assert.AreEqual(2, _testerTrue.inputCallCount);
            Assert.AreEqual(1, _testerFalse.inputCallCount);
        }
    }
}
