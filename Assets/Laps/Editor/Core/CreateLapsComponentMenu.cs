using System;
using System.Collections.Generic;
using LapsRuntime;
using UnityEngine;
using UnityEditor;

[InitializeOnLoad]
public class CreateLapsComponentMenu : Editor {
    static CreateLapsComponentMenu() {
        SceneView.duringSceneGui += SceneGUI;
    }
    private static Vector3 spawnPosition;
    private static void SceneGUI(SceneView sceneView) {
        if (Event.current.isKey && Event.current.keyCode == KeyCode.Space) {
            Event.current.Use();
            spawnPosition = GetObjectCreationPosition();
            var templates = GetTemplates();
            var menu = new GenericMenu();
            foreach (var template in templates) {
                menu.AddItem(new GUIContent(template.menuText), false, CreateLapsComponent, template);
            }
            menu.ShowAsContext();
        }
    }
    private static void CreateLapsComponent(object obj) {
        var template = (Template) obj;
        var go = new GameObject(template.type.Name);
        go.AddComponent(template.type);
        go.transform.position = spawnPosition;
    }
    private static Vector3 GetObjectCreationPosition() {
        Vector3 mousePosition = Event.current.mousePosition;
        var ray = HandleUtility.GUIPointToWorldRay(mousePosition);
        if (Physics.Raycast(ray, out var hit)) {
            return hit.point;
        }
        var pivot = SceneView.lastActiveSceneView.pivot;
        var cameraPos = SceneView.lastActiveSceneView.camera.transform.position;
        var distance = Vector3.Distance(cameraPos, pivot);
        var worldPoint = ray.GetPoint(distance);
        return worldPoint;
    }
    private static List<Template> GetTemplates() {
        var templates = new List<Template>();
        foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies()) {
            foreach (var assemblyDefinedType in assembly.DefinedTypes) {
                if (assemblyDefinedType.IsSubclassOf(typeof(LapsComponent))) {
                    var hidden = false;
                    var menuPriority = 0;
                    var buttonLabel = assemblyDefinedType.Name;
                    var attributes =
                        assemblyDefinedType.GetCustomAttributes(typeof(LapsAddMenuOptionsAttribute), false);
                    for (int i = 0; i < attributes.Length; i++) {
                        var attribute = (LapsAddMenuOptionsAttribute) attributes[i];
                        hidden = attribute.hidden;
                        menuPriority = attribute.menuPriority;
                        buttonLabel = attribute.buttonLabel;
                    }
                    if (assemblyDefinedType.IsAbstract) {
                        hidden = true;
                    }
                    if (assemblyDefinedType.IsGenericType) {
                        hidden = true;
                    }
                    if (!hidden) {
                        templates.Add(new Template() {
                            menuText = buttonLabel,
                            menuPriority = menuPriority,
                            type = assemblyDefinedType,
                        });
                    }
                }
            }
        }
        templates.Sort((template, buttonData) => {
            if (template.menuPriority != buttonData.menuPriority) {
                return buttonData.menuPriority - template.menuPriority;
            }
            else {
                return string.CompareOrdinal(template.menuText, buttonData.menuText);
            }
        });
        return templates;
    }
}