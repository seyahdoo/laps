using System.Collections;
using System.Collections.Generic;
using LapsRuntime;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace LapsEditModeTests {
    public class PositionTests {
        [Test]
        public void SimpleTest() {
            var positionComponent = new GameObject().AddComponent<Position>();
            positionComponent.transform.position = new Vector3(10f, 5f, 2f);
            var returnValue = positionComponent.HandleInput(0, null, null);
            Assert.AreEqual(new Vector3(10f, 5f, 2f), returnValue);
        }
    }
}
