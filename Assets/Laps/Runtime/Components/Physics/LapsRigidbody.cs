using UnityEngine;

namespace LapsRuntime  {
    [AddComponentMenu("Laps/Physics/Laps Rigidbody")]
    [LapsAddMenuOptions("Physics/Laps Rigidbody")]
    [RequireComponent(typeof(Rigidbody))]
    public class LapsRigidbody : LapsComponent {
        public override object HandleInput(int slotId, object parameter, LapsComponent eventSource) {
            switch (slotId) {
                case 0:  return GetComponent<Rigidbody>();
                case 1:  return transform;
                default: return null;
            }
        }
        public override void GetInputSlots(SlotList slots) {
            slots.Add("body", 0, null, typeof(Rigidbody));
            slots.Add("transform", 1, null, typeof(Transform));
        }
    }
}
