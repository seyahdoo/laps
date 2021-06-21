using LapsRuntime;
using NUnit.Framework;
using UnityEngine;

namespace LapsEditModeTests {
    public class LapsTransformTests {
        private LapsTransform _lapsTransform;
        [SetUp]
        public void Setup() {
            _lapsTransform = new GameObject().AddComponent<LapsTransform>();
        }
        [TearDown]
        public void Teardown() {
            Object.DestroyImmediate(_lapsTransform.gameObject);
        }
        [Test]
        public void GetTransform() {
            var r = _lapsTransform.HandleInput(0, null, null);
            Assert.AreEqual(_lapsTransform.transform, r);
        }
        [Test]
        public void Enable() {
            _lapsTransform.gameObject.SetActive(false);
            _lapsTransform.HandleInput(1, null, null);
            Assert.AreEqual(true, _lapsTransform.gameObject.activeSelf);
        }
        [Test]
        public void Disable() {
            _lapsTransform.gameObject.SetActive(true);
            _lapsTransform.HandleInput(2, null, null);
            Assert.AreEqual(false, _lapsTransform.gameObject.activeSelf);
        }
        [Test]
        public void Slots() {
            var slots = new SlotList();
            _lapsTransform.GetInputSlots(slots);
            Assert.AreEqual(3, slots.Count);
            Assert.AreEqual(new LogicSlot("transform", 0, null, typeof(Transform)), slots[0]);
            Assert.AreEqual(new LogicSlot("enable", 1), slots[1]);
            Assert.AreEqual(new LogicSlot("disable", 2), slots[2]);
            slots.Clear();
            _lapsTransform.GetOutputSlots(slots);
            Assert.AreEqual(0, slots.Count);
        }
    }
}
