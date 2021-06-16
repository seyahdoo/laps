using UnityEngine;

namespace LapsRuntime  {
    [LapsAddMenuOptions("Physics/Laps Rigidbody")]
    [RequireComponent(typeof(Rigidbody))]
    public class LapsRigidbody : LapsComponent {
        private Rigidbody _body;
        private Transform _transform;
        public void Awake() {
            _body = GetComponent<Rigidbody>();
            _transform = transform;
        }
        public override object HandleInput(int slotId, object parameter, LapsComponent eventSource) {
            switch (slotId) {
                case 0:  return _body;
                case 1:  return _transform;
                default: return null;
            }
        }
        public override void GetInputSlots(SlotList slots) {
            slots.Add("body", 0, null, typeof(Rigidbody));
            slots.Add("transform", 1, null, typeof(Transform));
        }
    }
}
