using System.Collections;
using System.Collections.Generic;
using LapsEditModeTests;
using LapsEditor;
using LapsRuntime;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEngine.UIElements;

namespace LapsPlayModeTests {
    public class Collider2DTests {
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
            Assert.AreEqual(Collider2DListenerComponent.FilterMode.AcceptAny, listener.filterMode);
        }
        [UnityTest]
        public IEnumerator FilterModeAny() {
            Assert.AreEqual(true, false);
            yield return null;
        }
        [UnityTest]
        public IEnumerator FilterModeCollider() {
            Assert.AreEqual(true, false);
            yield return null;
        }
        [UnityTest]
        public IEnumerator FilterModeTrigger() {
            Assert.AreEqual(true, false);
            yield return null;
        }
        [UnityTest]
        public IEnumerator EnabledOnAwake() {
            Assert.AreEqual(true, false);
            yield return null;
        }
        [UnityTest]
        public IEnumerator EnableInput() {
            Assert.AreEqual(true, false);
            yield return null;
        }
        [UnityTest]
        public IEnumerator DisableInput() {
            Assert.AreEqual(true, false);
            yield return null;
        }
        [Test]
        public void Slots() {
            Assert.AreEqual(true, false);
        }
    }
}
