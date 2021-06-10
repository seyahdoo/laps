using System;
using System.Collections.Generic;
using UnityEngine;

namespace LapsRuntime {
    public class LapsComponent : MonoBehaviour {
        public const string LOGIC_DEPTH_LIMIT_ERROR_STRING = "Logic depth limit reached! Something must be wrong!";
        private const int LOGIC_DEPTH_LIMIT = 10;
        private static int _logicFireDepth = 0;
        [HideInInspector, SerializeField] public List<Connection> connections = new List<Connection>();
        public bool ErrorExists => false;
        protected object FireOutput(int slotId, object parameter = null) {
            _logicFireDepth++;
            if (_logicFireDepth >= LOGIC_DEPTH_LIMIT) {
                Debug.LogError(LOGIC_DEPTH_LIMIT_ERROR_STRING);
                return null;
            }
            foreach (var connection in connections) {
                if (connection.sourceSlotId == slotId) {
                    connection.targetComponent.HandleInput(connection.targetSlotId, parameter, this);
                }
            }
            _logicFireDepth--;
            return null;
        }
        // protected OutputEnumerator FireOutputAdvanced() {
        //     _logicFireDepth++;
        //     if (_logicFireDepth >= LOGIC_DEPTH_LIMIT) {
        //         Debug.LogError("Logic depth limit reached! Something must be wrong!");
        //         return default;
        //     }
        //     
        //
        //     _logicFireDepth--;
        //     return default;
        // }
        protected virtual object HandleInput(int slotId, object parameter, LapsComponent eventSource) {
            return null;
        }
        public virtual void GetInputSlots(List<LogicSlot> slots) { }
        public virtual void GetOutputSlots(List<LogicSlot> slots) { }
        private void OnDrawGizmos() {
            //this is here because we want to be able to select laps components with unity selection rect,
            //unity selection rect wont select an object if it has no gizmos
            var oldColor = Gizmos.color;
            Gizmos.color = Color.clear;
            Gizmos.DrawSphere(transform.position, float.Epsilon);
            Gizmos.color = oldColor;
        }
    }
    // public struct OutputEnumerator {
    //     public int currentConnection;
    //     public LapsComponent CurrentComponent => default;
    //     public object CurrentOutput => default;
    // }
    [Serializable]
    public struct Connection {
        public int sourceSlotId;
        public LapsComponent targetComponent;
        public int targetSlotId;
        public Connection(int sourceSlotId, LapsComponent targetComponent, int targetSlotId) {
            this.sourceSlotId = sourceSlotId;
            this.targetComponent = targetComponent;
            this.targetSlotId = targetSlotId;
        }
    }
    public struct LogicSlot {
        public LogicSlot(string name, int id, Type parameterType = null, Type returnType = null) {
            this.name = name;
            this.id = id;
            this.parameterType = parameterType;
            this.returnType = returnType;
        }
        public string name;
        public int id;
        public Type parameterType;
        public Type returnType;
        public Color GetSlotParameterColor() {
            return TypeToColor(parameterType);
        }
        public Color GetSlotReturnColor() {
            return TypeToColor(returnType);
        }
        private Color TypeToColor(Type type) {
            if (type == null) return Color.white;
            if (type == typeof(Transform)) return Color.green;
            if (type == typeof(Rigidbody2D)) return Color.green;
            if (type == typeof(Rigidbody)) return Color.green;
            if (type == typeof(Collider2D)) return Color.green;
            if (type == typeof(Collider)) return Color.green;
            if (type == typeof(float)) return Color.magenta;
            if (type == typeof(string)) return Color.magenta;
            if (type == typeof(Vector3)) return Color.magenta;
            if (type == typeof(bool)) return Color.magenta;
            if (type == typeof(Color)) return Color.magenta;
            return Color.red;
        }
    }
}
