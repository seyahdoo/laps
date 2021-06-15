using System;
using System.Collections.Generic;
using UnityEngine;

namespace LapsRuntime {
    public class ColliderListenerComponent : LapsComponent {
        public ActivationMode activationMode;
        public TypeFilterMode typeFilterMode;
        public LayerMask layerMask = -1;
        public bool enabledOnAwake = true;
        private bool _enterEnabled;
        private bool _exitEnabled;
        private int _insideCount;
        public void Awake() {
            _insideCount = 0;
            _enterEnabled = enabledOnAwake;
            _exitEnabled = enabledOnAwake;
            enabled = enabledOnAwake;
        }
        private void OnTriggerEnter(Collider other)    { HandleEnter(other); }
        private void OnTriggerExit(Collider other)     { HandleExit(other); }
        private void OnCollisionEnter(Collision other) { HandleEnter(other.collider); }
        private void OnCollisionExit(Collision other)  { HandleExit(other.collider); }
        private void HandleEnter(Collider otherCollider) {
            if (!_enterEnabled) return;
            if (!FilterAccepts(otherCollider)) return;
            HandleEnterActivation(otherCollider);
        }
        private void HandleExit(Collider otherCollider) {
            if (!_exitEnabled) return;
            if (!FilterAccepts(otherCollider)) return;
            HandleExitActivation(otherCollider);
        }
        private bool FilterAccepts(Collider otherCollider) {
            if (!LapsMath.LayerMaskContains(layerMask, otherCollider.gameObject.layer)) return false;
            switch (typeFilterMode) {
                case TypeFilterMode.AcceptCollider: if (otherCollider.isTrigger) return false; break;
                case TypeFilterMode.AcceptTrigger: if (!otherCollider.isTrigger) return false; break;
            }
            return true;
        }
        private void HandleEnterActivation(Collider otherCollider) {
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
        private void HandleExitActivation(Collider otherCollider) {
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
        private void FireObjectEntered(Collider otherCollider) {
            FireOutput(0, otherCollider.attachedRigidbody);
        }
        private void FireObjectExited(Collider otherCollider) {
            FireOutput(1, otherCollider.attachedRigidbody);
        }
        public override object HandleInput(int slotId, object parameter, LapsComponent eventSource) {
            switch (slotId) {
                case 0: 
                    _enterEnabled = true;
                    _exitEnabled = true;
                    enabled = true;
                    return null;
                case 1: 
                    _enterEnabled = false;
                    _exitEnabled = false;
                    enabled = false;
                    return null;
                default:
                    return null;
            }
        }
        public override void GetInputSlots(List<LogicSlot> slots) {
            slots.Add(new LogicSlot("enable", 0));
            slots.Add(new LogicSlot("disable", 1));
        }
        public override void GetOutputSlots(List<LogicSlot> slots) {
            slots.Add(new LogicSlot("object entered", 0, typeof(Rigidbody)));
            slots.Add(new LogicSlot("object exited", 1, typeof(Rigidbody)));
        }
        public enum TypeFilterMode {
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
