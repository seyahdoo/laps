using LapsRuntime;
using UnityEditor;
using UnityEngine;

namespace LapsEditor {
    public static class IconHandle {
        private static readonly int HandleHintHash = nameof(IconHandle).GetHashCode();
        private static readonly Color SelectionIconSelectableColor = new Color(1f, 1f, 1f, 0.5f);
        private static readonly Color SelectionIconSelectedColor = new Color(1f, 1f, 1f, 0.9f);
        private static readonly Color SelectionIconHighlightedColor = new Color(1f, 1f, 1f, 0.7f);
        private static readonly Color SelectionIconErrorColor = new Color(1f, 0f, 0f, 1);
        private static Vector2 _mouseStart;
        private static Vector2 _mouseCurrent;
        private static bool _dragged = false;
        private static Vector3 _startWorldPosition;

        public static void Draw(LapsComponent lapsComponent, Texture texture, Rect rect) {
            int id = GUIUtility.GetControlID(HandleHintHash, FocusType.Passive);
            switch (Event.current.GetTypeForControl(id)) {
               case EventType.MouseDown:
                   if (Event.current.button == 0 && HandleUtility.nearestControl == id) {
                       GUIUtility.hotControl = id;
                       Event.current.Use();
                       EditorGUIUtility.SetWantsMouseJumping(1);
                       OnMouseDown(lapsComponent);
                   }
                   break;
               case EventType.MouseUp:
                   if (id == GUIUtility.hotControl) {
                       GUIUtility.hotControl = 0;
                       Event.current.Use();
                       EditorGUIUtility.SetWantsMouseJumping(0);
                       OnMouseUp(lapsComponent);
                   }
                   break;
               case EventType.MouseDrag:
                   if (id == GUIUtility.hotControl) {
                       Event.current.Use();
                       OnMouseDrag(lapsComponent);
                   }
                   break;
               case EventType.Repaint:
                   var isHotControl = id == GUIUtility.hotControl;
                   var isNearestControl = HandleUtility.nearestControl == id;
                   OnRepaint(
                       lapsComponent,
                       isHotControl, 
                       isNearestControl, 
                       texture,
                       rect);
                   break;
               case EventType.Layout:
                   var distance = CalculateDistance(rect);
                   HandleUtility.AddControl(id, distance);
                   break;
            }
        }
        private static float CalculateDistance(Rect rect) {
            var actualDistance = LapsMath.DistanceFromPointToRect(rect, Event.current.mousePosition);
            var distance = actualDistance <= .01f ? 0f: float.MaxValue;
            return distance;
        }
        private static void OnRepaint(LapsComponent lapsComponent, bool isHotControl, bool isNearestControl, Texture iconTexture, Rect rect) {
            var color = GetHandleColorForLapsComponent(lapsComponent, isHotControl, isNearestControl);
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
        private static void OnMouseDrag(LapsComponent lapsComponent) {
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
        }
        private static void OnMouseUp(LapsComponent lapsComponent) {
            if (!_dragged) {
                Selection.activeGameObject = lapsComponent.gameObject;
            }
        }
        private static void OnMouseDown(LapsComponent lapsComponent) {
            _mouseStart = Event.current.mousePosition;
            _mouseCurrent = _mouseStart;
            _startWorldPosition = lapsComponent.transform.position;
            _dragged = false;
        }
        private static Color GetHandleColorForLapsComponent(LapsComponent lapsComponent, bool isHotControl, bool isNearestControl) {
            if (lapsComponent.ErrorExists) return SelectionIconErrorColor;
            if (Selection.Contains(lapsComponent.gameObject)) return SelectionIconSelectedColor;
            if (isHotControl) return SelectionIconHighlightedColor;
            if (isNearestControl) return SelectionIconHighlightedColor;
            return SelectionIconSelectableColor;
        }
    }
}
