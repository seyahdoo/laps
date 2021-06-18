using UnityEngine;

namespace LapsRuntime {
    [LapsAddMenuOptions("Other/Debug")]
    public class DebugComponent : LapsComponent {
        public string label;
        public void FireDebugEvent() {
            FireOutput(0);
        }
        public override object HandleInput(int slotId, object parameter, LapsComponent eventSource) {
            switch (slotId) {
                case 0: Debug.Log($"{label}:{parameter}"); return null;
                case 1: Debug.LogError($"{label}:{parameter}"); return null;
            }
            return null;
        }
        public override void GetInputSlots(SlotList slots) {
            slots.Add("log normal", 0);
            slots.Add("log error", 1);
        }
        public override void GetOutputSlots(SlotList slots) {
            slots.Add("debug", 0);
        }
    }
}
