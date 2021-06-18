using LapsRuntime;
using UnityEditor;
using UnityEditor.SceneManagement;

namespace LapsEditor {
    public class LapsEditor {
        public static LapsEditor instance;
        public SelectionModule SelectionModule;
        public LogicModule LogicModule;
        public LapsComponent[] allComponents;
        
        [InitializeOnLoadMethod]
        private static void Initialize() {
            if (instance == null) {
                instance = new LapsEditor();
            }
        }
        public LapsEditor() {
            SelectionModule = new SelectionModule(this);
            LogicModule = new LogicModule(this);
            SceneView.duringSceneGui += SceneGUI;
            EditorApplication.playModeStateChanged += EditorApplicationOnPlayModeStateChanged;
        }
        private void EditorApplicationOnPlayModeStateChanged(PlayModeStateChange state) {
            if (state == PlayModeStateChange.ExitingEditMode || state == PlayModeStateChange.ExitingPlayMode) {
                LogicModule.Reset();
            }
        }
        private void SceneGUI(SceneView obj) {
            FindAllLapsComponents();
            SelectionModule.OnSceneGUI();
            LogicModule.OnSceneGUI();
        }
        private void FindAllLapsComponents() {
            //todo optimize this
            allComponents = StageUtility.GetCurrentStageHandle().FindComponentsOfType<LapsComponent>();
        }
    }
}
