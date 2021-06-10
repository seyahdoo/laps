using LapsRuntime;
using UnityEditor;
using UnityEngine;

namespace LapsEditor {
    [CustomEditor(typeof(DebugComponent))]
    public class DebugComponentEditor : LapsComponentEditor {
        public override void OnInspectorGUI() {
            base.OnInspectorGUI();
            if (GUILayout.Button("Fire Event")) {
                ((DebugComponent) target).FireDebugEvent();
            }
        }
    }
}
