using System.Collections.Generic;
using LapsRuntime;
using UnityEditor;
using UnityEngine;

namespace LapsEditor {
    public class LapsEditor {
        public LapsEditorSelectionModule lapsEditorSelectionModule;
        public LapsEditorLogicModule lapsEditorLogicModule;
        public readonly List<LapsComponent> allComponents = new List<LapsComponent>();
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
            EditorApplication.hierarchyChanged += EditorApplicationOnHierarchyChanged;
        }
        private void EditorApplicationOnHierarchyChanged() {
            //todo optimize this
            allComponents.Clear();
            var allComps = Object.FindObjectsOfType<LapsComponent>();
            allComponents.AddRange(allComps);
        }
        private void SceneGUI(SceneView obj) {
            lapsEditorSelectionModule.OnSceneGUI();
            lapsEditorLogicModule.OnSceneGUI();
        }
    }
}
