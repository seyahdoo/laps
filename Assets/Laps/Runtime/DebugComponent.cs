using System.Collections.Generic;
using UnityEngine;

namespace LapsRuntime {
    public class DebugComponent : LapsComponent {
        public string label;
        public void FireDebugEvent() {
            FireOutput(0);
        }
        protected override object HandleInput(int slotId, object parameter) {
            switch (slotId) {
                case 0: Debug.Log($"{label}:{parameter}"); return null;
                case 1: Debug.LogError($"{label}:{parameter}"); return null;
            }
            return null;
        }
        protected override void GetInputSlots(List<Slot> slots) {
            slots.Add(new Slot("log normal", 0));
            slots.Add(new Slot("log error", 1));
        }
        protected override void GetOutputSlots(List<Slot> slots) {
            slots.Add(new Slot("debug", 0));
        }
    }
}
