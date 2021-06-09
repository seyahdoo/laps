using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace LapsEditor.Utility {
    public class ShortcutManager {
        private List<Shortcut> shortcuts = new List<Shortcut>();
        private Shortcut _activeShortcut;
        private Event e => Event.current;
        public void AddShortcut(Shortcut t) {
            shortcuts.Add(t);
        }
        public void HandleInput() {
            HandleUtility.AddDefaultControl(GUIUtility.GetControlID(FocusType.Passive));
            if (e.type == EventType.Repaint) {
                HandleEventUpdate();
                return;
            }
            HandleEventPress();
            HandleEventDrag();
            HandleEventRelease();
        }
        private void HandleEventPress() {
            if (_activeShortcut != null) return;
            if (e.type != EventType.KeyDown && e.type != EventType.MouseDown) return;
            for (int i = 0; i < shortcuts.Count; i++) { var shortcut = shortcuts[i];
                if (!EventSatisfiesActivationRulePress(e, shortcut.activation)) continue;
                if (shortcut.onPress == null || shortcut.onPress()) {
                    _activeShortcut = shortcut;
                    e.Use();
                    return;
                }
            }
        }
        private bool EventSatisfiesActivationRulePress(Event @event, ActivationRule rule) {
            if (GUIUtility.hotControl != 0) return false;
            if (rule.modifiers != (@event.modifiers & ~EventModifiers.CapsLock & ~EventModifiers.FunctionKey)) return false;
            if (rule.mouseButton >= 0 && @event.isMouse && @event.button == rule.mouseButton) return true;  
            if (rule.key != KeyCode.None && @event.isKey && @event.keyCode == rule.key) return true;
            return false;
        }
        private void HandleEventDrag() {
            if (_activeShortcut == null) return;
            if (e.type != EventType.MouseDrag) return;
            e.Use();
            _activeShortcut.onDrag?.Invoke();
        }
        private void HandleEventUpdate() {
            if (_activeShortcut == null) return;
            if (_activeShortcut.onUpdate == null) return;
            _activeShortcut.onUpdate();
            EditorWindow.focusedWindow.Repaint();
        }
        private void HandleEventRelease() {
            if (_activeShortcut == null) return;
            if (!EventSatisfiesActivationRuleRelease(e, _activeShortcut.activation)) return;
            e.Use();
            _activeShortcut.onRelease?.Invoke();
            _activeShortcut = null;
        }
        private bool EventSatisfiesActivationRuleRelease(Event @event, ActivationRule rule) {
            // Mouse leaving the window triggers release because else after mouse has left the current window;
            // up condition does not exist therefore is never true; release does not get called
            if (rule.mouseButton >= 0 
                && (@event.type == EventType.MouseUp || @event.type == EventType.MouseLeaveWindow) 
                && rule.mouseButton == @event.button) {
                return true;
            }
            if (rule.key != KeyCode.None 
                && @event.type == EventType.KeyUp 
                && rule.key == @event.keyCode) {
                return true;
            }
            return false;
        }
        public class Shortcut {
            public string name;
            public ActivationRule activation;
            public Func<bool> onPress;
            public Action onDrag;
            public Action onRelease;
            public Action onUpdate;
            public Shortcut(string name) {
                this.name = name;
            }
        }
        public class ActivationRule {
            public EventModifiers modifiers = EventModifiers.None;
            public int mouseButton = -1;
            public KeyCode key = KeyCode.None;
        }
    }
}