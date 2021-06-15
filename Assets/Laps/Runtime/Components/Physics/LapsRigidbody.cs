using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LapsRuntime  {
    public class LapsRigidbody : LapsComponent {
        // public override object HandleInput(int slotId, object parameter, LapsComponent eventSource) {
        //     switch (slotId) {
        //         
        //     }
        // }
        public override void GetInputSlots(List<LogicSlot> slots) {
            slots.Add(new LogicSlot("body", 0, null, typeof(Rigidbody)));
            slots.Add(new LogicSlot("transform", 1, null, typeof(Transform)));
        }
    }
}
