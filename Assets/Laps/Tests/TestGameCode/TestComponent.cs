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
        protected override object HandleInput(int slotId, object parameter) {
            switch (slotId) {
                case 0: inputCallCount++; return null;
                case 1: FireSlotOne(); return null;
            }
            return null;
        }
        protected override void GetInputSlots(List<Slot> slots) {
            slots.Add(new Slot("log normal", 0));
            slots.Add(new Slot("fire slot 1", 1));
        }
        protected override void GetOutputSlots(List<Slot> slots) {
            slots.Add(new Slot("debug", 0));
            slots.Add(new Slot("slot 1", 1));
        }
    }
}
