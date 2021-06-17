using LapsEditor;
using LapsRuntime;
using NUnit.Framework;
using UnityEngine;

namespace LapsEditModeTests {
    public class CompoundComponentTests {
        public CompoundComponent compoundComponent;
        public TestComponent testComponent;
        [SetUp]
        public void Setup() {
            compoundComponent = new GameObject().AddComponent<CompoundComponent>();
            testComponent = new GameObject().AddComponent<TestComponent>();
        }
        [TearDown]
        public void Teardown() {
            Object.DestroyImmediate(compoundComponent.gameObject);
            Object.DestroyImmediate(testComponent.gameObject);
        }
        [Test]
        public void ZeroSlotIdIsInvalidAndWontGenerateSlots() {
            compoundComponent.inputSlots.Add(new CompoundComponent.SlotInformation() {
                id = 0,
                name = "input",
                parameterType = CompoundComponent.TypeEnum.Null,
                returnType = CompoundComponent.TypeEnum.Null,
            });
            var slots = new SlotList();
            compoundComponent.GetInputSlots(slots);
            Assert.AreEqual(0, slots.Count);
            slots.Clear();
            compoundComponent.GetOutputSlots(slots);
            Assert.AreEqual(0, slots.Count);
        }
        [Test]
        public void NegativeSlotIdIsInvalidAndWontGenerateSlots() {
            compoundComponent.inputSlots.Add(new CompoundComponent.SlotInformation() {
                id = -5,
                name = "input",
                parameterType = CompoundComponent.TypeEnum.Null,
                returnType = CompoundComponent.TypeEnum.Null,
            });
            var slots = new SlotList();
            compoundComponent.GetInputSlots(slots);
            Assert.AreEqual(0, slots.Count);
            slots.Clear();
            compoundComponent.GetOutputSlots(slots);
            Assert.AreEqual(0, slots.Count);
        }
        [Test]
        public void AddInputSlot() {
            var slotInformation = new CompoundComponent.SlotInformation() {
                id = 1,
                name = "input",
                parameterType = CompoundComponent.TypeEnum.Float,
                returnType = CompoundComponent.TypeEnum.Rigidbody2D,
            };
            compoundComponent.inputSlots.Add(slotInformation);
            var slots = new SlotList();
            compoundComponent.GetInputSlots(slots);
            Assert.AreEqual(1, slots.Count);
            Assert.AreEqual(1, slots[0].id);
            Assert.AreEqual("input", slots[0].name);
            Assert.AreEqual(typeof(float), slots[0].parameterType);
            Assert.AreEqual(typeof(Rigidbody2D), slots[0].returnType);
            slots.Clear();
            compoundComponent.GetOutputSlots(slots);
            Assert.AreEqual(1, slots.Count);
            Assert.AreEqual(-1, slots[0].id);
            Assert.AreEqual("input", slots[0].name);
            Assert.AreEqual(typeof(float), slots[0].parameterType);
            Assert.AreEqual(typeof(Rigidbody2D), slots[0].returnType);
        }
        [Test]
        public void AddOutputSlot() {
            var slotInformation = new CompoundComponent.SlotInformation() {
                id = 1,
                name = "output",
                parameterType = CompoundComponent.TypeEnum.Float,
                returnType = CompoundComponent.TypeEnum.Rigidbody2D,
            };
            compoundComponent.outputSlots.Add(slotInformation);
            var slots = new SlotList();
            compoundComponent.GetOutputSlots(slots);
            Assert.AreEqual(1, slots.Count);
            Assert.AreEqual(1, slots[0].id);
            Assert.AreEqual("output", slots[0].name);
            Assert.AreEqual(typeof(float), slots[0].parameterType);
            Assert.AreEqual(typeof(Rigidbody2D), slots[0].returnType);
            slots.Clear();
            compoundComponent.GetInputSlots(slots);
            Assert.AreEqual(1, slots.Count);
            Assert.AreEqual(-1, slots[0].id);
            Assert.AreEqual("output", slots[0].name);
            Assert.AreEqual(typeof(float), slots[0].parameterType);
            Assert.AreEqual(typeof(Rigidbody2D), slots[0].returnType);
        }
        [Test]
        public void InputSignalTest() {
            AddInputSlot();
            LogicModule.Connect(compoundComponent, -1, testComponent, 0);
            compoundComponent.HandleInput(1, "test parameter", null);
            Assert.AreEqual(1, testComponent.inputCallCount);
            Assert.AreEqual("test parameter", testComponent.inputList[0]);
        }
        [Test]
        public void OutputSignalTests() {
            AddOutputSlot();
            LogicModule.Connect(compoundComponent, 1, testComponent, 0);
            compoundComponent.HandleInput(-1, "test parameter", null);
            Assert.AreEqual(1, testComponent.inputCallCount);
            Assert.AreEqual("test parameter", testComponent.inputList[0]);
        }
        // [Test] public void InputSlotsGetAutomaticIds() { Assert.Fail(); }
        // [Test] public void AddSlotInformationToInputSlotsChangesInputSlotsOnOutside() { Assert.Fail(); }
        // [Test] public void AddSlotInformationToInputSlotsChangesOutputSlotsOnInside() { Assert.Fail(); }
        // [Test] public void AddSlotInformationToOutputSlotsChangesOutputSlotsOnOutside() { Assert.Fail(); }
        // [Test] public void AddSlotInformationToOutputSlotsChangesInputSlotsOnInside() { Assert.Fail(); }
    }
}
