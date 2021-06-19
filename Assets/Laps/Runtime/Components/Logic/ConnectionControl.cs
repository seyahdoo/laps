using UnityEngine;

namespace LapsRuntime {
    [AddComponentMenu("Laps/Logic/Connection Control")]
    [LapsAddMenuOptions("Logic/Connection Control")]
    public class ConnectionControl : LapsComponent {
        public bool enabledOnAwake = true;
        private bool _enabled = false;
        public void Awake() {
            _enabled = enabledOnAwake;
        }
        public override object HandleInput(int slotId, object parameter, LapsComponent eventSource) {
            switch (slotId) {
                case 0:  InputReceived(parameter); return null;
                case 1:  _enabled = true; return null;
                case 2:  _enabled = false; return null;
                default: return null;
            }
        }
        private void InputReceived(object parameter) {
            if (_enabled) {
                FireOutput(0, parameter);
            }
        }
        public override void GetInputSlots(SlotList slots) {
            slots.Add("input", 0);
            slots.Add("enable", 1);
            slots.Add("disable", 2);
        }
        public override void GetOutputSlots(SlotList slots) {
            slots.Add("output", 0);
        }
    }
}
