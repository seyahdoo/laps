using System.Collections;
using System.Collections.Generic;
using LapsRuntime;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace LapsEditModeTests {
    public class ConnectionTests {
        [Test]
        public void ConnectBasic() {
            var comp1 = new GameObject().AddComponent<TestComponent>();
            var comp2 = new GameObject().AddComponent<TestComponent>();
            LapsEditor.LapsEditor.lapsEditorLogicModule.Connect(comp1, 0, comp2, 0);
            comp1.FireEventBasic();
            Assert.AreEqual(1, comp2.inputCallCount);
        }
        [Test]
        public void DisconnectBasic() {
            var comp1 = new GameObject().AddComponent<TestComponent>();
            var comp2 = new GameObject().AddComponent<TestComponent>();
            LapsEditor.LapsEditor.lapsEditorLogicModule.Connect(comp1, 0, comp2, 0);
            LapsEditor.LapsEditor.lapsEditorLogicModule.Disconnect(comp1, 0, comp2, 0);
            comp1.FireEventBasic();
            Assert.AreEqual(0, comp2.inputCallCount);
        }
        [Test]
        public void RecursiveLoopExitsAtSomePointAndLogsErrorWithFireOutputBasic() {
            var comp1 = new GameObject().AddComponent<TestComponent>();
            var comp2 = new GameObject().AddComponent<TestComponent>();
            LapsEditor.LapsEditor.lapsEditorLogicModule.Connect(comp1, 1, comp2, 1);
            LapsEditor.LapsEditor.lapsEditorLogicModule.Connect(comp2, 1, comp1, 1);
            comp1.FireSlotOne();
            LogAssert.Expect(LogType.Error, LapsComponent.LOGIC_DEPTH_LIMIT_ERROR_STRING);
        }
        [Test]
        public void RecursiveLoopExitsAtSomePointAndLogsErrorWithFireOutputAdvanced() { Assert.Fail("Test not implemented yet"); }
        [Test]
        public void FireOutputBasicCallsHandleInputOnTwoConnectedObjects() {
            var comp1 = new GameObject().AddComponent<TestComponent>();
            var comp2 = new GameObject().AddComponent<TestComponent>();
            var comp3 = new GameObject().AddComponent<TestComponent>();
            LapsEditor.LapsEditor.lapsEditorLogicModule.Connect(comp1, 0, comp2, 0);
            LapsEditor.LapsEditor.lapsEditorLogicModule.Connect(comp1, 0, comp3, 0);
            comp1.FireEventBasic();
            Assert.AreEqual(1, comp2.inputCallCount);
            Assert.AreEqual(1, comp3.inputCallCount);
        }
        [Test]
        public void FireOutputAdvancedCallsHandleInputOnConnectedObjectsAsMoveNextCalled() { Assert.Fail("Test not implemented yet"); }
        [Test]
        public void TryingToConnectSameConnectionTwiceDoesNotCreateTwoConnections() {
            var comp1 = new GameObject().AddComponent<TestComponent>();
            var comp2 = new GameObject().AddComponent<TestComponent>();
            LapsEditor.LapsEditor.lapsEditorLogicModule.Connect(comp1, 0, comp2, 0);
            Assert.AreEqual(1, comp1.connections.Count);
            LapsEditor.LapsEditor.lapsEditorLogicModule.Connect(comp1, 0, comp2, 0);
            Assert.AreEqual(1, comp1.connections.Count);
            comp1.FireEventBasic();
            Assert.AreEqual(1, comp2.inputCallCount);
        }
        [Test]
        public void TryingToDisconnectAnInvalidConnectionDoesNotGenerateErrors() { Assert.Fail("Test not implemented yet"); }
        [Test]
        public void CanDisconnectLastConnectionOfParticularSlot() { Assert.Fail("Test not implemented yet"); }
    }
}
