using UnityEngine;
using UnityEditor;
 
[InitializeOnLoad]
public class CreateLapsObjectMenu : Editor {
    static CreateLapsObjectMenu () {
        SceneView.duringSceneGui += SceneGUI;
    }
    private static void SceneGUI (SceneView sceneView) {
        if (Event.current.isKey && Event.current.keyCode == KeyCode.Space) {
            Event.current.Use();
            var menu = new GenericMenu();
            menu.AddItem(new GUIContent("hello/Item 1"), false, Callback, "hello");
            menu.AddItem(new GUIContent("hello/Item 2"), false, Callback, 2);
            menu.AddItem(new GUIContent("hello/Item 3"), false, Callback, 2);
            menu.AddItem(new GUIContent("Item 4"), false, Callback, 2);
            menu.AddItem(new GUIContent("Item 5"), false, Callback, 2);
            menu.ShowAsContext();
        }
    }
    private static void Callback (object obj) {
        Debug.Log(obj);
    }
}
