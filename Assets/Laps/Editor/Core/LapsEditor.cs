using System.Collections.Generic;
using LapsRuntime;
using UnityEditor;
using UnityEngine;

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
        }
        private void SceneGUI(SceneView obj) {
            //todo optimize this
            allComponents = Object.FindObjectsOfType<LapsComponent>(false);
            lapsEditorSelectionModule.OnSceneGUI();
            lapsEditorLogicModule.OnSceneGUI();
        }
    }
}
