using LapsRuntime;
using UnityEditor;

namespace LapsEditor {
    [CustomEditor(typeof(LapsComponent), true)]
    public class LapsComponentEditor : Editor {
        private LapsComponent targetComponent;
        private void OnEnable() {
            targetComponent = (LapsComponent) target;
        }
        private void OnSceneGUI() {
            // Handles.DrawLine(targetComponent.transform.position, targetComponent.transform.position + Vector3.one);
        }
    }
}
