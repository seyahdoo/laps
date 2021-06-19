using UnityEngine;

namespace LapsRuntime {
    [AddComponentMenu("Laps/Physics/Collider 2D Listener")]
    [LapsAddMenuOptions("Physics/Collider 2D Listener")]
    public class Collider2DListenerComponent : LapsComponent {
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
            switch (typeFilterMode) {
                case TypeFilterMode.AcceptCollider: if (otherCollider.isTrigger) return false; break;
                case TypeFilterMode.AcceptTrigger: if (!otherCollider.isTrigger) return false; break;
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
        public override void GetInputSlots(SlotList slots) {
            slots.Add("enable", 0);
            slots.Add("disable", 1);
        }
        public override void GetOutputSlots(SlotList slots) {
            slots.Add("object entered", 0, typeof(Rigidbody2D));
            slots.Add("object exited", 1, typeof(Rigidbody2D));
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
