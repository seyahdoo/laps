using LapsEditor.Utility;
using LapsRuntime;
using UnityEditor;
using UnityEngine;

namespace LapsEditor {
    public class LapsEditorSelectionModule {
        private static readonly Vector2 SelectionIconSize = new Vector2(30f, 30f);
        private static readonly Color SelectionIconSelectableColor = new Color(1f, 1f, 1f, 0.5f);
        private static readonly Color SelectionIconSelectedColor = new Color(1f, 1f, 1f, 0.9f);
        private static readonly Color SelectionIconHighlightedColor = new Color(1f, 1f, 1f, 0.7f);
        private static readonly Color SelectionIconErrorColor = new Color(1f, 0f, 0f, 1);

        private LapsComponent[] _lapsComponents;
        public LapsEditorSelectionModule() {
            EditorApplication.hierarchyChanged += EditorApplicationOnHierarchyChanged;
            EditorApplicationOnHierarchyChanged();
        }
        public void OnSceneGUI() {
            DrawLapsIcons();
        }
        private void EditorApplicationOnHierarchyChanged() {
            //todo optimize this
            _lapsComponents = Object.FindObjectsOfType<LapsComponent>();
        }
        private void DrawLapsIcons() {
            foreach (var lapsComponent in _lapsComponents) {
                DrawLapsIcon(lapsComponent);
            }
        }
        private void DrawLapsIcon(LapsComponent lapsComponent) {
            var rect = GetScreenSelectionRect(lapsComponent);
            CustomHandle.Draw((isHotControl, isClosestHandle) => {
                using (Scopes.HandlesGUI()) {
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
                }
            },() => {
                var p = Event.current.mousePosition;
                //source MultiRRomero at https://stackoverflow.com/questions/5254838/calculating-distance-between-a-point-and-a-rectangular-box-nearest-point
                var dx = Mathf.Max(rect.min.x - p.x, 0, p.x - rect.max.x);
                var dy = Mathf.Max(rect.min.y - p.y, 0, p.y - rect.max.y);
                var actualDistance = Mathf.Sqrt(dx * dx + dy * dy);
                var distance = actualDistance <= .01f ? 0f: float.MaxValue;
                return distance;
            },null,() => {
                Selection.activeGameObject = lapsComponent.gameObject;
            }, null);
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