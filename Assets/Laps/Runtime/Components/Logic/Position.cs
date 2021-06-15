using System.Collections.Generic;
using UnityEngine;

namespace LapsRuntime {
    public class Position : LapsComponent {
        public override object HandleInput(int slotId, object parameter, LapsComponent eventSource) {
            switch (slotId) {
                case 0:  return transform.position;
                default: return null;
            }
        }
        public override void GetInputSlots(SlotList slots) {
            slots.Add("get position", 0, null, typeof(Vector3));
        }
    }
}
