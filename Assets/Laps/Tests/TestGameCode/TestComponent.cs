using System.Collections;
using System.Collections.Generic;
using LapsRuntime;
using UnityEngine;

namespace LapsEditModeTests {
    public class TestComponent : LapsComponent {
        public int inputCallCount = 0;
        public List<object> inputList = new List<object>();
        public Queue<object> testReturnQueue = new Queue<object>();
        public void FireEventBasic() {
            FireOutput(0);
        }
        public void FireSlotOne() {
            FireOutput(1);
        }
        public override object HandleInput(int slotId, object parameter, LapsComponent eventSource) {
            switch (slotId) {
                case 0: inputCallCount++; inputList.Add(parameter); return null;
                case 1: FireSlotOne(); return null;
                case 2: return testReturnQueue.Dequeue();
            }
            return null;
        }
        public override void GetInputSlots(List<LogicSlot> slots) {
            slots.Add(new LogicSlot("log normal", 0));
            slots.Add(new LogicSlot("fire slot 1", 1));
            slots.Add(new LogicSlot("test return", 2));
        }
        public override void GetOutputSlots(List<LogicSlot> slots) {
            slots.Add(new LogicSlot("debug", 0));
            slots.Add(new LogicSlot("slot 1", 1));
        }
    }
}
