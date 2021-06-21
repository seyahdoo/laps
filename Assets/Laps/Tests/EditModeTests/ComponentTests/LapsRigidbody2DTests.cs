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
        [TearDown]
        public void Teardown() {
            Object.DestroyImmediate(comp2D.gameObject);
        }
        [Test]
        public void Body() {
            var body = comp2D.GetComponent<Rigidbody2D>();
            var r2d = comp2D.HandleInput(0, null, null);
            Assert.AreEqual(body, r2d);
        }
        [Test]
        public void Transform() {
            var trans2d = comp2D.transform;
            var t2d = comp2D.HandleInput(1, null, null);
            Assert.AreEqual(trans2d, t2d);
        }
        [Test]
        public void Enable() {
            comp2D.gameObject.SetActive(false);
            comp2D.HandleInput(2, null, null);
            Assert.AreEqual(true, comp2D.gameObject.activeSelf);
        }
        [Test]
        public void Disable() {
            comp2D.gameObject.SetActive(true);
            comp2D.HandleInput(3, null, null);
            Assert.AreEqual(false, comp2D.gameObject.activeSelf);
        }
        [Test]
        public void SetKinematic() {
            var body = comp2D.GetComponent<Rigidbody2D>();
            body.isKinematic = false;
            comp2D.HandleInput(4, null, null);
            Assert.AreEqual(true, body.isKinematic);
        }
        [Test]
        public void SetDynamic() {
            var body = comp2D.GetComponent<Rigidbody2D>();
            body.isKinematic = true;
            comp2D.HandleInput(5, null, null);
            Assert.AreEqual(false, body.isKinematic);
        }
        [Test]
        public void Slots() {
            var slots = new SlotList();
            comp2D.GetInputSlots(slots);
            Assert.AreEqual(6, slots.Count);
            Assert.AreEqual(new LogicSlot("body", 0, null, typeof(Rigidbody2D)), slots[0]);
            Assert.AreEqual(new LogicSlot("transform", 1, null, typeof(Transform)), slots[1]);
            Assert.AreEqual(new LogicSlot("enable", 2), slots[2]);
            Assert.AreEqual(new LogicSlot("disable", 3), slots[3]);
            Assert.AreEqual(new LogicSlot("set kinematic", 4), slots[4]);
            Assert.AreEqual(new LogicSlot("set dynamic", 5), slots[5]);
            slots.Clear();
            comp2D.GetOutputSlots(slots);
            Assert.AreEqual(0, slots.Count);
        }
    }
}
