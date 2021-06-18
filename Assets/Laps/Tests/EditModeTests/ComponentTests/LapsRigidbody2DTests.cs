using LapsRuntime;
using NUnit.Framework;
using UnityEngine;

namespace LapsEditModeTests {
    public class LapsRigidbody2DTests {
        private LapsRigidbody2D comp2D;
        [SetUp]
        public void Setup() {
            var go = new GameObject();
            go.AddComponent<Rigidbody2D>();
            comp2D = go.AddComponent<LapsRigidbody2D>();
        }
        [Test]
        public void LapsRigidbodyReturnsTheRigidbodyOfTheObject() {
            var coll2D = comp2D.GetComponent<Rigidbody2D>();
            var r2d = comp2D.HandleInput(0, null, null);
            Assert.AreEqual(coll2D, r2d);
            var trans2d = comp2D.transform;
            var t2d = comp2D.HandleInput(1, null, null);
            Assert.AreEqual(trans2d, t2d);
        }
        [Test]
        public void Slots() {
            var slots = new SlotList();
            comp2D.GetInputSlots(slots);
            Assert.AreEqual(new LogicSlot("body", 0, null, typeof(Rigidbody2D)), slots[0]);
            Assert.AreEqual(new LogicSlot("transform", 1, null, typeof(Transform)), slots[1]);
            slots.Clear();
            comp2D.GetOutputSlots(slots);
            Assert.AreEqual(0, slots.Count);
        }
    }
}
