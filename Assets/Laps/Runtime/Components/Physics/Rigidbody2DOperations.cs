using UnityEngine;

namespace LapsRuntime {
    [AddComponentMenu("Laps/Physics/Rigidbody 2D Operations")]
    [LapsAddMenuOptions("Physics/Rigidbody 2D Operations")]
    public class Rigidbody2DOperations : LapsComponent {
        private Rigidbody2D _currentBody;
        public object CurrentBody => _currentBody;
        public void Awake() {
            _currentBody = FireOutput(0) as Rigidbody2D;
        }
        public override object HandleInput(int slotId, object parameter, LapsComponent eventSource) {
            switch (slotId) {
                case 0:  _currentBody = (Rigidbody2D) parameter; return null;
                case 1:  return (Vector3) _currentBody.position;
                case 2:  _currentBody.position = (Vector2) (Vector3) parameter; return null;
                case 3:  return (Vector3) _currentBody.velocity;
                case 4:  _currentBody.velocity = (Vector2) (Vector3) parameter; return null;
                default: return null;
             }
        }
        public override void GetInputSlots(SlotList slots) {
            slots.Add("set body", 0, typeof(Rigidbody2D));
            slots.Add("get position", 1, null, typeof(Vector3));
            slots.Add("set position", 2, typeof(Vector3));
            slots.Add("get velocity", 3, null, typeof(Vector3));
            slots.Add("set velocity", 4, typeof(Vector3));
        }
        public override void GetOutputSlots(SlotList slots) {
            slots.Add("body", 0, typeof(Rigidbody2D));
        }
    }
}
