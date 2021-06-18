using LapsEditor;
using LapsRuntime;
using NUnit.Framework;
using UnityEngine;

namespace LapsPlayModeTests {
    public class Rigidbody2DOperationsTests {
        public Rigidbody2DOperations operations;
        public LapsRigidbody2D body;
        public Rigidbody2D rigidbody2D;
        [SetUp]
        public void Setup() {
            operations = new GameObject().AddComponent<Rigidbody2DOperations>();
            body = new GameObject().AddComponent<LapsRigidbody2D>();
            rigidbody2D = body.GetComponent<Rigidbody2D>();
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
            Assert.AreEqual(rigidbody2D, operations.CurrentBody);
        }
        [Test]
        public void SetBody() {
            Assert.AreEqual(null, operations.CurrentBody);
            operations.HandleInput(0, body.GetComponent<Rigidbody2D>(), null);
            Assert.AreEqual(rigidbody2D, operations.CurrentBody);
        }
        [Test]
        public void GetPosition() {
            SetBody();
            var pos = new Vector2(887, 223);
            rigidbody2D.position = pos;
            var gotPosition = (Vector2) (Vector3) operations.HandleInput(1, null, null);
            Assert.AreEqual(pos, gotPosition);
        }
        [Test]
        public void SetPosition() {
            SetBody();
            var pos = new Vector2(887, 223);
            rigidbody2D.position = Vector2.zero;
            operations.HandleInput(2, (Vector3) pos, null);
            Assert.AreEqual(pos, rigidbody2D.position);
        }
        [Test]
        public void GetVelocity() {
            SetBody();
            var velocity = new Vector2(887, 223);
            rigidbody2D.velocity = velocity;
            var gotVelocity = (Vector2) (Vector3) operations.HandleInput(3, null, null);
            Assert.AreEqual(velocity, gotVelocity);
        }
        [Test]
        public void SetVelocity() {
            SetBody();
            var velocity = new Vector2(887, 223);
            rigidbody2D.velocity = Vector2.zero;
            operations.HandleInput(4, (Vector3) velocity, null);
            Assert.AreEqual(velocity, rigidbody2D.velocity);
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
