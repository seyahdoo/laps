using UnityEngine;

namespace LapsRuntime {
    [AddComponentMenu("Laps/Value/Layer")]
    [LapsAddMenuOptions("Value/Layer")]
    public class LayerValue : LapsComponent {
        [Layer] public int value;
        public bool triggerOnAwake = false;
        public bool triggerOnChange = true;
        private void Awake() {
            if (triggerOnAwake) {
                Trigger();
            }
        }
        public override object HandleInput(int index, object argument, LapsComponent eventSource) {
            switch(index) {
                case 10: value  = (int)(float)argument;  OnValueChanged(); break;
                case 11: return value;
                case 12: Trigger(); break;
            }
            return null;
        }
        public void OnValueChanged() {
            if (triggerOnChange) {
                Trigger();
            }
        }
        public void Trigger() {
            FireOutput(0, value);
        }
        public override void GetInputSlots(SlotList slots) {
            slots.Add("set value", 10, typeof(float));
            slots.Add("get value", 11, null, typeof(float));
            slots.Add("trigger", 12);
        }
        public override void GetOutputSlots(SlotList slots) {
            slots.Add("on value change", 0, typeof(float));
        }
    }
}
