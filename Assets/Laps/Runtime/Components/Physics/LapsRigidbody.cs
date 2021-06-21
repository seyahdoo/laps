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
                case 2:  gameObject.SetActive(true); return null;
                case 3:  gameObject.SetActive(false); return null;
                case 4:  GetComponent<Rigidbody>().isKinematic = true; return null;
                case 5:  GetComponent<Rigidbody>().isKinematic = false; return null;
                default: return null;
            }
        }
        public override void GetInputSlots(SlotList slots) {
            slots.Add("body", 0, null, typeof(Rigidbody));
            slots.Add("transform", 1, null, typeof(Transform));
            slots.Add("enable", 2);
            slots.Add("disable", 3);
            slots.Add("set kinematic", 4);
            slots.Add("set dynamic", 5);
        }
    }
}
