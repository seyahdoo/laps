using System.Collections;
using System.Collections.Generic;
using LapsRuntime;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace LapsEditModeTests {
    public class LapsRigidbodyTests {
        private LapsRigidbody comp2D;
        private LapsRigidbody comp3D;
        
        [SetUp]
        public void Setup() {
            var go = new GameObject();
            go.AddComponent<Rigidbody2D>();
            comp2D = go.AddComponent<LapsRigidbody>();

            go = new GameObject();
            go.AddComponent<Rigidbody>();
            comp3D = go.AddComponent<LapsRigidbody>();
        }
        
        [Test]
        public void LapsRigidbodyReturnsTheRigidbodyOfTheObject() {
            var coll2D = comp2D.GetComponent<Rigidbody2D>();
            var r2d = comp2D.HandleInput(0, null, null);
            Assert.AreEqual(coll2D, r2d);
            var trans2d = comp2D.transform;
            var t2d = comp2D.HandleInput(1, null, null);
            Assert.AreEqual(trans2d, t2d);
            
            var coll3D = comp3D.GetComponent<Rigidbody2D>();
            var r3d = comp3D.HandleInput(0, null, null);
            Assert.AreEqual(coll3D, r3d);
            var trans3d = comp3D.transform;
            var t3d = comp3D.HandleInput(1, null, null);
            Assert.AreEqual(trans3d, t3d);
        }
        [Test]
        public void Slots() {
            var slots = new List<LogicSlot>();
            comp2D.GetInputSlots(slots);
            Assert.AreEqual(new LogicSlot("body", 0, null, typeof(Rigidbody2D)), slots[0]);
            Assert.AreEqual(new LogicSlot("transform", 1, null, typeof(Transform)), slots[1]);
            
            slots.Clear();
            comp2D.GetOutputSlots(slots);
            Assert.AreEqual(0, slots.Count);
            
            slots.Clear();
            comp3D.GetInputSlots(slots);
            Assert.AreEqual(new LogicSlot("body", 0, null, typeof(Rigidbody)), slots[0]);
            Assert.AreEqual(new LogicSlot("transform", 1, null, typeof(Transform)), slots[1]);
            
            slots.Clear();
            comp3D.GetOutputSlots(slots);
            Assert.AreEqual(0, slots.Count);
            
        }
    }
}
