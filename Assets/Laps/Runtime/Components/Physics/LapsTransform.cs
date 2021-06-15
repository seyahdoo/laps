using System.Collections.Generic;
using UnityEngine;

namespace LapsRuntime {
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
        public override void GetInputSlots(List<LogicSlot> slots) {
            slots.Add(new LogicSlot("transform", 0, null, typeof(Transform)));
        }
    }
}