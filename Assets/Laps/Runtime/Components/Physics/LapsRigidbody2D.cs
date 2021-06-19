using UnityEngine;

namespace LapsRuntime  {
    [AddComponentMenu("Laps/Physics/Laps Rigidbody 2D")]
    [LapsAddMenuOptions("Physics/Laps Rigidbody 2D")]
    [RequireComponent(typeof(Rigidbody2D))]
    public class LapsRigidbody2D : LapsComponent {
        public override object HandleInput(int slotId, object parameter, LapsComponent eventSource) {
            switch (slotId) {
                case 0:  return GetComponent<Rigidbody2D>();
                case 1:  return transform;
                default: return null;
            }
        }
        public override void GetInputSlots(SlotList slots) {
            slots.Add("body", 0, null, typeof(Rigidbody2D));
            slots.Add("transform", 1, null, typeof(Transform));
        }
    }
}
