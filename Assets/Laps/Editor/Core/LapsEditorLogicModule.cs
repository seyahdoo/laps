using System.Collections.Generic;
using LapsEditor.Utility;
using LapsRuntime;
using UnityEditor;
using UnityEngine;

namespace LapsEditor {
    public class LapsEditorLogicModule {
        private static readonly float SlotRadius = 6f;
        private static readonly float SlotSpace = 12f;
        private static readonly float SlotMargin = 6f;
        private static readonly Color ConnectableConnectionColor = new Color(0, 1, 0, 1);
        private static readonly Color NonConnectableConnectionColor = new Color(1, 0, 0, 1);
        private static readonly Color DanglingConnectionColor = new Color(1, 1, 1, 1);
        
        private LapsEditor _editor;
        private static List<Slot> _slots = new List<Slot>();
        public LapsEditorLogicModule(LapsEditor lapsEditor) {
            _editor = lapsEditor;
        }
        public void OnSceneGUI() {
            CustomHandle.Draw(((isHotControl, isClosestHandle) => {
                DrawAllSlots();
                DrawAllConnections();
                DrawDraggingAndHoverConnection();
                DrawLabels();
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
            foreach (var lapsComponent in _editor.allComponents) {
                _slots.Clear();
                lapsComponent.GetInputSlots(_slots);
                for (int i = 0; i < _slots.Count; i++) {
                    var slotInformation = new SlotInformation(lapsComponent, true, _slots[i], i);
                    DrawSlot(slotInformation);
                }
                _slots.Clear();
                lapsComponent.GetOutputSlots(_slots);
                for (int i = 0; i < _slots.Count; i++) {
                    var slotInformation = new SlotInformation(lapsComponent, false, _slots[i], i);
                    DrawSlot(slotInformation);
                }
            }
        }
        private void DrawSlot(SlotInformation slotInformation) {
            var position = GetScreenPositionOfSlot(slotInformation);
            if (!IsSlotPositionInsideSceneView(position)) return;
            position.x = Mathf.Floor(position.x);
            position.y = Mathf.Floor(position.y);
            var parameterColor = slotInformation.slot.GetSlotParameterColor();
            var returnColor = slotInformation.slot.GetSlotReturnColor();
            using (Scopes.HandlesGUI()) {
                using (Scopes.HandlesColor(Color.white)) {
                    Handles.DrawSolidRectangleWithOutline(new Rect(position - Vector2.one * SlotRadius, Vector2.one* SlotRadius * 2), Color.black, Color.clear);
                    Handles.DrawSolidRectangleWithOutline(new Rect(position - Vector2.one * SlotRadius*.8f, new Vector2(SlotRadius * .8f, SlotRadius * 2 * .8f)), parameterColor, Color.clear);
                    Handles.DrawSolidRectangleWithOutline(new Rect(new Vector2(position.x, position.y - SlotRadius * .8f), new Vector2(SlotRadius * .8f, SlotRadius * 2 * .8f)), returnColor, Color.clear);
                }
            }
        }
        private Vector2 GetScreenPositionOfSlot(SlotInformation slotInformation) {
            var worldPoint = slotInformation.lapsComponent.transform.position;
            var screenPosition =  HandleUtility.WorldToGUIPoint(worldPoint);
            screenPosition -= new Vector2((slotInformation.isInput ? 1: -1) * ((LapsEditorSelectionModule.SelectionIconSize.x / 2) + SlotRadius), LapsEditorSelectionModule.SelectionIconSize.y / 2 - SlotMargin);
            screenPosition -= (slotInformation.index * SlotSpace * Vector2.down);
            return screenPosition;
        }
        private bool IsSlotPositionInsideSceneView(Vector2 screenPosition) {
            if (screenPosition.x < - SlotRadius / 2f - 10f) return false;
            if (screenPosition.x > Screen.width + SlotRadius / 2f + 10f) return false;
            if (screenPosition.y < - SlotRadius / 2f - 10f) return false;
            if (screenPosition.y > Screen.height - 40 + SlotRadius / 2f + 10f) return false;
            return true;
        }
        private void DrawAllConnections() {
        }
        private void DrawDraggingAndHoverConnection() {
        }
        private float GetNearestDistanceFromPointToAnySlot(Vector2 point) {
            return float.MaxValue;
        }
        private void DrawLabels() {
        }
        private struct SlotInformation {
            public LapsComponent lapsComponent;
            public bool isInput;
            public Slot slot;
            public int index;
            public SlotInformation(LapsComponent lapsComponent, bool isInput, Slot slot, int index) {
                this.lapsComponent = lapsComponent;
                this.isInput = isInput;
                this.slot = slot;
                this.index = index;
            }
        }
    }
}
