using System.Collections;
using System.Collections.Generic;
using LapsRuntime;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace LapsEditModeTests {
    public class LapsTransformTests {
        private LapsTransform _lapsTransform;
        [SetUp]
        public void Setup() {
            _lapsTransform = new GameObject().AddComponent<LapsTransform>();
            _lapsTransform.Awake();
        }
        [Test]
        public void GetTransform() {
            var r = _lapsTransform.HandleInput(0, null, null);
            Assert.AreEqual(_lapsTransform.transform, r);
        }
        [Test]
        public void Slots() {
            var slots = new List<LogicSlot>();
            _lapsTransform.GetInputSlots(slots);
            Assert.AreEqual(1, slots.Count);
            Assert.AreEqual(new LogicSlot("transform", 0, null, typeof(Transform)), slots[0]);
            slots.Clear();
            _lapsTransform.GetOutputSlots(slots);
            Assert.AreEqual(0, slots.Count);
        }
    }
}
