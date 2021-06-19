using System.Collections;
using System.Numerics;
using LapsRuntime;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;

namespace LapsPlayModeTests {
    public class Movement2DTests {
        public Movement2D movement;
        public Rigidbody2D body;
        [SetUp]
        public void Setup() {
            movement = new GameObject().AddComponent<Movement2D>();
            movement.path.points.Clear();
            movement.path.points.Add(Vector3.zero);
            movement.path.points.Add(new Vector3(1,0,0));
            movement.path.points.Add(new Vector3(1,1,0));
            movement.path.points.Add(new Vector3(2,1,0));
            var go = GameObject.CreatePrimitive(PrimitiveType.Cube);
            Object.DestroyImmediate(go.GetComponent<BoxCollider>());
            body = go.AddComponent<Rigidbody2D>();
            movement.body = this.body;
            movement.Awake();
        }
        [TearDown]
        public void Teardown() {
            Object.DestroyImmediate(movement.gameObject);
            Object.DestroyImmediate(body.gameObject);
        }
        [Test]
        public void Slots() {
            var slots = new SlotList();
            movement.GetInputSlots(slots);
            Assert.AreEqual(7, slots.Count);
            Assert.AreEqual(new LogicSlot("set body", 0, typeof(Rigidbody2D)), slots[0]);
            Assert.AreEqual(new LogicSlot("go forwards", 1), slots[1]);
            Assert.AreEqual(new LogicSlot("go backwards", 2), slots[2]);
            Assert.AreEqual(new LogicSlot("set speed", 3, typeof(float)), slots[3]);
            Assert.AreEqual(new LogicSlot("stop", 4), slots[4]);
            Assert.AreEqual(new LogicSlot("teleport forward end", 5), slots[5]);
            Assert.AreEqual(new LogicSlot("teleport backward end", 6), slots[6]);
            slots.Clear();
            movement.GetOutputSlots(slots);
            Assert.AreEqual(3, slots.Count);
            Assert.AreEqual(new LogicSlot("body", 0, null, typeof(Rigidbody2D)), slots[0]);
            Assert.AreEqual(new LogicSlot("forwards finished", 1), slots[1]);
            Assert.AreEqual(new LogicSlot("backwards finished", 2), slots[2]);
        }
        [UnityTest]
        public IEnumerator GoForwards() {
            var speed = 1f;
            movement.SetSpeed(speed);
            movement.TeleportBackwardEnd();
            Assert.IsTrue(LapsMath.ApproximatelyClose(Vector2.zero, body.position));
            movement.GoForwards();
            var startFixedTime = Time.fixedTime;
            yield return new WaitForSeconds(1.5f);
            var elapsedFixedTime = Time.fixedTime - startFixedTime;
            var movementAmount = elapsedFixedTime * speed;
            var e = movement.path.GetEnumerator();
            e.GoForward(movementAmount);
            var expectedPosition = (Vector2) e.CurrentPosition;
            Assert.IsTrue(Vector2.Distance(expectedPosition, body.position) < 0.1f);
        }
    }
}
