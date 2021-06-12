using System.Collections.Generic;

namespace LapsRuntime {
    public class StartEventComponent : LapsComponent {
        private void Update() {
            enabled = false;
            FireOutput(0, null);
        }
        public override void GetOutputSlots(List<LogicSlot> slots) {
            slots.Add(new LogicSlot("start", 0));
        }
    }
}
