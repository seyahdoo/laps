using LapsEditor;
using LapsRuntime;
using NUnit.Framework;
using UnityEngine;

namespace LapsEditModeTests {
    public class GetAndFireTests {
        [Test]
        public void SimpleUseCase() {
            var getAndFire = new GameObject().AddComponent<GetAndFire>();
            var tester1 = new GameObject().AddComponent<TestComponent>();
            tester1.testReturnQueue.Enqueue(5f);
            var tester2 = new GameObject().AddComponent<TestComponent>();
            LogicModule.Connect(getAndFire, 0, tester1, 2);
            LogicModule.Connect(getAndFire, 1, tester2, 0);
            getAndFire.HandleInput(0, null, null);
            Assert.AreEqual(1, tester2.inputCallCount);
            Assert.AreEqual(5f, tester2.inputList[0]);
        }
    }
}
