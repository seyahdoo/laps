using LapsEditor.Utility;
using LapsRuntime;
using UnityEditor;
using UnityEngine;

namespace LapsEditor {
    public class LapsEditorSelectionModule {
        private static readonly Vector2 SelectionIconSize = new Vector2(30f, 30f);
        // private static readonly float DragThreshold = 20f;
        private static readonly Color SelectionIconColor = Color.white;
        public LapsComponent[] lapsComponents;
        private ShortcutManager _shortcuts;
        public LapsEditorSelectionModule() {
            EditorApplication.hierarchyChanged += EditorApplicationOnHierarchyChanged;
            EditorApplicationOnHierarchyChanged();
            SetupShortcuts();
        }
        public void OnSceneGUI() {
            DrawLapsIcons();
            _shortcuts.HandleInput();
        }
        private void SetupShortcuts() {
            _shortcuts = new ShortcutManager();
            _shortcuts.AddShortcut(new ShortcutManager.Shortcut("select with l-click") {
                activation = new ShortcutManager.ActivationRule() {
                    mouseButton = 0,
                },
                onPress = () => SelectionPress(Event.current.mousePosition, false),
                onRelease = () => SelectionRelease(Event.current.mousePosition),
            });
            _shortcuts.AddShortcut(new ShortcutManager.Shortcut("additive select with shift l-click") {
                activation = new ShortcutManager.ActivationRule() {
                    mouseButton = 0,
                    modifiers = EventModifiers.Shift,
                },
                onPress = () => SelectionPress(Event.current.mousePosition, true),
                onRelease = () => SelectionRelease(Event.current.mousePosition),
            });
        }
        public bool SelectionPress(Vector2 position, bool additive) {
            return false;
        }
        public void SelectionRelease(Vector2 position) {
        }
        private void EditorApplicationOnHierarchyChanged() {
            //todo optimize this
            lapsComponents = Object.FindObjectsOfType<LapsComponent>();
        }
        private void DrawLapsIcons() {
            using (Scopes.HandlesGUI()) {
                foreach (var lapsComponent in lapsComponents) {
                    DrawLapsIcon(lapsComponent);
                }
            }
        }
        private void DrawLapsIcon(LapsComponent lapsComponent) {
            var rect = GetScreenSelectionRect(lapsComponent);
            var iconTexture = GetIconTexture(lapsComponent);
            GUI.DrawTexture(
                rect,
                iconTexture,
                ScaleMode.StretchToFill,
                true,
                0.0f,
                SelectionIconColor,
                0.0f,
                0.0f);
        }
        private Rect GetScreenSelectionRect(LapsComponent lapsComponent) {
            return default;
        }
        private Texture GetIconTexture(LapsComponent lapsComponent) {
            return default;
        }
    }
}