using UnityEditor;

namespace LapsEditor {
    public static class LapsEditor {
        public static LapsEditorSelectionModule lapsEditorSelectionModule;
        public static LapsEditorLogicModule lapsEditorLogicModule;
        static LapsEditor () {
            lapsEditorSelectionModule = new LapsEditorSelectionModule();
            lapsEditorLogicModule = new LapsEditorLogicModule();
            SceneView.duringSceneGui += SceneGUI;
        }
        private static void SceneGUI(SceneView obj) {
            lapsEditorSelectionModule.OnSceneGUI();
            lapsEditorLogicModule.OnSceneGUI();
        }
    }
}
