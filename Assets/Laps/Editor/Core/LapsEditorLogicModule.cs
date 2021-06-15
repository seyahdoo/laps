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
        private static readonly Color ConnectableConnectionColor = Color.white;
        private static readonly Color ConnectionJustFiredColor = Color.green;
        private static readonly Color NonConnectableConnectionColor = Color.red;
        private static readonly Color DanglingConnectionColor = Color.white;
        private static readonly Color ConnectionBackgroundColor = Color.black;
        private static readonly Color ConnectionJustFiredBackgroundColor = Color.green;
        private static readonly float FireAnimationDuration = 1f;
        private static readonly int HandleHintHash = nameof(LapsEditorLogicModule).GetHashCode();

        private static List<LogicSlot> _slots = new List<LogicSlot>();
        private LapsEditor _editor;
        private ShortcutManager _shortcutManager;
        private Dictionary<SlotInformationCacheKey, SlotInformation> _slotInformationCacheDictionary = new Dictionary<SlotInformationCacheKey, SlotInformation>();
        private SlotInformation _draggingSlot;
        private bool _dragging = false;
        private bool _logicEditModeEnabled = false;
        private Dictionary<OutputFireTimingKey, float> _lastOutputFireTimes = new Dictionary<OutputFireTimingKey, float>();
        private struct OutputFireTimingKey {
            public LapsComponent lapsComponent;
            public int slotId;
        }
        public LapsEditorLogicModule(LapsEditor lapsEditor) {
            _editor = lapsEditor;
            SetupShortcuts();
        }
        private void SetupShortcuts() {
            _shortcutManager = new ShortcutManager();
            _shortcutManager.AddShortcut(new ShortcutManager.Shortcut("toggle logic edit mode") {
                activation = new ShortcutManager.ActivationRule() {
                    key = KeyCode.S,
                },
                onPress = ToggleLogicEditMode
            });
        }
        private bool ToggleLogicEditMode() {
            _logicEditModeEnabled = !_logicEditModeEnabled;
            return true;
        }
        public void OnSceneGUI() {
            _shortcutManager.HandleInput();
            if (!_logicEditModeEnabled) return;
            SetupConnectionFireFeedbackActions();
            HandleTheHandle();
        }
        public void Reset() {
            _lastOutputFireTimes.Clear();
        }
        private void SetupConnectionFireFeedbackActions() {
            foreach (var lapsComponent in _editor.allComponents) {
                lapsComponent.OutputFired -= OutputFired;
                lapsComponent.OutputFired += OutputFired;
            }
        }
        private void OutputFired(LapsComponent lapsComponent, int slotId) {
            var key = new OutputFireTimingKey() {
                lapsComponent = lapsComponent,
                slotId = slotId,
            };
            if (_lastOutputFireTimes.ContainsKey(key)) {
                _lastOutputFireTimes[key] = Time.time;
            }
            else {
                _lastOutputFireTimes.Add(key, Time.time);
            }
        }
        private void HandleTheHandle() {
            int id = GUIUtility.GetControlID(HandleHintHash, FocusType.Passive);
            switch (Event.current.GetTypeForControl(id)) {
                case EventType.MouseDown:
                    if (HandleUtility.nearestControl == id) {
                        GUIUtility.hotControl = id;
                        Event.current.Use();
                        EditorGUIUtility.SetWantsMouseJumping(1);
                        OnMouseDown();
                    }
                    break;
                case EventType.MouseUp:
                    if (id == GUIUtility.hotControl) {
                        GUIUtility.hotControl = 0;
                        Event.current.Use();
                        EditorGUIUtility.SetWantsMouseJumping(0);
                        OnMouseUp();
                    }
                    break;
                case EventType.MouseDrag:
                    if (id == GUIUtility.hotControl) {
                        Event.current.Use();
                        // OnMouseDrag();
                    }
                    break;
                case EventType.Repaint:
                    OnRepaint();
                    break;
                case EventType.Layout:
                    var distance = GetNearestDistanceFromPointToAnySlot(Event.current.mousePosition);
                    HandleUtility.AddControl(id, distance);
                    break;
            }
        }
        private void OnMouseDown() {
            _dragging = TryGetHoveredSlot(Event.current.mousePosition, out _draggingSlot);
        }
        private void OnMouseUp() {
            if (!_dragging) return;
            _dragging = false;
            if (!TryGetHoveredSlot(Event.current.mousePosition, out var releasedSlot)) return;
            if (Equals(_draggingSlot, releasedSlot)) {
                RemoveConnectionOfSlot(_draggingSlot);
            }
            else {
                if (!CanConnect(_draggingSlot, releasedSlot)) return;
                var sourceSlotInformation = _draggingSlot.isTarget ? releasedSlot : _draggingSlot;
                var targetSlotInformation = _draggingSlot.isTarget ? _draggingSlot : releasedSlot;
                Connect(
                    sourceSlotInformation.lapsComponent,
                    sourceSlotInformation.LogicSlot.id,
                    targetSlotInformation.lapsComponent,
                    targetSlotInformation.LogicSlot.id);
            }
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
        private void RemoveConnectionOfSlot(SlotInformation slotInformation) {
            if (!slotInformation.isTarget) {
                var connections = slotInformation.lapsComponent.connections;
                for (var i = 0; i < connections.Count; i++) {
                    if (connections[i].sourceSlotId == slotInformation.LogicSlot.id) {
                        connections.RemoveAt(i);
                        return;
                    }
                }
            }
            else {
                foreach (var lapsComponent in _editor.allComponents) {
                    for (int i = 0; i < lapsComponent.connections.Count; i++) {
                        var connection = lapsComponent.connections[i];
                        if (connection.targetComponent == slotInformation.lapsComponent && connection.targetSlotId == slotInformation.LogicSlot.id){
                            lapsComponent.connections.RemoveAt(i);
                            return;
                        }
                    }
                }
            }
        }
        private bool CanConnect(SlotInformation slot1, SlotInformation slot2) {
            if (slot1.isTarget == slot2.isTarget) return false;
            var targetSlot = slot1.isTarget ? slot1 : slot2;
            var sourceSlot = slot1.isTarget ? slot2 : slot1;
            return CanConnect(sourceSlot.lapsComponent, sourceSlot.LogicSlot.id, targetSlot.lapsComponent,targetSlot.LogicSlot.id);
        }
        private bool CanConnect(LapsComponent sourceComponent, int sourceSlotId, LapsComponent targetComponent, int targetSlotId) {
            return true;
        }
        private bool ConnectionExists(LapsComponent sourceComponent, int sourceSlotId, LapsComponent targetComponent, int targetSlotId) {
            foreach (var connection in sourceComponent.connections) {
                if (connection.sourceSlotId == sourceSlotId && connection.targetSlotId == targetSlotId && connection.targetComponent == targetComponent) {
                    return true;
                }
            }
            return false;
        }
        public void OnRepaint() {
            using (Scopes.HandlesGUI()) {
                DrawAllSlots();
                DrawAllConnections();
                DrawDraggingAndHoverConnection();
                DrawLabels();
            }
            SceneView.RepaintAll();
        }
        private void DrawAllSlots() {
            _slotInformationCacheDictionary.Clear();
            foreach (var lapsComponent in _editor.allComponents) {
                _slots.Clear();
                lapsComponent.GetInputSlots(_slots);
                for (int i = 0; i < _slots.Count; i++) {
                    var slotInformation = new SlotInformation(lapsComponent, true, _slots[i], i);
                    _slotInformationCacheDictionary.Add(new SlotInformationCacheKey(lapsComponent, true, _slots[i].id), slotInformation);
                    DrawSlot(slotInformation);
                }
                _slots.Clear();
                lapsComponent.GetOutputSlots(_slots);
                for (int i = 0; i < _slots.Count; i++) {
                    var slotInformation = new SlotInformation(lapsComponent, false, _slots[i], i);
                    _slotInformationCacheDictionary.Add(new SlotInformationCacheKey(lapsComponent, false, _slots[i].id), slotInformation);
                    DrawSlot(slotInformation);
                }
            }
        }
        private void DrawSlot(SlotInformation slotInformation) {
            var position = GetScreenPositionOfSlot(slotInformation);
            if (!IsSlotPositionInsideSceneView(position)) return;
            position.x = Mathf.Floor(position.x);
            position.y = Mathf.Floor(position.y);
            var parameterColor = slotInformation.LogicSlot.GetSlotParameterColor();
            var returnColor = slotInformation.LogicSlot.GetSlotReturnColor();
            using (Scopes.HandlesColor(Color.white)) {
                Handles.DrawSolidRectangleWithOutline(new Rect(position - Vector2.one * SlotRadius, Vector2.one* SlotRadius * 2), Color.black, Color.clear);
                Handles.DrawSolidRectangleWithOutline(new Rect(position - Vector2.one * SlotRadius*.8f, new Vector2(SlotRadius * .8f, SlotRadius * 2 * .8f)), parameterColor, Color.clear);
                Handles.DrawSolidRectangleWithOutline(new Rect(new Vector2(position.x, position.y - SlotRadius * .8f), new Vector2(SlotRadius * .8f, SlotRadius * 2 * .8f)), returnColor, Color.clear);
            }
        }
        private Vector2 GetScreenPositionOfSlot(SlotInformation slotInformation) {
            var worldPoint = slotInformation.lapsComponent.transform.position;
            var screenPosition =  HandleUtility.WorldToGUIPoint(worldPoint);
            screenPosition -= new Vector2((slotInformation.isTarget ? 1: -1) * ((LapsEditorSelectionModule.SelectionIconSize.x / 2) + SlotRadius), LapsEditorSelectionModule.SelectionIconSize.y / 2 - SlotMargin);
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
            foreach (var lapsComponent in _editor.allComponents) {
                foreach (var connection in lapsComponent.connections) {
                    if (!TryGetDrawInformation(lapsComponent, false, connection.sourceSlotId, out var sourceDrawInformation)) continue;
                    if (!TryGetDrawInformation(connection.targetComponent, true, connection.targetSlotId, out var targetDrawInformation)) continue;
                    var sourcePosition = GetScreenPositionOfSlot(sourceDrawInformation);
                    var destinationPosition = GetScreenPositionOfSlot(targetDrawInformation);
                    var color = GetConnectionColor(sourceDrawInformation, targetDrawInformation);
                    var backgroundColor = GetConnectionBackgroundColor(sourceDrawInformation, targetDrawInformation);
                    DrawConnection(sourcePosition, destinationPosition, color, backgroundColor);
                }
            }
        }
        private void DrawConnection(SlotInformation slot1, SlotInformation slot2) {
            var targetSlot = slot1.isTarget ? slot1 : slot2;
            var sourceSlot = slot1.isTarget ? slot2 : slot1;
            var color = GetConnectionColor(sourceSlot, targetSlot);
            var backgroundColor = GetConnectionBackgroundColor(sourceSlot, targetSlot);
            DrawConnection(GetScreenPositionOfSlot(sourceSlot),GetScreenPositionOfSlot(targetSlot), color, backgroundColor);
        }
        private void DrawConnection(Vector2 sourcePosition, Vector2 targetPosition, Color color, Color backgroundColor) {
            var sourceDrawPosition = sourcePosition + Vector2.right * (SlotRadius * .9f);
            var targetDrawPosition = targetPosition - Vector2.right * (SlotRadius * .9f);
            Handles.DrawBezier(
                sourceDrawPosition,
                targetDrawPosition,
                sourceDrawPosition + Vector2.right * 80f,
                targetDrawPosition + Vector2.left * 80f,
                backgroundColor,
                null,
                10f);
            Handles.DrawBezier(
                sourceDrawPosition,
                targetDrawPosition,
                sourceDrawPosition + Vector2.right * 80f,
                targetDrawPosition + Vector2.left * 80f,
                color,
                null,
                4f);
        }
        private Color GetConnectionColor(SlotInformation sourceSlot, SlotInformation targetSlot) {
            if (!CanConnect(sourceSlot, targetSlot)) return NonConnectableConnectionColor;
            var key = new OutputFireTimingKey() {
                lapsComponent = sourceSlot.lapsComponent,
                slotId = sourceSlot.LogicSlot.id,
            };
            if (!_lastOutputFireTimes.TryGetValue(key, out float value)) return ConnectableConnectionColor;
            var elapsed = Time.time - value;
            if (elapsed > FireAnimationDuration) {
                _lastOutputFireTimes.Remove(key);
                return ConnectableConnectionColor;
            }
            return Color.Lerp(ConnectionJustFiredColor, ConnectableConnectionColor, elapsed / FireAnimationDuration);
        }
        private Color GetConnectionBackgroundColor(SlotInformation sourceSlot, SlotInformation targetSlot) {
            var key = new OutputFireTimingKey() {
                lapsComponent = sourceSlot.lapsComponent,
                slotId = sourceSlot.LogicSlot.id,
            };
            if (!_lastOutputFireTimes.TryGetValue(key, out float value)) return ConnectionBackgroundColor;
            var elapsed = Time.time - value;
            if (elapsed > FireAnimationDuration) {
                _lastOutputFireTimes.Remove(key);
                return ConnectableConnectionColor;
            }
            return Color.Lerp(ConnectionJustFiredBackgroundColor, ConnectionBackgroundColor, elapsed / FireAnimationDuration);
        }
        private bool TryGetDrawInformation(LapsComponent lapsComponent, bool isInput, int connectionSourceSlotId, out SlotInformation slotInformation) {
            var key = new SlotInformationCacheKey(lapsComponent, isInput, connectionSourceSlotId);
            return _slotInformationCacheDictionary.TryGetValue(key, out slotInformation);
        }
        private void DrawDraggingAndHoverConnection() {
            if (!_dragging) return;
            if (TryGetHoveredSlot(Event.current.mousePosition, out var hoveredSlot)) {
                if (!Equals(_draggingSlot, hoveredSlot)) {
                    DrawConnection(_draggingSlot, hoveredSlot);
                }
                else {
                    //todo draw would be deleted connection in red 
                }
            }
            else {
                if (_draggingSlot.isTarget) {
                    DrawConnection(Event.current.mousePosition, GetScreenPositionOfSlot(_draggingSlot), DanglingConnectionColor, ConnectionBackgroundColor);
                }
                else {
                    DrawConnection(GetScreenPositionOfSlot(_draggingSlot), Event.current.mousePosition, DanglingConnectionColor, ConnectionBackgroundColor);
                }
            }
        }
        private bool TryGetHoveredSlot(Vector2 point, out SlotInformation slotInformation) {
            foreach (var pair in _slotInformationCacheDictionary) {
                var rect = GetSlotRect(pair.Value);
                var distance = LapsMath.DistanceFromPointToRect(rect, point);
                if (distance <= .01f) {
                    slotInformation = pair.Value;
                    return true;
                }
            }
            slotInformation = default;
            return false;
        }
        private float GetNearestDistanceFromPointToAnySlot(Vector2 point) {
            var nearestDistance = float.MaxValue;
            foreach (var lapsComponent in _editor.allComponents) {
                _slots.Clear();
                lapsComponent.GetInputSlots(_slots);
                for (int i = 0; i < _slots.Count; i++) {
                    var slotInformation = new SlotInformation(lapsComponent, true, _slots[i], i);
                    var rect = GetSlotRect(slotInformation);
                    var distance = LapsMath.DistanceFromPointToRect(rect, point);
                    if (distance < nearestDistance) {
                        nearestDistance = distance;
                    }
                }
                _slots.Clear();
                lapsComponent.GetOutputSlots(_slots);
                for (int i = 0; i < _slots.Count; i++) {
                    var slotInformation = new SlotInformation(lapsComponent, false, _slots[i], i);
                    var rect = GetSlotRect(slotInformation);
                    var distance = LapsMath.DistanceFromPointToRect(rect, point);
                    if (distance < nearestDistance) {
                        nearestDistance = distance;
                    }
                }
            }
            return nearestDistance;
        }
        private Rect GetSlotRect(SlotInformation slotInformation) {
            var position = GetScreenPositionOfSlot(slotInformation);
            return new Rect(position - Vector2.one * SlotRadius, Vector2.one * SlotRadius * 2);
        }
        private void DrawLabels() {
            if (TryGetHoveredSlot(Event.current.mousePosition, out var slotInformation)) {
                var screenPos = GetScreenPositionOfSlot(slotInformation)
                                + (slotInformation.isTarget ? Vector2.left : Vector2.right) * SlotRadius;
                var name = slotInformation.LogicSlot.name;
                var style = new GUIStyle(GUIStyle.none);
                style.padding = new RectOffset(4, 4, 1, 1);
                var size = style.CalcSize(new GUIContent(name));
                if (slotInformation.isTarget) {
                    screenPos.x -= size.x;
                }
                screenPos.y -= size.y / 2f;
                var rect = new Rect(screenPos, size);
                EditorGUI.DrawRect(rect, Color.gray);
                GUI.Label(rect, slotInformation.LogicSlot.name, style);
            }
        }
        private struct SlotInformation {
            public LapsComponent lapsComponent;
            public bool isTarget;
            public LogicSlot LogicSlot;
            public int index;
            public SlotInformation(LapsComponent lapsComponent, bool isTarget, LogicSlot logicSlot, int index) {
                this.lapsComponent = lapsComponent;
                this.isTarget = isTarget;
                this.LogicSlot = logicSlot;
                this.index = index;
            }
        }
        private struct SlotInformationCacheKey {
            public LapsComponent lapsComponent;
            public bool isInput;
            public int slotId;
            public SlotInformationCacheKey(LapsComponent lapsComponent, bool isInput, int slotId) {
                this.lapsComponent = lapsComponent;
                this.isInput = isInput;
                this.slotId = slotId;
            }
        }
    }
}
