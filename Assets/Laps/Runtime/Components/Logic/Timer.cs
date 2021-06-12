using System.Collections.Generic;

namespace LapsRuntime {
    public class Timer : LapsComponent {
        public List<float> times = new List<float>() {.5f};
        public bool triggerOnAwake = false;
        public override void GetInputSlots(List<LogicSlot> slots) {
            slots.Add(new LogicSlot("start", 1));
            slots.Add(new LogicSlot("stop", 0));
            slots.Add(new LogicSlot("set time", 1, typeof(float)));
        }
        public override void GetOutputSlots(List<LogicSlot> slots) {
            slots.Add(new LogicSlot("trigger", 0));
        }
    }
}
