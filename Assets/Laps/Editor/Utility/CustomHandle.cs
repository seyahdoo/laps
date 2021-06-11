using UnityEditor;
using UnityEngine;

namespace LapsEditor {
    public static class CustomHandle {
        //thanks to higekun @ https://answers.unity.com/questions/463207/how-do-you-make-a-custom-handle-respond-to-the-mou.html
        private static readonly int DragHandleHintHash = nameof(CustomHandle).GetHashCode();
        public delegate void DrawFunction(bool isHotControl, bool isClosestHandle);
        public delegate float DistanceFunction();
        public delegate void MouseDownFunction();
        public delegate void MouseUpFunction();
        public delegate void MouseDragFunction();
        public static void Draw(
            DrawFunction drawFunction,
            DistanceFunction distanceFunction,
            MouseDownFunction mouseDownFunction,
            MouseUpFunction mouseUpFunction,
            MouseDragFunction mouseDragFunction = null) {

            int id = GUIUtility.GetControlID(DragHandleHintHash, FocusType.Passive);
            switch (Event.current.GetTypeForControl(id)) {
               case EventType.MouseDown:
                   if (HandleUtility.nearestControl == id) {
                       GUIUtility.hotControl = id;
                       Event.current.Use();
                       EditorGUIUtility.SetWantsMouseJumping(1);
                       mouseDownFunction?.Invoke();
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
                       mouseDragFunction?.Invoke();
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
