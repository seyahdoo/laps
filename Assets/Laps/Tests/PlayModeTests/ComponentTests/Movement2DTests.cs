using System.Collections;
using LapsEditModeTests;
using LapsEditor;
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
        public LapsRigidbody2D lapsBody;
        public TestComponent test;
        [SetUp]
        public void Setup() {
            movement = new GameObject().AddComponent<Movement2D>();
            movement.path.points.Clear();
            movement.path.points.Add(new Vector3(0,0,0));
            movement.path.points.Add(new Vector3(1,0,0));
            movement.path.points.Add(new Vector3(1,1,0));
            movement.path.points.Add(new Vector3(2,1,0));
            var go = GameObject.CreatePrimitive(PrimitiveType.Cube);
            Object.DestroyImmediate(go.GetComponent<BoxCollider>());
            body = go.AddComponent<Rigidbody2D>();
            lapsBody = go.AddComponent<LapsRigidbody2D>();
            movement.body = this.body;
            movement.Awake();
            test = new GameObject().AddComponent<TestComponent>();
        }
        [TearDown]
        public void Teardown() {
            Object.DestroyImmediate(movement.gameObject);
            Object.DestroyImmediate(body.gameObject);
            Object.DestroyImmediate(test.gameObject);
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
        [TestCase(1f, ExpectedResult = null)]
        [TestCase(2f, ExpectedResult = null)]
        public IEnumerator GoForwards(float speed) {
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
        [Test]
        public void TeleportToForwardEnd() {
            movement.TeleportForwardEnd();
            Assert.IsTrue(LapsMath.ApproximatelyClose((Vector2)movement.path.points[3], body.position));
        }
        [Test]
        public void TeleportToBackwardEnd() {
            movement.TeleportForwardEnd();
            Assert.IsTrue(LapsMath.ApproximatelyClose((Vector2)movement.path.points[3], body.position));
            movement.TeleportBackwardEnd();
            Assert.IsTrue(LapsMath.ApproximatelyClose((Vector2)movement.path.points[0], body.position));
        }
        [UnityTest]
        [TestCase(1f, ExpectedResult = null)]
        [TestCase(2f, ExpectedResult = null)]
        public IEnumerator GoBackwards(float speed) {
            movement.SetSpeed(speed);
            movement.TeleportForwardEnd();
            Assert.IsTrue(LapsMath.ApproximatelyClose((Vector2)movement.path.points[3], body.position));
            movement.GoBackwards();
            var startFixedTime = Time.fixedTime;
            yield return new WaitForSeconds(1.5f);
            var elapsedFixedTime = Time.fixedTime - startFixedTime;
            var movementAmount = elapsedFixedTime * speed;
            var e = movement.path.GetEnumerator();
            e.GoToEndPoint();
            e.GoBackward(movementAmount);
            var expectedPosition = (Vector2) e.CurrentPosition;
            Assert.IsTrue(Vector2.Distance(expectedPosition, body.position) < 0.1f);
        }
        [UnityTest]
        public IEnumerator Stop() {
            var speed = 1f;
            movement.SetSpeed(speed);
            movement.TeleportBackwardEnd();
            Assert.IsTrue(LapsMath.ApproximatelyClose(Vector2.zero, body.position));
            movement.GoForwards();
            var startFixedTime = Time.fixedTime;
            yield return new WaitForSeconds(0.5f);
            var elapsedFixedTime = Time.fixedTime - startFixedTime;
            var movementAmount = elapsedFixedTime * speed;
            var e = movement.path.GetEnumerator();
            e.GoForward(movementAmount);
            var expectedPosition = (Vector2) e.CurrentPosition;
            Assert.IsTrue(Vector2.Distance(expectedPosition, body.position) < 0.1f);
            movement.Stop();
            yield return new WaitForSeconds(0.5f);
            Assert.IsTrue(Vector2.Distance(expectedPosition, body.position) < 0.1f);
        }
        [Test]
        public void SetBodySlot() {
            movement.body = null;
            movement.HandleInput(0, body, null);
            Assert.AreEqual(body, movement.body);
        }
        [UnityTest]
        public IEnumerator GoForwardsSlot() {
            var speed = 3f;
            movement.SetSpeed(speed);
            movement.TeleportBackwardEnd();
            Assert.IsTrue(LapsMath.ApproximatelyClose(Vector2.zero, body.position));
            movement.HandleInput(1, null, null);
            var startFixedTime = Time.fixedTime;
            yield return new WaitForSeconds(1.5f);
            var elapsedFixedTime = Time.fixedTime - startFixedTime;
            var movementAmount = elapsedFixedTime * speed;
            var e = movement.path.GetEnumerator();
            e.GoForward(movementAmount);
            var expectedPosition = (Vector2) e.CurrentPosition;
            Assert.IsTrue(Vector2.Distance(expectedPosition, body.position) < 0.1f);
        }
        [UnityTest]
        public IEnumerator GoBackwardsSlot() {
            var speed = 3f;
            movement.SetSpeed(speed);
            movement.TeleportForwardEnd();
            Assert.IsTrue(LapsMath.ApproximatelyClose((Vector2)movement.path.points[3], body.position));
            movement.HandleInput(2, null, null);
            var startFixedTime = Time.fixedTime;
            yield return new WaitForSeconds(1.5f);
            var elapsedFixedTime = Time.fixedTime - startFixedTime;
            var movementAmount = elapsedFixedTime * speed;
            var e = movement.path.GetEnumerator();
            e.GoToEndPoint();
            e.GoBackward(movementAmount);
            var expectedPosition = (Vector2) e.CurrentPosition;
            Assert.IsTrue(Vector2.Distance(expectedPosition, body.position) < 0.1f);
        }
        [Test]
        [TestCase(1f, ExpectedResult = 1f)]
        [TestCase(2f, ExpectedResult = 2f)]
        [TestCase(-1f, ExpectedResult = -1f)]
        [TestCase(-5f, ExpectedResult = -5f)]
        public float SetSpeedSlot(float speed) {
            movement.HandleInput(3, speed, null);
            return movement.Speed;
        }
        [UnityTest]
        public IEnumerator StopSlot() {
            var speed = 1f;
            movement.SetSpeed(speed);
            movement.TeleportBackwardEnd();
            Assert.IsTrue(LapsMath.ApproximatelyClose(Vector2.zero, body.position));
            movement.GoForwards();
            var startFixedTime = Time.fixedTime;
            yield return new WaitForSeconds(0.5f);
            var elapsedFixedTime = Time.fixedTime - startFixedTime;
            var movementAmount = elapsedFixedTime * speed;
            var e = movement.path.GetEnumerator();
            e.GoForward(movementAmount);
            var expectedPosition = (Vector2) e.CurrentPosition;
            Assert.IsTrue(Vector2.Distance(expectedPosition, body.position) < 0.1f);
            movement.HandleInput(4, null, null);
            yield return new WaitForSeconds(0.5f);
            Assert.IsTrue(Vector2.Distance(expectedPosition, body.position) < 0.1f);
        }
        [Test]
        public void TeleportForwardsEndSlot() {
            movement.HandleInput(5, null, null);
            Assert.IsTrue(LapsMath.ApproximatelyClose((Vector2)movement.path.points[3], body.position));
        }
        [Test]
        public void TeleportBackwardsEndSlot() {
            movement.TeleportForwardEnd();
            Assert.IsTrue(LapsMath.ApproximatelyClose((Vector2)movement.path.points[3], body.position));
            movement.HandleInput(6, null, null);
            Assert.IsTrue(LapsMath.ApproximatelyClose((Vector2)movement.path.points[0], body.position));
        }
        [Test]
        public void BodyOutputAwakeSlot() {
            movement.body = null;
            LogicModule.Connect(movement, 0, lapsBody, 0);
            movement.Awake();
            Assert.AreEqual(body, movement.Body);
        }
        [Test]
        public void ForwardsFinishedOnTeleportSlot() {
            LogicModule.Connect(movement, 1, test, 0);
            movement.TeleportForwardEnd();
            Assert.AreEqual(1, test.inputCallCount);
        }
        [UnityTest]
        public IEnumerator ForwardsFinishedNormalSlot() {
            LogicModule.Connect(movement, 1, test, 0);
            movement.SetSpeed(4f);
            movement.GoForwards();
            yield return new WaitForSeconds(1f);
            Assert.AreEqual(1, test.inputCallCount);
        }
        [Test]
        public void BackwardsFinishedOnTeleportSlot() {
            LogicModule.Connect(movement, 2, test, 0);
            movement.TeleportBackwardEnd();
            Assert.AreEqual(1, test.inputCallCount);
        }
        [UnityTest]
        public IEnumerator BackwardsFinishedNormalSlot() {
            LogicModule.Connect(movement, 2, test, 0);
            movement.TeleportForwardEnd();
            movement.SetSpeed(4f);
            movement.GoBackwards();
            yield return new WaitForSeconds(1f);
            Assert.AreEqual(1, test.inputCallCount);
        }
    }
}
