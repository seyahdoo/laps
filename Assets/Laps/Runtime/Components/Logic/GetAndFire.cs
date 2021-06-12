using System.Collections.Generic;

namespace LapsRuntime {
    public class GetAndFire : LapsComponent {
        public override object HandleInput(int slotId, object parameter, LapsComponent eventSource) {
            switch (slotId) {
                case 0: FireOutput(1, FireOutput(0)); return null;
                default: return null;
            }
        }
        public override void GetInputSlots(List<LogicSlot> slots) {
            slots.Add(new LogicSlot("trigger", 0));
        }
        public override void GetOutputSlots(List<LogicSlot> slots) {
            slots.Add(new LogicSlot("get value", 0));
            slots.Add(new LogicSlot("fire wire", 1));
        }
    }
}
