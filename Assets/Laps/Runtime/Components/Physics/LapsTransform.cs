using UnityEngine;

namespace LapsRuntime {
    [AddComponentMenu("Laps/Physics/Laps Transform")]
    [LapsAddMenuOptions("Physics/Laps Transform")]
    public class LapsTransform : LapsComponent {
        private Transform _transform;
        public void Awake() {
            _transform = transform;
        }
        public override object HandleInput(int slotId, object parameter, LapsComponent eventSource) {
            switch (slotId) {
                case 0:  return _transform;
                default: return null;
            }
        }
        public override void GetInputSlots(SlotList slots) {
            slots.Add("transform", 0, null, typeof(Transform));
        }
    }
}
