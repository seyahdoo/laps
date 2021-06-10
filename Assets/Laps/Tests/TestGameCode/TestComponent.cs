using System.Collections;
using System.Collections.Generic;
using LapsRuntime;
using UnityEngine;

namespace LapsEditModeTests {
    public class TestComponent : LapsComponent {
        public int inputCallCount = 0;
        public void FireEventBasic() {
            FireOutput(0);
        }
        public void FireSlotOne() {
            FireOutput(1);
        }
        protected override object HandleInput(int slotId, object parameter, LapsComponent eventSource) {
            switch (slotId) {
                case 0: inputCallCount++; return null;
                case 1: FireSlotOne(); return null;
            }
            return null;
        }
        public override void GetInputSlots(List<LogicSlot> slots) {
            slots.Add(new LogicSlot("log normal", 0));
            slots.Add(new LogicSlot("fire slot 1", 1));
        }
        public override void GetOutputSlots(List<LogicSlot> slots) {
            slots.Add(new LogicSlot("debug", 0));
            slots.Add(new LogicSlot("slot 1", 1));
        }
    }
}
