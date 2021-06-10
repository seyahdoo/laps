using LapsEditor.Utility;
using LapsRuntime;
using UnityEditor;
using UnityEngine;

namespace LapsEditor {
    [CustomPropertyDrawer(typeof(NonEditableOnInspectorAttribute))]
    public class NonEditableOnInspectorAttributeDrawer : PropertyDrawer {
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label) {
            return EditorGUI.GetPropertyHeight(property, label, true);
        }
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
            using (Scopes.GUIEnabled(false)) {
                EditorGUI.PropertyField(position, property, label, true);
            }
        }
    }
}
