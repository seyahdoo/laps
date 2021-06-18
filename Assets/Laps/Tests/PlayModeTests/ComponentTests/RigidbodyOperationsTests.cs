using LapsEditor;
using LapsRuntime;
using NUnit.Framework;
using UnityEngine;

namespace LapsPlayModeTests {
    public class RigidbodyOperationsTests {
        public RigidbodyOperations operations;
        public LapsRigidbody body;
        public Rigidbody rigidbody;
        [SetUp]
        public void Setup() {
            operations = new GameObject().AddComponent<RigidbodyOperations>();
            body = new GameObject().AddComponent<LapsRigidbody>();
            rigidbody = body.GetComponent<Rigidbody>();
        }
        [TearDown]
        public void Teardown() {
            Object.DestroyImmediate(operations.gameObject);
        }
        [Test]
        public void SetBodyWithOutput() {
            Assert.AreEqual(null, operations.CurrentBody);
            LogicModule.Connect(operations, 0, body, 0);
            operations.Awake();
            Assert.AreEqual(rigidbody, operations.CurrentBody);
        }
        [Test]
        public void SetBody() {
            Assert.AreEqual(null, operations.CurrentBody);
            operations.HandleInput(0, body.GetComponent<Rigidbody>(), null);
            Assert.AreEqual(rigidbody, operations.CurrentBody);
        }
        [Test]
        public void GetPosition() {
            SetBody();
            var pos = new Vector3(887, 223, 42);
            rigidbody.position = pos;
            var gotPosition = (Vector3) operations.HandleInput(1, null, null);
            Assert.AreEqual(pos, gotPosition);
        }
        [Test]
        public void SetPosition() {
            SetBody();
            var pos = new Vector3(887, 223, 42);
            rigidbody.position = Vector2.zero;
            operations.HandleInput(2, (Vector3) pos, null);
            Assert.AreEqual(pos, rigidbody.position);
        }
        [Test]
        public void GetVelocity() {
            SetBody();
            var velocity = new Vector3(887, 223, 42);
            rigidbody.velocity = velocity;
            var gotVelocity = (Vector3) operations.HandleInput(3, null, null);
            Assert.AreEqual(velocity, gotVelocity);
        }
        [Test]
        public void SetVelocity() {
            SetBody();
            var velocity = new Vector3(887, 223, 42);
            rigidbody.velocity = Vector2.zero;
            operations.HandleInput(4, (Vector3) velocity, null);
            Assert.AreEqual(velocity, rigidbody.velocity);
        }
        [Test]
        public void Slots() {
            var slots = new SlotList();
            operations.GetInputSlots(slots);
            Assert.AreEqual(5, slots.Count);
            Assert.AreEqual(new LogicSlot("set body", 0, typeof(Rigidbody)), slots[0]);
            Assert.AreEqual(new LogicSlot("get position", 1, null, typeof(Vector3)), slots[1]);
            Assert.AreEqual(new LogicSlot("set position", 2, typeof(Vector3)), slots[2]);
            Assert.AreEqual(new LogicSlot("get velocity", 3, null, typeof(Vector3)), slots[3]);
            Assert.AreEqual(new LogicSlot("set velocity", 4, typeof(Vector3)), slots[4]);
            slots.Clear();
            operations.GetOutputSlots(slots);
            Assert.AreEqual(1, slots.Count);
            Assert.AreEqual(new LogicSlot("body", 0, typeof(Rigidbody)), slots[0]);
        }
    }
}
