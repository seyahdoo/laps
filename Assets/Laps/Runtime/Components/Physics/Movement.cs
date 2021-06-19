using UnityEngine;

namespace LapsRuntime {
    [AddComponentMenu("Laps/Physics/Movement")]
    [LapsAddMenuOptions("Physics/Movement")]
    public class Movement : LapsComponent {
        public override void GetInputSlots(SlotList slots) {
            slots.Add("set body", 0);
            slots.Add("go forwards", 1);
            slots.Add("go backwards", 2);
            slots.Add("set speed", 3, typeof(float));
            slots.Add("stop", 4);
            slots.Add("teleport forward end", 5);
            slots.Add("teleport backward end", 6);
        }
        public override void GetOutputSlots(SlotList slots) {
            slots.Add("body", 0);
            slots.Add("forwards finished", 1);
            slots.Add("backwards finished", 2);
        }
    }
}
