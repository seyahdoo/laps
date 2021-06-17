using LapsRuntime;
using UnityEditor;

namespace LapsEditor {
    public static class EditorCoreCommons {
        public static bool ShoudDrawNormal(LapsComponent lapsComponent) {
            var activeTransform = Selection.activeTransform;
            if (activeTransform == null) {
                if (lapsComponent.transform.parent == null) {
                    return true;
                }
                else {
                    return false;
                }
            }
            else {
                if (lapsComponent.transform.parent == activeTransform.parent) {
                    return true;
                }
            }
            return false;
        }
        public static bool ShoudDrawCompoundInside(CompoundComponent compoundComponent) {
            var activeTransform = Selection.activeTransform;
            if (activeTransform == null) return false;
            if (compoundComponent.transform == activeTransform.parent) {
                return true;
            }
            return false;
        }
    }
}
