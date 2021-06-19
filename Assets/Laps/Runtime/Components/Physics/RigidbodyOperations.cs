using UnityEngine;

namespace LapsRuntime {
    [AddComponentMenu("Laps/Physics/Rigidbody Operations")]
    [LapsAddMenuOptions("Physics/Rigidbody Operations")]
    public class RigidbodyOperations : LapsComponent {
        private Rigidbody _rigidbody;
        public Rigidbody CurrentBody => _rigidbody;
        public void Awake() {
            _rigidbody = FireOutput(0) as Rigidbody;
        }
        public override object HandleInput(int slotId, object parameter, LapsComponent eventSource) {
            switch (slotId) {
                case 0:  _rigidbody = (Rigidbody) parameter; return null;
                case 1:  return _rigidbody.position;
                case 2:  _rigidbody.position = (Vector3) parameter; return null;
                case 3:  return _rigidbody.velocity;
                case 4:  _rigidbody.velocity = (Vector3) parameter; return null;
                default: return null;
            }
        }
        public override void GetInputSlots(SlotList slots) {
            slots.Add("set body", 0, typeof(Rigidbody));
            slots.Add("get position", 1, null, typeof(Vector3));
            slots.Add("set position", 2, typeof(Vector3));
            slots.Add("get velocity", 3, null, typeof(Vector3));
            slots.Add("set velocity", 4, typeof(Vector3));
        }
        public override void GetOutputSlots(SlotList slots) {
            slots.Add("body", 0, typeof(Rigidbody));
        }
    }
}
