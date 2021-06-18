using LapsRuntime;
using NUnit.Framework;
using UnityEngine;

namespace LapsPlayModeTests {
    public class Rigidbody2DOperationsTests {
        public Rigidbody2DOperations operations;
        [SetUp]
        public void Setup() {
            operations = new GameObject().AddComponent<Rigidbody2DOperations>();
        }
        [TearDown]
        public void Teardown() {
            Object.DestroyImmediate(operations.gameObject);
        }
        [Test]
        public void Slots() {
            var slots = new SlotList();
            operations.GetInputSlots(slots);
            Assert.AreEqual(5, slots.Count);
            Assert.AreEqual(new LogicSlot("set body", 0, typeof(Rigidbody2D)), slots[0]);
            Assert.AreEqual(new LogicSlot("get position", 1, null, typeof(Vector3)), slots[1]);
            Assert.AreEqual(new LogicSlot("set position", 2, typeof(Vector3)), slots[2]);
            Assert.AreEqual(new LogicSlot("get velocity", 3, null, typeof(Vector3)), slots[3]);
            Assert.AreEqual(new LogicSlot("set velocity", 4, typeof(Vector3)), slots[4]);
            slots.Clear();
            operations.GetOutputSlots(slots);
            Assert.AreEqual(1, slots.Count);
            Assert.AreEqual(new LogicSlot("body", 0, typeof(Rigidbody2D)), slots[0]);
        }
    }
}
