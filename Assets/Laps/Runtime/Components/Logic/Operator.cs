using UnityEngine;

namespace LapsRuntime {
    [AddComponentMenu("Laps/Logic/Operator")]
    [LapsAddMenuOptions("Logic/Operator")]
    public class Operator : LapsComponent {
        public float a;
        public float b;
        public OperationType operationType;
        public bool triggerOnAwake = false;
        public bool triggerOnResultChange = false;
        public bool triggerOnValueChange = true;
        private float _lastResult;
        public enum OperationType {
            Add = 0,
            Subtract = 1,
            Multiply = 2,
            Divide = 3,
        }
        public void Awake() {
            _lastResult = Calculate();
            if (triggerOnAwake) {
                Trigger();
            }
        }
        private void OnValueChange() {
            if (triggerOnValueChange) {
                Trigger();
                return;
            }
            if (triggerOnResultChange) {
                var result = Calculate();
                if (!LapsMath.ApproximatelyClose(result, _lastResult)) {
                    _lastResult = result;
                    FireOutput(0, _lastResult);
                }
            }
        }
        public void Trigger() {
            _lastResult = Calculate();
            FireOutput(0, _lastResult);
        }
        private float Calculate() {
            switch (operationType) {
                case OperationType.Add:      return a + b;
                case OperationType.Subtract: return a - b;
                case OperationType.Multiply: return a * b;
                case OperationType.Divide:   return a / b;
                default:                     return 0f;
            }
        }
        public override object HandleInput(int slotId, object parameter, LapsComponent eventSource) {
            switch (slotId) {
                case 0:  a = (float) parameter; OnValueChange(); return null;
                case 1:  b = (float) parameter; OnValueChange(); return null;
                default: return null;
            }
        }
        public override void GetInputSlots(SlotList slots) {
            slots.Add("set a", 0, typeof(float));
            slots.Add("set b", 1, typeof(float));
            slots.Add("trigger", 2);
        }
        public override void GetOutputSlots(SlotList slots) {
            slots.Add("output", 0, typeof(float));
        }
    }
}
