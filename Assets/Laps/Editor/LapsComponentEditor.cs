using LapsRuntime;
using UnityEditor;
using UnityEngine;

namespace LapsEditor {
    [CustomEditor(typeof(LapsComponent), true)]
    [CanEditMultipleObjects]
    public class LapsComponentEditor : Editor {
        private LapsComponent targetComponent;
        private void OnEnable() {
            targetComponent = (LapsComponent) target;
        }
        private void OnSceneGUI() {
            Handles.DrawLine(targetComponent.transform.position, targetComponent.transform.position + Vector3.one);
        }
    }
}
