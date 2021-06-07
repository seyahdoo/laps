using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace LapsRuntime {
    public class LapsComponent : MonoBehaviour {
        public List<Connection> connections = new List<Connection>();
        protected object FireOutput(int slotId, object parameter, int depth = 0) {
            return null;
        }
        protected OutputEnumerator FireOutputAdvanced() {
            
            return default;
        }
        protected virtual object HandleInput(int slotId) {
            return null;
        }
        private void OnDrawGizmos() {
            GUI.DrawTexture(
                new Rect(transform.position, Vector2.one * 28f), 
                Resources.Load<Texture>("debugobject-icon"), 
                ScaleMode.StretchToFill, 
                true, 
                0.0f, 
                Color.white, 
                0.0f,
                0.0f);
            // Handle
            // Gizmos.DrawIcon(transform.position, "debugobject-icon", true, Color.clear);
        }
    }
    public struct OutputEnumerator {
        public int currentConnection;
        public LapsComponent CurrentComponent => default;
        public object CurrentOutput => default;
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
