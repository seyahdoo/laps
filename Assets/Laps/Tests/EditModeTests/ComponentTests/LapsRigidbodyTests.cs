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
        [Test]
        public void LapsRigidbodyReturnsTheRigidbodyOfTheObject() {
            var coll3D = comp3D.GetComponent<Rigidbody>();
            var r3d = comp3D.HandleInput(0, null, null);
            Assert.AreEqual(coll3D, r3d);
            var trans3d = comp3D.transform;
            var t3d = comp3D.HandleInput(1, null, null);
            Assert.AreEqual(trans3d, t3d);
        }
        [Test]
        public void Slots() {
            var slots = new SlotList();
            comp3D.GetInputSlots(slots);
            Assert.AreEqual(new LogicSlot("body", 0, null, typeof(Rigidbody)), slots[0]);
            Assert.AreEqual(new LogicSlot("transform", 1, null, typeof(Transform)), slots[1]);
            slots.Clear();
            comp3D.GetOutputSlots(slots);
            Assert.AreEqual(0, slots.Count);
        }
    }
}
