using System;
using System.Collections.Generic;
using UnityEngine;

namespace LapsRuntime {
    public class Collider2DListenerComponent : LapsComponent {
        public ActivationMode activationMode;
        public FilterMode filterMode;
        public LayerMask layerMask = -1;
        public bool enabledOnAwake = true;
        private bool _enterEnabled;
        private bool _exitEnabled;
        private int _insideCount;
        private void Awake() {
            _insideCount = 0;
            _enterEnabled = enabledOnAwake;
            _exitEnabled = enabledOnAwake;
            enabled = enabledOnAwake;
        }
        private void OnTriggerEnter2D(Collider2D other)    { HandleEnter(other); }
        private void OnTriggerExit2D(Collider2D other)     { HandleExit(other); }
        private void OnCollisionEnter2D(Collision2D other) { HandleEnter(other.collider); }
        private void OnCollisionExit2D(Collision2D other)  { HandleExit(other.collider); }
        private void HandleEnter(Collider2D otherCollider) {
            if (!_enterEnabled) return;
            if (!FilterAccepts(otherCollider)) return;
            HandleEnterActivation(otherCollider);
        }
        private void HandleExit(Collider2D otherCollider) {
            if (!_exitEnabled) return;
            if (!FilterAccepts(otherCollider)) return;
            HandleExitActivation(otherCollider);
        }
        private bool FilterAccepts(Collider2D otherCollider) {
            if (!LapsMath.LayerMaskContains(layerMask, otherCollider.gameObject.layer)) return false;
            switch (filterMode) {
                case FilterMode.AcceptCollider: if (otherCollider.isTrigger) return false; break;
                case FilterMode.AcceptTrigger: if (!otherCollider.isTrigger) return false; break;
            }
            return true;
        }
        private void HandleEnterActivation(Collider2D otherCollider) {
            _insideCount++;
            switch (activationMode) {
                case ActivationMode.ActivateAlways:
                    FireObjectEntered(otherCollider);
                    break;
                case ActivationMode.ActivateOnce:
                    _enterEnabled = false;
                    FireObjectEntered(otherCollider);
                    break;
                case ActivationMode.ActivateCumulatively:
                    if (_insideCount == 1) {
                        FireObjectEntered(otherCollider);
                    }
                    break;
            }
        }
        private void HandleExitActivation(Collider2D otherCollider) {
            _insideCount--;
            switch (activationMode) {
                case ActivationMode.ActivateAlways:
                    FireObjectExited(otherCollider);
                    break;
                case ActivationMode.ActivateOnce:
                    _exitEnabled = false;
                    FireObjectExited(otherCollider);
                    break;
                case ActivationMode.ActivateCumulatively:
                    if (_insideCount == 0) {
                        FireObjectExited(otherCollider);
                    }
                    break;
            }
        }
        private void FireObjectEntered(Collider2D otherCollider) {
            FireOutput(0, otherCollider.attachedRigidbody);
        }
        private void FireObjectExited(Collider2D otherCollider) {
            FireOutput(1, otherCollider.attachedRigidbody);
        }
        public override void GetInputSlots(List<LogicSlot> slots) {
            slots.Add(new LogicSlot("enable", 0));
            slots.Add(new LogicSlot("disable", 1));
        }
        public override void GetOutputSlots(List<LogicSlot> slots) {
            slots.Add(new LogicSlot("object entered", 0, typeof(Rigidbody2D)));
            slots.Add(new LogicSlot("object exited", 1, typeof(Rigidbody2D)));
        }
        public enum FilterMode {
            AcceptAny = 0,
            AcceptCollider = 1,
            AcceptTrigger = 2,
        }
        public enum ActivationMode {
            ActivateAlways = 0,
            ActivateOnce = 1,
            ActivateCumulatively = 2,
        }
    }
}
