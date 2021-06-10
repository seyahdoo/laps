using LapsEditor.Utility;
using LapsRuntime;
using UnityEditor;
using UnityEngine;

namespace LapsEditor {
    public class LapsEditorSelectionModule {
        internal static readonly Vector2 SelectionIconSize = new Vector2(30f, 30f);
        private static readonly Color SelectionIconSelectableColor = new Color(1f, 1f, 1f, 0.5f);
        private static readonly Color SelectionIconSelectedColor = new Color(1f, 1f, 1f, 0.9f);
        private static readonly Color SelectionIconHighlightedColor = new Color(1f, 1f, 1f, 0.7f);
        private static readonly Color SelectionIconErrorColor = new Color(1f, 0f, 0f, 1);
        private Vector2 _mouseStart;
        private Vector2 _lastMousePosition;
        private Vector2 _mouseCurrent;
        private bool _dragged = false;
        private LapsEditor _editor;
        private Vector3 _startWorldPosition;
        public LapsEditorSelectionModule(LapsEditor lapsEditor) {
            _editor = lapsEditor;
        }
        public void OnSceneGUI() {
            using (Scopes.HandlesGUI()) {
                DrawLapsIcons();
            }
        }
        private void DrawLapsIcons() {
            foreach (var lapsComponent in _editor.allComponents) {
                DrawLapsIcon(lapsComponent);
            }
        }
        private void DrawLapsIcon(LapsComponent lapsComponent) {
            var rect = GetScreenSelectionRect(lapsComponent);
            CustomHandle.Draw((isHotControl, isClosestHandle) => {
                var iconTexture = GetIconTexture(lapsComponent);
                var color = GetHandleColorForLapsComponent(lapsComponent, isHotControl, isClosestHandle);
                GUI.DrawTexture(
                    rect,
                    iconTexture,
                    ScaleMode.StretchToFill,
                    true,
                    0.0f,
                    color,
                    0.0f,
                    0.0f);
            },() => {
                var actualDistance = LapsMath.DistanceFromPointToRect(rect, Event.current.mousePosition);
                var distance = actualDistance <= .01f ? 0f: float.MaxValue;
                return distance;
            },(() => {
                _mouseStart = Event.current.mousePosition;
                _mouseCurrent = _mouseStart;
                _startWorldPosition = lapsComponent.transform.position;
                _dragged = false;
            }),() => {
                if (!_dragged) {
                    Selection.activeGameObject = lapsComponent.gameObject;
                }
            }, () => {
                _dragged = true;
                _mouseCurrent += new Vector2(Event.current.delta.x, -Event.current.delta.y);
                var position2 = Camera.current.WorldToScreenPoint(Handles.matrix.MultiplyPoint(_startWorldPosition))
                                + (Vector3)(_mouseCurrent - _mouseStart);
                var position = Handles.matrix.inverse.MultiplyPoint(Camera.current.ScreenToWorldPoint(position2));
 
                if (Camera.current.transform.forward == Vector3.forward || Camera.current.transform.forward == -Vector3.forward)
                    position.z = _startWorldPosition.z;
                if (Camera.current.transform.forward == Vector3.up || Camera.current.transform.forward == -Vector3.up)
                    position.y = _startWorldPosition.y;
                if (Camera.current.transform.forward == Vector3.right || Camera.current.transform.forward == -Vector3.right)
                    position.x = _startWorldPosition.x;

                lapsComponent.transform.position = position;
                GUI.changed = true;
            });
        }
        private Color GetHandleColorForLapsComponent(LapsComponent lapsComponent, bool isHotControl, bool isClosestHandle) {
            if (lapsComponent.ErrorExists) return SelectionIconErrorColor;
            if (Selection.Contains(lapsComponent.gameObject)) return SelectionIconSelectedColor;
            if (isHotControl) return SelectionIconHighlightedColor;
            if (isClosestHandle) return SelectionIconHighlightedColor;
            return SelectionIconSelectableColor;
        }
        private Rect GetScreenSelectionRect(LapsComponent lapsComponent) {
            var rect = new Rect(GetSelectionRectPosition(lapsComponent) - SelectionIconSize / 2f, SelectionIconSize);
            return rect;
        }
        private Vector2 GetSelectionRectPosition(LapsComponent lapsComponent) {
            var screenPosition = GetScreenPosition(lapsComponent);
            if (lapsComponent.ErrorExists) {
                if (screenPosition.x < SelectionIconSize.x / 2f) {
                    screenPosition.x = SelectionIconSize.x / 2f;
                }
                if (screenPosition.x > Screen.width - SelectionIconSize.x / 2f) {
                    screenPosition.x = Screen.width - SelectionIconSize.x / 2f;
                }
                if (screenPosition.y < SelectionIconSize.y / 2f) {
                    screenPosition.y = SelectionIconSize.y / 2f;
                }
                if (screenPosition.y > Screen.height - 40 - SelectionIconSize.y / 2f) {
                    screenPosition.y = Screen.height - 40 - SelectionIconSize.y / 2f;
                }
            }
            return screenPosition;
        }
        private Vector2 GetScreenPosition(LapsComponent lapsComponent) {
            return WorldToScreenPosition(lapsComponent.transform.position);
        }
        private Vector2 WorldToScreenPosition(Vector3 worldPoint) {
            return HandleUtility.WorldToGUIPoint(worldPoint);
        }
        private Texture GetIconTexture(LapsComponent lapsComponent) {
            //todo cache this
            return Resources.Load<Texture>("lapsobject-icon");
        }
    }
}