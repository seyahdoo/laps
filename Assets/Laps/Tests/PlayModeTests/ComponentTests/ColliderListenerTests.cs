using System.Collections;
using LapsEditModeTests;
using LapsEditor;
using LapsRuntime;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
// ReSharper disable Unity.InefficientPropertyAccess

namespace LapsPlayModeTests {
    public class ColliderListenerTests {
        public ColliderListenerComponent listener;
        public BoxCollider colliderOne;
        public BoxCollider colliderTwo;
        public TestComponent testEnter;
        public TestComponent testExit;
        [SetUp]
        public void Setup() {
            var cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
            listener = cube.AddComponent<ColliderListenerComponent>();
            listener.transform.position = Vector3.zero;

            cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
            cube.AddComponent<Rigidbody>();
            colliderOne = cube.GetComponent<BoxCollider>();
            colliderOne.transform.position = Vector3.right * 5f;

            cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
            cube.AddComponent<Rigidbody>();
            colliderTwo = cube.GetComponent<BoxCollider>();
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
        public IEnumerator OutputsAreCorrect() {
            colliderTwo.isTrigger = true;
            colliderOne.transform.position = Vector3.zero;
            yield return new WaitForFixedUpdate();
            yield return new WaitForFixedUpdate();
            Assert.AreEqual(1, testEnter.inputCallCount);
            Assert.AreEqual(colliderOne.attachedRigidbody, testEnter.inputList[0]);

            colliderTwo.transform.position = Vector3.zero;
            yield return new WaitForFixedUpdate();
            yield return new WaitForFixedUpdate();
            Assert.AreEqual(2, testEnter.inputCallCount);
            Assert.AreEqual(colliderTwo.attachedRigidbody, testEnter.inputList[1]);
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
            Assert.AreEqual(ColliderListenerComponent.ActivationMode.ActivateAlways, listener.activationMode);
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
            listener.activationMode = ColliderListenerComponent.ActivationMode.ActivateOnce;
            Assert.AreEqual(0, testEnter.inputCallCount);
            colliderOne.transform.position = Vector3.zero;
            colliderTwo.transform.position = Vector3.zero;
            yield return new WaitForFixedUpdate();
            yield return new WaitForFixedUpdate();
            Assert.AreEqual(1, testEnter.inputCallCount);
        }
        [UnityTest]
        public IEnumerator ActivationModeCumulative() {
            listener.activationMode = ColliderListenerComponent.ActivationMode.ActivateCumulatively;
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
            Assert.AreEqual(ColliderListenerComponent.TypeFilterMode.AcceptAny, listener.typeFilterMode);
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
            listener.typeFilterMode = ColliderListenerComponent.TypeFilterMode.AcceptCollider;
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
            listener.typeFilterMode = ColliderListenerComponent.TypeFilterMode.AcceptTrigger;
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
            var slots = new SlotList();
            listener.GetInputSlots(slots);
            Assert.AreEqual(2, slots.Count);
            Assert.AreEqual(new LogicSlot("enable", 0), slots[0]);
            Assert.AreEqual(new LogicSlot("disable", 1), slots[1]);
            slots.Clear();
            listener.GetOutputSlots(slots);
            Assert.AreEqual(2, slots.Count);
            Assert.AreEqual(new LogicSlot("object entered", 0, typeof(Rigidbody)), slots[0]);
            Assert.AreEqual(new LogicSlot("object exited", 1, typeof(Rigidbody)), slots[1]);
        }
    }
}
