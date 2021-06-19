using System.Collections;
using LapsRuntime;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

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
        public void PathWithZeroPointIsInvalid() {
            Assert.IsFalse(path.IsValid);
        }
        [Test]
        public void PathWithOnePointIsInvalid() {
            path.points.Add(new Vector3());
            Assert.IsFalse(path.IsValid);
        }
        [Test]
        public void PathWithTwoPointIsInvalid() {
            path.points.Add(new Vector3());
            path.points.Add(new Vector3());
            Assert.IsTrue(path.IsValid);
        }
        
        
        
    }
}
