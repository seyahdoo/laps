using UnityEditor;
using UnityEngine;

namespace LapsEditor {
    public static class CustomHandle {
        private static readonly int DragHandleHintHash = nameof(CustomHandle).GetHashCode();
        public delegate void DrawFunction(bool isHotControl, bool isClosestHandle);
        public delegate float DistanceFunction();
        public delegate bool MouseDownFunction();
        public delegate void MouseUpFunction();
        public static void Draw(
            DrawFunction drawFunction,
            DistanceFunction distanceFunction,
            MouseDownFunction mouseDownFunction,
            MouseUpFunction mouseUpFunction) {
            
            int id = GUIUtility.GetControlID(DragHandleHintHash, FocusType.Passive);
            switch (Event.current.GetTypeForControl(id)) {
               case EventType.MouseDown:
                   if (HandleUtility.nearestControl == id) {
                       if (mouseDownFunction.Invoke()) {
                           GUIUtility.hotControl = id;
                           Event.current.Use();
                           EditorGUIUtility.SetWantsMouseJumping(1);
                       }
                   }
                   break;
               case EventType.MouseUp:
                   if (id == GUIUtility.hotControl) {
                       GUIUtility.hotControl = 0;
                       Event.current.Use();
                       EditorGUIUtility.SetWantsMouseJumping(0);
                       mouseUpFunction?.Invoke();
                   }
                   break;
               case EventType.MouseDrag:
                   if (id == GUIUtility.hotControl) {
                       Event.current.Use();
                   }
                   break;
               case EventType.Repaint:
                   drawFunction?.Invoke(id == GUIUtility.hotControl, HandleUtility.nearestControl == id);
                   break;
               case EventType.Layout:
                   var distance = distanceFunction != null ? distanceFunction.Invoke() : float.MaxValue;
                   HandleUtility.AddControl(id, distance);
                   break;
            }
        }
    }
}
