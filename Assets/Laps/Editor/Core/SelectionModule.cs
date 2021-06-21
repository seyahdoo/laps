using System;
using System.Collections.Generic;
using LapsEditor.Utility;
using LapsRuntime;
using UnityEditor;
using UnityEngine;

namespace LapsEditor {
    public class SelectionModule {
        private static readonly string LapsIconsFolderName = "LapsIcons";
        internal static readonly Vector2 SelectionIconSize = new Vector2(30f, 30f);
        private LapsEditor _editor;
        private Dictionary<Type, Texture> _loadedIconTextures = new Dictionary<Type, Texture>();
        private Texture _compoundInsideTexture;
        public SelectionModule(LapsEditor lapsEditor) {
            _editor = lapsEditor;
        }
        public void OnSceneGUI() {
            using (Scopes.HandlesGUI()) {
                DrawLapsIcons();
            }
        }
        private void DrawLapsIcons() {
            foreach (var lapsComponent in _editor.allComponents) {
                if (EditorCoreCommons.ShoudDrawNormal(lapsComponent)) {
                    DrawLapsIcon(lapsComponent);
                }
                if (lapsComponent is CompoundComponent compound) {
                    if (EditorCoreCommons.ShoudDrawCompoundInside(compound)) {
                        DrawLapsIcon(compound);
                    }
                }
            }
        }
        private void DrawLapsIcon(LapsComponent lapsComponent) {
            var rect = GetScreenSelectionRect(lapsComponent);
            var iconTexture = GetIconTexture(lapsComponent);
            IconHandle.Draw(lapsComponent, iconTexture, rect);
        }
        private Rect GetScreenSelectionRect(LapsComponent lapsComponent) {
            var rect = new Rect(GetSelectionRectPosition(lapsComponent) - SelectionIconSize / 2f, SelectionIconSize);
            return rect;
        }
        private Vector2 GetSelectionRectPosition(LapsComponent lapsComponent) {
            var screenPosition = GetGUIPosition(lapsComponent);
            if (lapsComponent.ErrorExists) {
                if (screenPosition.x < SelectionIconSize.x / 2f) {
                    screenPosition.x = SelectionIconSize.x / 2f;
                }
                if (screenPosition.x > Screen.width - SelectionIconSize.x / 2f) {
                    screenPosition.x = Screen.width - SelectionIconSize.x / 2f;
                }
                if (screenPosition.y < SelectionIconSize.y / 2f) {
                    screenPosition.y = SelectionIconSize.y / 2f;
                }
                if (screenPosition.y > Screen.height - 40 - SelectionIconSize.y / 2f) {
                    screenPosition.y = Screen.height - 40 - SelectionIconSize.y / 2f;
                }
            }
            return screenPosition;
        }
        private Vector2 GetGUIPosition(LapsComponent lapsComponent) {
            return WorldToGUIPosition(lapsComponent.transform.position);
        }
        private Vector2 WorldToGUIPosition(Vector3 worldPoint) {
            return HandleUtility.WorldToGUIPoint(worldPoint);
        }
        private Texture GetIconTexture(LapsComponent lapsComponent) {
            if (lapsComponent is CompoundComponent compound) {
                if (EditorCoreCommons.ShoudDrawCompoundInside(compound)) {
                    if (_compoundInsideTexture != null) {
                        return _compoundInsideTexture;
                    }
                    _compoundInsideTexture = LoadTexture("compoundinside");
                    return _compoundInsideTexture;
                }
            }
            var type = lapsComponent.GetType();
            if (_loadedIconTextures.TryGetValue(type, out var texture)) {
                return texture;
            }
            texture = LoadTexture(type);
            _loadedIconTextures.Add(type, texture);
            return texture;
        }
        private Texture LoadTexture(Type type) {
            var texture = LoadTexture(type.Name.ToLower());
            if (texture == null) {
                texture = LoadTexture(typeof(LapsComponent));
            }
            return texture;
        }
        private Texture LoadTexture(string name) {
            var folderGuids = AssetDatabase.FindAssets($"t:folder {LapsIconsFolderName}");
            foreach (var folderGuid in folderGuids) {
                var folderPath = AssetDatabase.GUIDToAssetPath(folderGuid);
                var foundAssetGuids = AssetDatabase.FindAssets($"t:Texture {name}", new[] {folderPath});
                foreach (var guid in foundAssetGuids) {
                    var texture = AssetDatabase.LoadAssetAtPath<Texture>(AssetDatabase.GUIDToAssetPath(guid));
                    if (texture != null) {
                        return texture;
                    }
                }
            }
            return null;
        }
    }
}