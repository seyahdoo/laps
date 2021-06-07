using System;
using System.Collections.Generic;
using UnityEngine;

namespace LapsRuntime {
    public class LapsComponent : MonoBehaviour {
        protected const int LOGIC_DEPTH_LIMIT = 10;
        private static int _logicFireDepth = 0;
        public List<Connection> connections = new List<Connection>();
        protected object FireOutput(int slotId, object parameter, int depth = 0) {
            _logicFireDepth++;
            if (_logicFireDepth >= LOGIC_DEPTH_LIMIT) {
                Debug.LogError("Logic depth limit reached! Something must be wrong!");
                return null;
            }
            

            _logicFireDepth--;
            return null;
        }
        protected OutputEnumerator FireOutputAdvanced() {
            _logicFireDepth++;
            if (_logicFireDepth >= LOGIC_DEPTH_LIMIT) {
                Debug.LogError("Logic depth limit reached! Something must be wrong!");
                return default;
            }
            

            _logicFireDepth--;
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
