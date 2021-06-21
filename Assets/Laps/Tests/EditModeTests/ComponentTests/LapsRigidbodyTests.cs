using LapsRuntime;
using NUnit.Framework;
using UnityEngine;

namespace LapsEditModeTests {
    public class LapsRigidbodyTests {
        private LapsRigidbody comp3D;
        [SetUp]
        public void Setup() {
            var go = new GameObject();
            go.AddComponent<Rigidbody>();
            comp3D = go.AddComponent<LapsRigidbody>();
        }
        [TearDown]
        public void Teardown() {
            Object.DestroyImmediate(comp3D.gameObject);
        }
        [Test]
        public void Body() {
            var body = comp3D.GetComponent<Rigidbody>();
            var r3d = comp3D.HandleInput(0, null, null);
            Assert.AreEqual(body, r3d);
        }
        [Test]
        public void Transform() {
            var trans3d = comp3D.transform;
            var t3d = comp3D.HandleInput(1, null, null);
            Assert.AreEqual(trans3d, t3d);
        }
        [Test]
        public void Enable() {
            comp3D.gameObject.SetActive(false);
            comp3D.HandleInput(2, null, null);
            Assert.AreEqual(true, comp3D.gameObject.activeSelf);
        }
        [Test]
        public void Disable() {
            comp3D.gameObject.SetActive(true);
            comp3D.HandleInput(3, null, null);
            Assert.AreEqual(false, comp3D.gameObject.activeSelf);
        }
        [Test]
        public void SetKinematic() {
            var body = comp3D.GetComponent<Rigidbody>();
            body.isKinematic = false;
            comp3D.HandleInput(4, null, null);
            Assert.AreEqual(true, body.isKinematic);
        }
        [Test]
        public void SetDynamic() {
            var body = comp3D.GetComponent<Rigidbody>();
            body.isKinematic = true;
            comp3D.HandleInput(5, null, null);
            Assert.AreEqual(false, body.isKinematic);
        }
        [Test]
        public void Slots() {
            var slots = new SlotList();
            comp3D.GetInputSlots(slots);
            Assert.AreEqual(6, slots.Count);
            Assert.AreEqual(new LogicSlot("body", 0, null, typeof(Rigidbody)), slots[0]);
            Assert.AreEqual(new LogicSlot("transform", 1, null, typeof(Transform)), slots[1]);
            Assert.AreEqual(new LogicSlot("enable", 2), slots[2]);
            Assert.AreEqual(new LogicSlot("disable", 3), slots[3]);
            Assert.AreEqual(new LogicSlot("set kinematic", 4), slots[4]);
            Assert.AreEqual(new LogicSlot("set dynamic", 5), slots[5]);
            slots.Clear();
            comp3D.GetOutputSlots(slots);
            Assert.AreEqual(0, slots.Count);
        }
    }
}
