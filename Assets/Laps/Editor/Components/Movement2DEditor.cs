using LapsEditor.Utility;
using LapsRuntime;
using UnityEditor;
using UnityEngine;

namespace LapsEditor {
    [CustomEditor(typeof(Movement2D))]
    public class Movement2DEditor : Editor {
        private bool _editing;
        private PathEditor _pathEditor;
        private Movement2D _movement;
        private void OnEnable() {
            _movement = (Movement2D) target;
            _pathEditor = new PathEditor(_movement.path);
        }
        public override void OnInspectorGUI() {
            base.OnInspectorGUI();
            if (GUILayout.Button((_editing ? "Exit" : "Enter") + " Edit Mode")) {
                _editing = !_editing;
                Repaint();
                SceneView.RepaintAll();
            }
        }
        private void OnSceneGUI() {
            if (_editing) {
                using (Scopes.HandlesMatrix(_movement.transform.localToWorldMatrix)) {
                    _pathEditor.OnSceneGUI();
                }
            }
        }
    }
}
