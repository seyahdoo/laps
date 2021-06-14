using System;
using System.Collections.Generic;
using LapsRuntime;
using UnityEditor;
using Object = UnityEngine.Object;

namespace LapsEditor {
    public class LapsEditor {
        public LapsEditorSelectionModule lapsEditorSelectionModule;
        public LapsEditorLogicModule lapsEditorLogicModule;
        public LapsComponent[] allComponents;
        
        public static LapsEditor instance;
        [InitializeOnLoadMethod]
        private static void Initialize() {
            if (instance == null) {
                instance = new LapsEditor();
            }
        }
        public LapsEditor() {
            lapsEditorSelectionModule = new LapsEditorSelectionModule(this);
            lapsEditorLogicModule = new LapsEditorLogicModule(this);
            SceneView.duringSceneGui += SceneGUI;
            EditorApplication.playModeStateChanged += EditorApplicationOnPlayModeStateChanged;
        }
        private void EditorApplicationOnPlayModeStateChanged(PlayModeStateChange state) {
            if (state == PlayModeStateChange.ExitingEditMode || state == PlayModeStateChange.ExitingPlayMode) {
                lapsEditorLogicModule.Reset();
            }
        }
        private void SceneGUI(SceneView obj) {
            //todo optimize this
            allComponents = Object.FindObjectsOfType<LapsComponent>(false);
            lapsEditorSelectionModule.OnSceneGUI();
            lapsEditorLogicModule.OnSceneGUI();
        }
    }
}
