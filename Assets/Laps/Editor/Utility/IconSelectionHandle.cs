using UnityEditor;
using UnityEngine;

namespace LapsEditor {
    public class IconSelectionHandle {
        // internal state for DragHandle()
        static int s_DragHandleHash = nameof(IconSelectionHandle).GetHashCode();
        static Vector2 s_DragHandleMouseStart;
        static Vector2 s_DragHandleMouseCurrent;
        static Vector3 s_DragHandleWorldStart;
        static float s_DragHandleClickTime = 0;
        static int s_DragHandleClickID;
        static float s_DragHandleDoubleClickInterval = 0.5f;
        static bool s_DragHandleHasMoved;

        // externally accessible to get the ID of the most resently processed DragHandle
        public static int lastDragHandleID;

        public static void SelectionIconHandle(Vector3 position, Texture iconTexture, Vector2 size, Color passiveColor,
            Color hoverColor, Color selectedColor) {
            int id = GUIUtility.GetControlID(s_DragHandleHash, FocusType.Passive);
            lastDragHandleID = id;
            var screenPosition = Handles.matrix.MultiplyPoint(position);
            var cachedMatrix = Handles.matrix;
            switch (Event.current.GetTypeForControl(id)) {
                case EventType.MouseDown:
                    if (HandleUtility.nearestControl == id && Event.current.button == 0) {
                        GUIUtility.hotControl = id;
                        Event.current.Use();
                        EditorGUIUtility.SetWantsMouseJumping(1);
                    }
                    break;
                case EventType.MouseUp:
                    if (GUIUtility.hotControl == id && Event.current.button == 0) {
                        GUIUtility.hotControl = 0;
                        Event.current.Use();
                        EditorGUIUtility.SetWantsMouseJumping(0);
                    }
                    break;
                case EventType.MouseDrag:
                    if (GUIUtility.hotControl == id) {
                        Event.current.Use();
                    }
                    break;
                case EventType.Repaint:
                    Color currentColour = Handles.color;
                    if (id == GUIUtility.hotControl && s_DragHandleHasMoved)
                        Handles.color = colorSelected;
                    Handles.matrix = Matrix4x4.identity;
                    capFunc(id, screenPosition, Quaternion.identity, handleSize);
                    Handles.matrix = cachedMatrix;
                    Handles.color = currentColour;
                    break;
                case EventType.Layout:
                    Handles.matrix = Matrix4x4.identity;
                    HandleUtility.AddControl(id, HandleUtility.DistanceToCircle(screenPosition, handleSize));
                    Handles.matrix = cachedMatrix;
                    break;
            }
        }
    }
}