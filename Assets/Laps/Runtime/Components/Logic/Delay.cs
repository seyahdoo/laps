using System.Collections.Generic;
using UnityEngine;

namespace LapsRuntime {
    [LapsAddMenuOptions("Logic/Delay")]
    public class Delay : LapsComponent {
        public float delayAmount = 0f;
        private List<Flow> _flows = new List<Flow>();
        private struct Flow {
            public float activateTime;
            public object data;
        }
        private void Update() {
            for (int i = _flows.Count - 1; i >= 0; i--) {
                var flow = _flows[i];
                if (Time.time >= flow.activateTime) {
                    _flows.RemoveAt(i);
                    FireOutput(0, flow.data);
                }
            }
        }
        public override object HandleInput(int slotId, object parameter, LapsComponent eventSource) {
            switch (slotId) {
                case 0:  OnInput(parameter); return null;
                case 1:  delayAmount = (float) parameter; return null;
                default: return null;
            }
        }
        private void OnInput(object parameter) {
            var flow = new Flow() {
                data = parameter,
                activateTime = Time.time + delayAmount,
            };
            _flows.Add(flow);
        }
        public override void GetOutputSlots(SlotList slots) {
            slots.Add("output", 0);
        }
        public override void GetInputSlots(SlotList slots) {
            slots.Add("input", 0);
            slots.Add("set delay amount", 1, typeof(float));
        }
    }
    
}
