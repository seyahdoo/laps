using System.Collections.Generic;
using UnityEngine;

namespace LapsRuntime {
    [LapsAddMenuOptions("Logic/Debug Component")]
    public class DebugComponent : LapsComponent {
        public string label;
        public void FireDebugEvent() {
            FireOutput(0);
        }
        protected override object HandleInput(int slotId, object parameter, LapsComponent eventSource) {
            switch (slotId) {
                case 0: Debug.Log($"{label}:{parameter}"); return null;
                case 1: Debug.LogError($"{label}:{parameter}"); return null;
            }
            return null;
        }
        public override void GetInputSlots(List<LogicSlot> slots) {
            slots.Add(new LogicSlot("log normal", 0));
            slots.Add(new LogicSlot("log error", 1));
        }
        public override void GetOutputSlots(List<LogicSlot> slots) {
            slots.Add(new LogicSlot("debug", 0));
        }
    }
}
