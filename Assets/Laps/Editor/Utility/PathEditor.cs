using LapsRuntime;
using UnityEditor;
using UnityEngine;

namespace LapsEditor { 
    public class PathEditor {
        private static readonly int HandleHintHash = nameof(PathEditor).GetHashCode();
        private Path _path;
        public PathEditor(Path path) {
            _path = path;
        }
        public void OnSceneGUI() {
            int id = GUIUtility.GetControlID(HandleHintHash, FocusType.Passive);
            switch (Event.current.GetTypeForControl(id)) {
                case EventType.MouseDown:
                    if (Event.current.button == 0 && HandleUtility.nearestControl == id) {
                        GUIUtility.hotControl = id;
                        OnMouseDown();
                        Event.current.Use();
                        EditorGUIUtility.SetWantsMouseJumping(1);
                    }
                    break;
                case EventType.MouseUp:
                    if (id == GUIUtility.hotControl) {
                        GUIUtility.hotControl = 0;
                        OnMouseUp();
                        Event.current.Use();
                        EditorGUIUtility.SetWantsMouseJumping(0);
                    }
                    break;
                case EventType.MouseDrag:
                    if (id == GUIUtility.hotControl) {
                        OnMouseDrag();
                        Event.current.Use();
                    }
                    break;
                case EventType.Repaint:
                    var isHotControl = id == GUIUtility.hotControl;
                    var isNearestControl = HandleUtility.nearestControl == id;
                    OnRepaint(isHotControl, isNearestControl);
                    break;
                case EventType.Layout:
                    var distance = CalculateDistance();
                    HandleUtility.AddControl(id, distance);
                    break;
            }
        }
        private float CalculateDistance() {
            return 100f;
        }
        private void OnRepaint(bool isHotControl, bool isNearestControl) {
            
        }
        private void OnMouseDrag() {
        }
        private void OnMouseUp() {
        }
        private void OnMouseDown() {
        }
    }
}
