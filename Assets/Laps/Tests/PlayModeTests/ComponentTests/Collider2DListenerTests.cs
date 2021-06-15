using System.Collections;
using System.Collections.Generic;
using LapsEditModeTests;
using LapsEditor;
using LapsRuntime;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEngine.UIElements;
// ReSharper disable Unity.InefficientPropertyAccess

namespace LapsPlayModeTests {
    public class Collider2DListenerTests {
        public Collider2DListenerComponent listener;
        public BoxCollider2D colliderOne;
        public BoxCollider2D colliderTwo;
        public TestComponent testEnter;
        public TestComponent testExit;
        [SetUp]
        public void Setup() {
            var cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
            Object.DestroyImmediate(cube.GetComponent<BoxCollider>());
            cube.AddComponent<BoxCollider2D>();
            listener = cube.AddComponent<Collider2DListenerComponent>();
            listener.transform.position = Vector3.zero;

            cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
            Object.DestroyImmediate(cube.GetComponent<BoxCollider>());
            var bodyOne = cube.AddComponent<Rigidbody2D>();
            colliderOne = cube.AddComponent<BoxCollider2D>();
            colliderOne.transform.position = Vector3.right * 5f;

            cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
            Object.DestroyImmediate(cube.GetComponent<BoxCollider>());
            var bodyTwo = cube.AddComponent<Rigidbody2D>();
            colliderTwo = cube.AddComponent<BoxCollider2D>();
            colliderTwo.transform.position = Vector3.right * 10f;

            testEnter = new GameObject().AddComponent<TestComponent>();
            LogicModule.Connect(listener, 0, testEnter, 0);

            testExit = new GameObject().AddComponent<TestComponent>();
            LogicModule.Connect(listener, 1, testExit, 0);
        }
        [TearDown]
        public void Teardown() {
            Object.DestroyImmediate(listener.gameObject);
            Object.DestroyImmediate(colliderOne.gameObject);
            Object.DestroyImmediate(colliderTwo.gameObject);
            Object.DestroyImmediate(testEnter.gameObject);
            Object.DestroyImmediate(testExit.gameObject);
        }
        [UnityTest]
        public IEnumerator LayerMask() {
            listener.layerMask = UnityEngine.LayerMask.GetMask("Water");
            colliderOne.gameObject.layer = UnityEngine.LayerMask.NameToLayer("Water");
            colliderTwo.gameObject.layer = UnityEngine.LayerMask.NameToLayer("Default");
            colliderOne.transform.position = Vector3.zero;
            yield return new WaitForFixedUpdate();
            yield return new WaitForFixedUpdate();
            Assert.AreEqual(1, testEnter.inputCallCount);

            colliderTwo.transform.position = Vector3.zero;
            yield return new WaitForFixedUpdate();
            yield return new WaitForFixedUpdate();
            Assert.AreEqual(1, testEnter.inputCallCount);
        }
        [Test]
        public void DefaultActivationModeIsAlways() {
            Assert.AreEqual(Collider2DListenerComponent.ActivationMode.ActivateAlways, listener.activationMode);
        }
        [UnityTest]
        public IEnumerator ActivationModeAlways() {
            yield return new WaitForFixedUpdate();
            yield return new WaitForFixedUpdate();
            Assert.AreEqual(0, testEnter.inputCallCount);
            colliderOne.transform.position = Vector3.zero;
            colliderTwo.transform.position = Vector3.zero;
            yield return new WaitForFixedUpdate();
            yield return new WaitForFixedUpdate();
            Assert.AreEqual(2, testEnter.inputCallCount);
        }
        [UnityTest]
        public IEnumerator ActivationModeOnce() {
            listener.activationMode = Collider2DListenerComponent.ActivationMode.ActivateOnce;
            Assert.AreEqual(0, testEnter.inputCallCount);
            colliderOne.transform.position = Vector3.zero;
            colliderTwo.transform.position = Vector3.zero;
            yield return new WaitForFixedUpdate();
            yield return new WaitForFixedUpdate();
            Assert.AreEqual(1, testEnter.inputCallCount);
        }
        [UnityTest]
        public IEnumerator ActivationModeCumulative() {
            listener.activationMode = Collider2DListenerComponent.ActivationMode.ActivateCumulatively;
            Assert.AreEqual(0, testEnter.inputCallCount);
            colliderOne.transform.position = Vector3.zero;
            yield return new WaitForFixedUpdate();
            yield return new WaitForFixedUpdate();
            Assert.AreEqual(1, testEnter.inputCallCount);
            Assert.AreEqual(0, testExit.inputCallCount);
            colliderTwo.transform.position = Vector3.zero;
            yield return new WaitForFixedUpdate();
            yield return new WaitForFixedUpdate();
            Assert.AreEqual(1, testEnter.inputCallCount);
            Assert.AreEqual(0, testExit.inputCallCount);
        }
        [Test]
        public void DefaultFilterModeIsAcceptAny() {
            Assert.AreEqual(Collider2DListenerComponent.TypeFilterMode.AcceptAny, listener.typeFilterMode);
        }
        [UnityTest]
        public IEnumerator FilterModeAny() {
            yield return new WaitForFixedUpdate();
            yield return new WaitForFixedUpdate();
            Assert.AreEqual(0, testEnter.inputCallCount);
            colliderOne.transform.position = Vector3.zero;
            colliderTwo.transform.position = Vector3.zero;
            colliderTwo.isTrigger = true;
            yield return new WaitForFixedUpdate();
            yield return new WaitForFixedUpdate();
            Assert.AreEqual(2, testEnter.inputCallCount);
        }
        [UnityTest]
        public IEnumerator FilterModeCollider() {
            listener.typeFilterMode = Collider2DListenerComponent.TypeFilterMode.AcceptCollider;
            yield return new WaitForFixedUpdate();
            yield return new WaitForFixedUpdate();
            Assert.AreEqual(0, testEnter.inputCallCount);
            colliderOne.transform.position = Vector3.zero;
            colliderTwo.transform.position = Vector3.zero;
            colliderTwo.isTrigger = true;
            yield return new WaitForFixedUpdate();
            yield return new WaitForFixedUpdate();
            Assert.AreEqual(1, testEnter.inputCallCount);
            Assert.AreEqual(colliderOne.attachedRigidbody, testEnter.inputList[0]);
            colliderOne.transform.position = Vector3.right * 10f; 
            colliderTwo.transform.position = Vector3.right * 12f;
            yield return new WaitForFixedUpdate();
            yield return new WaitForFixedUpdate();
            Assert.AreEqual(1, testExit.inputCallCount);
            Assert.AreEqual(colliderOne.attachedRigidbody, testExit.inputList[0]);
        }
        [UnityTest]
        public IEnumerator FilterModeTrigger() {
            listener.typeFilterMode = Collider2DListenerComponent.TypeFilterMode.AcceptTrigger;
            yield return new WaitForFixedUpdate();
            yield return new WaitForFixedUpdate();
            Assert.AreEqual(0, testEnter.inputCallCount);
            colliderOne.transform.position = Vector3.zero;
            colliderTwo.transform.position = Vector3.zero;
            colliderTwo.isTrigger = true;
            yield return new WaitForFixedUpdate();
            yield return new WaitForFixedUpdate();
            Assert.AreEqual(1, testEnter.inputCallCount);
            Assert.AreEqual(colliderTwo.attachedRigidbody, testEnter.inputList[0]);
            colliderOne.transform.position = Vector3.right * 10f; 
            colliderTwo.transform.position = Vector3.right * 12f;
            yield return new WaitForFixedUpdate();
            yield return new WaitForFixedUpdate();
            Assert.AreEqual(1, testExit.inputCallCount);
            Assert.AreEqual(colliderTwo.attachedRigidbody, testExit.inputList[0]);
        }
        [UnityTest]
        public IEnumerator EnabledOnAwake() {
            listener.enabledOnAwake = false;
            listener.Awake();
            yield return new WaitForFixedUpdate();
            yield return new WaitForFixedUpdate();
            colliderOne.transform.position = Vector3.zero;
            colliderTwo.transform.position = Vector3.zero;
            colliderTwo.isTrigger = true;
            yield return new WaitForFixedUpdate();
            yield return new WaitForFixedUpdate();
            colliderOne.transform.position = Vector3.right * 10f; 
            colliderTwo.transform.position = Vector3.right * 12f;
            yield return new WaitForFixedUpdate();
            yield return new WaitForFixedUpdate();
            Assert.AreEqual(0, testExit.inputCallCount);
            Assert.AreEqual(0, testEnter.inputCallCount);
        }
        [UnityTest]
        public IEnumerator EnableInput() {
            listener.enabledOnAwake = false;
            listener.Awake();
            listener.HandleInput(0, null, null);
            yield return new WaitForFixedUpdate();
            yield return new WaitForFixedUpdate();
            colliderOne.transform.position = Vector3.zero;
            colliderTwo.transform.position = Vector3.zero;
            colliderTwo.isTrigger = true;
            yield return new WaitForFixedUpdate();
            yield return new WaitForFixedUpdate();
            colliderOne.transform.position = Vector3.right * 10f; 
            colliderTwo.transform.position = Vector3.right * 12f;
            yield return new WaitForFixedUpdate();
            yield return new WaitForFixedUpdate();
            Assert.AreEqual(2, testExit.inputCallCount);
            Assert.AreEqual(2, testEnter.inputCallCount);
        }
        [UnityTest]
        public IEnumerator DisableInput() {
            listener.enabledOnAwake = true;
            listener.Awake();
            listener.HandleInput(1, null, null);
            yield return new WaitForFixedUpdate();
            yield return new WaitForFixedUpdate();
            colliderOne.transform.position = Vector3.zero;
            colliderTwo.transform.position = Vector3.zero;
            colliderTwo.isTrigger = true;
            yield return new WaitForFixedUpdate();
            yield return new WaitForFixedUpdate();
            colliderOne.transform.position = Vector3.right * 10f; 
            colliderTwo.transform.position = Vector3.right * 12f;
            yield return new WaitForFixedUpdate();
            yield return new WaitForFixedUpdate();
            Assert.AreEqual(0, testExit.inputCallCount);
            Assert.AreEqual(0, testEnter.inputCallCount);
        }
        [Test]
        public void Slots() {
            var slots = new List<LogicSlot>();
            listener.GetInputSlots(slots);
            Assert.AreEqual(2, slots.Count);
            Assert.AreEqual(new LogicSlot("enable", 0), slots[0]);
            Assert.AreEqual(new LogicSlot("disable", 1), slots[1]);
            slots.Clear();
            listener.GetOutputSlots(slots);
            Assert.AreEqual(2, slots.Count);
            Assert.AreEqual(new LogicSlot("object entered", 0, typeof(Rigidbody2D)), slots[0]);
            Assert.AreEqual(new LogicSlot("object exited", 1, typeof(Rigidbody2D)), slots[1]);
        }
    }
}
