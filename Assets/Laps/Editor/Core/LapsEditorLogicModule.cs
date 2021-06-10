using System.Collections;
using System.Collections.Generic;
using LapsRuntime;
using UnityEngine;

namespace LapsEditor {
    public class LapsEditorLogicModule {
        public void OnSceneGUI() {
            
            //draw logic slots
            //draw logic connections
            //handle logic connection add remove
        }

        public void Connect(LapsComponent sourceComponent, int sourceSlotId, LapsComponent targetComponent, int targetSlotId) {
            if (!CanConnect(sourceComponent, sourceSlotId, targetComponent, targetSlotId)) return;
            sourceComponent.connections.Add(new Connection(sourceSlotId, targetComponent, targetSlotId));
        }
        public void Disconnect(LapsComponent sourceComponent, int sourceSlotId, LapsComponent targetComponent, int targetSlotId) {
            if (!ConnectionExists(sourceComponent, sourceSlotId, targetComponent, targetSlotId)) return;
            for (int i = 0; i < sourceComponent.connections.Count; i++) {
                var connection = sourceComponent.connections[i];
                if (connection.sourceSlotId == sourceSlotId && connection.targetSlotId == targetSlotId && connection.targetComponent == targetComponent) {
                    sourceComponent.connections.RemoveAt(i);
                    return;
                }
            }
        }
        public bool CanConnect(LapsComponent sourceComponent, int sourceSlotId, LapsComponent targetComponent, int targetSlotId) {
            return true;
        }
        public bool ConnectionExists(LapsComponent sourceComponent, int sourceSlotId, LapsComponent targetComponent, int targetSlotId) {
            return true;
        }
    }
}
