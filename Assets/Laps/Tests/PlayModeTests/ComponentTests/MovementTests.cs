using LapsRuntime;
using NUnit.Framework;
using UnityEngine;

namespace LapsPlayModeTests {
    public class MovementTests {
        public Movement movement;
        [SetUp]
        public void Setup() {
            movement = new GameObject().AddComponent<Movement>();
        }
        [TearDown]
        public void Teardown() {
            Object.DestroyImmediate(movement.gameObject);
        }
        [Test]
        public void Slots() {
            var slots = new SlotList();
            movement.GetInputSlots(slots);
            Assert.AreEqual(7, slots.Count);
            Assert.AreEqual(new LogicSlot("set body", 0), slots[0]);
            Assert.AreEqual(new LogicSlot("go forwards", 1), slots[1]);
            Assert.AreEqual(new LogicSlot("go backwards", 2), slots[2]);
            Assert.AreEqual(new LogicSlot("set speed", 3, typeof(float)), slots[3]);
            Assert.AreEqual(new LogicSlot("stop", 4), slots[4]);
            Assert.AreEqual(new LogicSlot("teleport forward end", 5), slots[5]);
            Assert.AreEqual(new LogicSlot("teleport backward end", 6), slots[6]);
            slots.Clear();
            movement.GetOutputSlots(slots);
            Assert.AreEqual(3, slots.Count);
            Assert.AreEqual(new LogicSlot("body", 0), slots[0]);
            Assert.AreEqual(new LogicSlot("forwards finished", 1), slots[1]);
            Assert.AreEqual(new LogicSlot("backwards finished", 2), slots[2]);
        }
        [Test]
        public void GoForwards() {
            Assert.Fail("path editor has to be done first");
        }
    }
}
