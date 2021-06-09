using UnityEditor;

namespace LapsEditor {
    public static class LapsEditor {
        private static LapsEditorSelectionModule _lapsEditorSelectionModule;
        private static LapsEditorLogicModule _lapsEditorLogicModule;
        static LapsEditor () {
            _lapsEditorSelectionModule = new LapsEditorSelectionModule();
            _lapsEditorLogicModule = new LapsEditorLogicModule();
            SceneView.duringSceneGui += SceneGUI;
        }
        private static void SceneGUI(SceneView obj) {
            _lapsEditorSelectionModule.OnSceneGUI();
            _lapsEditorLogicModule.OnSceneGUI();
        }
    }
}
