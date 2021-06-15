using System.Collections.Generic;
using UnityEngine;

namespace LapsRuntime  {
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
        public override void GetInputSlots(List<LogicSlot> slots) {
            slots.Add(new LogicSlot("body", 0, null, typeof(Rigidbody)));
            slots.Add(new LogicSlot("transform", 1, null, typeof(Transform)));
        }
    }
}
