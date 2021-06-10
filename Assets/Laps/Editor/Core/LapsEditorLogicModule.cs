using LapsRuntime;
using UnityEngine;

namespace LapsEditor {
    public class LapsEditorLogicModule {
        private LapsEditor _editor;
        public LapsEditorLogicModule(LapsEditor lapsEditor) {
            _editor = lapsEditor;
        }
        public void OnSceneGUI() {
            CustomHandle.Draw(((isHotControl, isClosestHandle) => {
                DrawAllSlots();
                DrawAllConnections();
                DrawDraggingAndHoverConnection();
            }),(() => {
                return GetNearestDistanceFromPointToAnySlot(Event.current.mousePosition);
            }), () => {
                //remember pressed slot
            }, () => {
                //handle connecting
            });
            
            //draw logic slots
            //draw logic connections
            //handle logic connection add remove
        }


        public void Connect(LapsComponent sourceComponent, int sourceSlotId, LapsComponent targetComponent, int targetSlotId) {
            if (!CanConnect(sourceComponent, sourceSlotId, targetComponent, targetSlotId)) return;
            if (ConnectionExists(sourceComponent, sourceSlotId, targetComponent, targetSlotId)) return;
            sourceComponent.connections.Add(new Connection(sourceSlotId, targetComponent, targetSlotId));
        }
        public void Disconnect(LapsComponent sourceComponent, int sourceSlotId, LapsComponent targetComponent, int targetSlotId) {
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
            foreach (var connection in sourceComponent.connections) {
                if (connection.sourceSlotId == sourceSlotId && connection.targetSlotId == targetSlotId && connection.targetComponent == targetComponent) {
                    return true;
                }
            }
            return false;
        }
        private void DrawAllSlots() {
            
            
        }
        private void DrawAllConnections() {
        }
        private void DrawDraggingAndHoverConnection() {
        }
        private float GetNearestDistanceFromPointToAnySlot(Vector2 point) {
            return float.MaxValue;
        }
    }
}
