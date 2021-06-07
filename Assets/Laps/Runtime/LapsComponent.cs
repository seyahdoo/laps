using System;
using System.Collections.Generic;
using UnityEngine;

namespace LapsRuntime {
    public class LapsComponent : MonoBehaviour {
        public List<Connection> connections = new List<Connection>();
        protected object FireOutput(int slotId, object parameter, int depth = 0) {
            return null;
        }
        protected IEnumerable<object> FireOutputAdvanced() {
            
            return null;
        }

        protected virtual object HandleInput(int slotId) {
            return null;
        }
    }
    [Serializable]
    public struct Connection {
        public LapsComponent targetComponent;
        public int sourceSlotId;
        public int targetSlotId;
    }
    public struct Slot {
        public string name;
        public Type parameterType;
        public Type returnType;
    }
}
