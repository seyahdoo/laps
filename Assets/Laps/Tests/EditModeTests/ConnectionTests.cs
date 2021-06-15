using System;
using LapsEditor;
using LapsRuntime;
using NUnit.Framework;
using UnityEngine;

namespace LapsEditModeTests {
    public class ConnectionTests {
        [Test]
        public void ConnectBasic() {
            var comp1 = new GameObject().AddComponent<TestComponent>();
            var comp2 = new GameObject().AddComponent<TestComponent>();
            LogicModule.Connect(comp1, 0, comp2, 0);
            comp1.FireEventBasic();
            Assert.AreEqual(1, comp2.inputCallCount);
        }
        [Test]
        public void DisconnectBasic() {
            var comp1 = new GameObject().AddComponent<TestComponent>();
            var comp2 = new GameObject().AddComponent<TestComponent>();
            LogicModule.Connect(comp1, 0, comp2, 0);
            LogicModule.Disconnect(comp1, 0, comp2, 0);
            comp1.FireEventBasic();
            Assert.AreEqual(0, comp2.inputCallCount);
        }
        [Test]
        public void RecursiveLoopExitsAtSomePointAndLogsErrorWithFireOutputBasic() {
            var comp1 = new GameObject().AddComponent<TestComponent>();
            var comp2 = new GameObject().AddComponent<TestComponent>();
            LogicModule.Connect(comp1, 1, comp2, 1);
            LogicModule.Connect(comp2, 1, comp1, 1);
            try {
                comp1.FireSlotOne();
            }
            catch (Exception e) {
                Assert.AreEqual(LapsComponent.LOGIC_DEPTH_LIMIT_ERROR_STRING, e.Message);
            }
        }
        [Test]
        public void FireOutputBasicCallsHandleInputOnTwoConnectedObjects() {
            var comp1 = new GameObject().AddComponent<TestComponent>();
            var comp2 = new GameObject().AddComponent<TestComponent>();
            var comp3 = new GameObject().AddComponent<TestComponent>();
            LogicModule.Connect(comp1, 0, comp2, 0);
            LogicModule.Connect(comp1, 0, comp3, 0);
            comp1.FireEventBasic();
            Assert.AreEqual(1, comp2.inputCallCount);
            Assert.AreEqual(1, comp3.inputCallCount);
        }
        [Test]
        public void TryingToConnectSameConnectionTwiceDoesNotCreateTwoConnections() {
            var comp1 = new GameObject().AddComponent<TestComponent>();
            var comp2 = new GameObject().AddComponent<TestComponent>();
            LogicModule.Connect(comp1, 0, comp2, 0);
            Assert.AreEqual(1, comp1.connections.Count);
            LogicModule.Connect(comp1, 0, comp2, 0);
            Assert.AreEqual(1, comp1.connections.Count);
            comp1.FireEventBasic();
            Assert.AreEqual(1, comp2.inputCallCount);
        }
        [Test]
        public void TryingToDisconnectAnInvalidConnectionDoesNotGenerateErrors() {
            var comp1 = new GameObject().AddComponent<TestComponent>();
            var comp2 = new GameObject().AddComponent<TestComponent>();
            LogicModule.Disconnect(comp1, 0, comp2, 0);
        }
        //todo implement this tests
        // [Test]
        // [Ignore("gives an error with an unreleted issue")]
        // public void DrawingInvalidConnectionWorksDoesNotThrowException() {
        //     var comp1 = new GameObject().AddComponent<TestComponent>();
        //     var comp2 = new GameObject().AddComponent<TestComponent>();
        //     LapsEditor.LapsEditor.instance.lapsEditorLogicModule.Connect(comp1, 0, comp2, 100);
        //     LapsEditor.LapsEditor.instance.allComponents = new[] {comp1, comp2};
        //     LapsEditor.LapsEditor.instance.lapsEditorLogicModule.OnRepaint();
        // }
        // [Test]
        // [Ignore("not implemented yet")]
        // public void CanDisconnectLastConnectionOfParticularSlot() { Assert.Fail("Test not implemented yet"); }
        // [Test]
        // [Ignore("not implemented yet")]
        // public void CanConnectIsSane() { Assert.Fail("Test not implemented yet"); }
        // [Test]
        // [Ignore("not implemented yet")]
        // public void FireOutputAdvancedCallsHandleInputOnConnectedObjectsAsMoveNextCalled() { Assert.Fail("Test not implemented yet"); }
        // [Test]
        // [Ignore("not implemented yet")]
        // public void RecursiveLoopExitsAtSomePointAndLogsErrorWithFireOutputAdvanced() { Assert.Fail("Test not implemented yet"); }
    }
}
