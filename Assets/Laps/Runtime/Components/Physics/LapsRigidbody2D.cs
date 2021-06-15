using System.Collections.Generic;
using UnityEngine;

namespace LapsRuntime  {
    [RequireComponent(typeof(Rigidbody2D))]
    public class LapsRigidbody2D : LapsComponent {
        private Rigidbody2D _body;
        private Transform _transform;
        public void Awake() {
            _body = GetComponent<Rigidbody2D>();
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
            slots.Add("body", 0, null, typeof(Rigidbody2D));
            slots.Add("transform", 1, null, typeof(Transform));
        }
    }
}
