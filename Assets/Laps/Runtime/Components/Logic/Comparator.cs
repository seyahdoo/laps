using System;
using UnityEngine;

namespace LapsRuntime {
    [AddComponentMenu("Laps/Logic/Comparator")]
    [LapsAddMenuOptions("Logic/Comparator")]
    public class Comparator : LapsComponent {
        public enum ComparisonMode {
            LessThan = 0,
            LessEqual = 1,
            GraterThan = 2,
            GraterEqual = 3,
            Equals = 4,
            NotEquals = 5,
        }
        public float a;
        public ComparisonMode comparisonMode;
        public float b;
        public bool triggerOnAwake = false;
        public bool triggerOnValueChange = true;
        public bool triggerOnResultChange = true;
        private bool _lastTriggerResult = false;
        public void Awake() {
            if (triggerOnAwake) {
                Trigger();
            }
        }
        public override object HandleInput(int slotId, object parameter, LapsComponent eventSource) {
            switch (slotId) {
                case 0: a = (float) parameter; OnValueChange(); return null;
                case 1: b = (float) parameter; OnValueChange(); return null;
                case 2: Trigger(); return null;
            }
            return null;
        }
        public void OnValueChange() {
            if (triggerOnValueChange) {
                Trigger();
                return;
            }
            if (triggerOnResultChange) {
                var currentResult = Evaluate();
                if (currentResult != _lastTriggerResult) {
                    Trigger();
                }
            }
        }
        public void Trigger() {
            _lastTriggerResult = Evaluate();
            if (_lastTriggerResult) {
                FireOutput(0);
            }
            else {
                FireOutput(1);
            }
        }
        private bool Evaluate() {
            switch (comparisonMode) {
                case ComparisonMode.Equals:      return Math.Abs(a - b) < 0.01f;
                case ComparisonMode.NotEquals:   return Math.Abs(a - b) > 0.01f;
                case ComparisonMode.GraterEqual: return a >= b;
                case ComparisonMode.GraterThan:  return a > b;
                case ComparisonMode.LessEqual:   return a <= b;
                case ComparisonMode.LessThan:    return a < b;
                default:                         return false;
            }
        }
        public override void GetInputSlots(SlotList slots) {
            slots.Add("set a", 0);
            slots.Add("set b", 1);
            slots.Add("trigger", 2);
        }
        public override void GetOutputSlots(SlotList slots) {
            slots.Add("true", 0);
            slots.Add("false", 1);
        }
    }
}
