using System.Collections.Generic;
using LapsRuntime;
using NUnit.Framework;
using UnityEngine;

namespace LapsEditModeTests {
    public class PathTests {
        public Path path;
        [SetUp]
        public void Setup() {
            path = new Path();
        }
        [TearDown]
        public void Teardown() {
            path = null;
        }
        [Test]
        public void PathEnumeratorCreate() {
            var e = path.GetEnumerator();
        }
        [Test]
        public void DefaultPathIsValid() {
            Assert.IsTrue(path.IsValid);
        }
        [Test]
        public void PathWithZeroPointIsInvalid() {
            path.points.Clear();
            Assert.IsFalse(path.IsValid);
        }
        [Test]
        public void PathWithOnePointIsInvalid() {
            path.points.Clear();
            path.points.Add(new Vector3());
            Assert.IsFalse(path.IsValid);
        }
        [Test]
        public void PathWithTwoPointIsInvalid() {
            path.points.Clear();
            path.points.Add(new Vector3());
            path.points.Add(new Vector3());
            Assert.IsTrue(path.IsValid);
        }
        [Test]
        public void DefaultValuesAreCorrect() {
            Assert.AreEqual(new List<Vector3>(){Vector3.zero, Vector3.right}, path.points);
            Assert.AreEqual(false, path.closed);
        }
        [Test]
        public void MoveToNextPoint() {
            path.points = new List<Vector3>() {Vector3.zero, Vector3.right, Vector3.right * 2f};
            var e = path.GetEnumerator();
            Assert.AreEqual(0, e.PreviousPointIndex);
            Assert.AreEqual(0, e.CurrentPointIndex);
            Assert.AreEqual(1, e.NextPointIndex);
            Assert.IsTrue(e.GoToNextPoint());
            Assert.AreEqual(0, e.PreviousPointIndex);
            Assert.AreEqual(1, e.CurrentPointIndex);
            Assert.AreEqual(2, e.NextPointIndex);
            Assert.IsTrue(e.GoToNextPoint());
            Assert.AreEqual(1, e.PreviousPointIndex);
            Assert.AreEqual(2, e.CurrentPointIndex);
            Assert.AreEqual(2, e.NextPointIndex);
            Assert.IsFalse(e.GoToNextPoint());
            Assert.AreEqual(1, e.PreviousPointIndex);
            Assert.AreEqual(2, e.CurrentPointIndex);
            Assert.AreEqual(2, e.NextPointIndex);
        }
        [Test]
        public void MoveToPreviousPoint() {
            path.points = new List<Vector3>() {Vector3.zero, Vector3.right, Vector3.right * 2f};
            var e = path.GetEnumerator();
            Assert.AreEqual(0, e.PreviousPointIndex);
            Assert.AreEqual(0, e.CurrentPointIndex);
            Assert.AreEqual(1, e.NextPointIndex);
            Assert.IsFalse(e.GoToPreviousPoint());
            Assert.IsTrue(e.GoToNextPoint());
            Assert.IsTrue(e.GoToNextPoint());
            Assert.AreEqual(1, e.PreviousPointIndex);
            Assert.AreEqual(2, e.CurrentPointIndex);
            Assert.AreEqual(2, e.NextPointIndex);
            Assert.IsTrue(e.GoToPreviousPoint());
            Assert.AreEqual(0, e.PreviousPointIndex);
            Assert.AreEqual(1, e.CurrentPointIndex);
            Assert.AreEqual(2, e.NextPointIndex);
            Assert.IsTrue(e.GoToPreviousPoint());
            Assert.AreEqual(0, e.PreviousPointIndex);
            Assert.AreEqual(0, e.CurrentPointIndex);
            Assert.AreEqual(1, e.NextPointIndex);
            Assert.IsFalse(e.GoToPreviousPoint());
            Assert.AreEqual(0, e.PreviousPointIndex);
            Assert.AreEqual(0, e.CurrentPointIndex);
            Assert.AreEqual(1, e.NextPointIndex);
        }
        [Test] public void MoveForwardGiganticAmound() { Assert.Fail(); }
        [Test] public void MoveForwardNormalAmound() { Assert.Fail(); }
        [Test] public void MoveBackwardGiganticAmound() { Assert.Fail(); }
        [Test] public void MoveBackwardNormalAmound() { Assert.Fail(); }
        [Test] public void MoveForwardOnLastPoint() { Assert.Fail(); }
        [Test] public void MoveBackwardOnFirstPoint() { Assert.Fail(); }
        [Test] public void MoveForwardsSkippingTwoPoints() { Assert.Fail(); }
        [Test] public void MoveBackwardsSkippingTwoPoints() { Assert.Fail(); }
        [Test] public void EnumeratorReset() { Assert.Fail(); }
        // [Test] public void Test() { Assert.Fail(); }
    }
}
