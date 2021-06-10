using System;
using System.Collections.Generic;
using UnityEngine;

namespace LapsRuntime {
    public class LapsComponent : MonoBehaviour {
        protected const int LOGIC_DEPTH_LIMIT = 10;
        private static int _logicFireDepth = 0;
        [HideInInspector, SerializeField] public List<Connection> connections = new List<Connection>();
        public bool ErrorExists => false;
        protected object FireOutput(int slotId, object parameter = null) {
            _logicFireDepth++;
            if (_logicFireDepth >= LOGIC_DEPTH_LIMIT) {
                Debug.LogError("Logic depth limit reached! Something must be wrong!");
                return null;
            }
            foreach (var connection in connections) {
                if (connection.sourceSlotId == slotId) {
                    connection.targetComponent.HandleInput(connection.targetSlotId, parameter);
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
        protected virtual object HandleInput(int slotId, object parameter) {
            return null;
        }
        protected virtual void GetInputSlots(List<Slot> slots) { }
        protected virtual void GetOutputSlots(List<Slot> slots) { }
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
    public struct Slot {
        public Slot(string name, int id, Type parameterType = null, Type returnType = null) {
            this.name = name;
            this.id = id;
            this.parameterType = parameterType;
            this.returnType = returnType;
        }
        public string name;
        public int id;
        public Type parameterType;
        public Type returnType;
    }
}
