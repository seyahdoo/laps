using UnityEngine;

namespace LapsRuntime {
    [AddComponentMenu("Laps/Physics/Laps Transform")]
    [LapsAddMenuOptions("Physics/Laps Transform")]
    public class LapsTransform : LapsComponent {
        public override object HandleInput(int slotId, object parameter, LapsComponent eventSource) {
            switch (slotId) {
                case 0:  return transform;
                case 1:  gameObject.SetActive(true); return null;
                case 2:  gameObject.SetActive(false); return null;
                default: return null;
            }
        }
        public override void GetInputSlots(SlotList slots) {
            slots.Add("transform", 0, null, typeof(Transform));
            slots.Add("enable", 1);
            slots.Add("disable", 2);
        }
    }
}
