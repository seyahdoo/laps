using UnityEngine;

namespace LapsRuntime {
    [AddComponentMenu("Laps/Physics/Movement 2D")]
    [LapsAddMenuOptions("Physics/Movement 2D")]
    public class Movement2D : LapsComponent {
        public Path path = new Path();
        public Rigidbody2D body;
        public float speed = 1f;
        
        private float _direction = 0;
        private float _speed = 0f;
        private Path.PathEnumerator _pathEnumerator;
        public float Direction => _direction;
        public float Speed => _speed;
        public void Awake() {
            _speed = speed;
            _pathEnumerator = path.GetEnumerator();
        }
        private void FixedUpdate() {
            var effectiveSpeed = _speed * _direction;
            var distance = effectiveSpeed * Time.fixedDeltaTime;
            if (distance > 0) {
                _pathEnumerator.GoForward(distance);
            }
            if (distance < 0) {
                _pathEnumerator.GoBackward(-distance);
            }
            SetPosition(_pathEnumerator.CurrentPosition);
        }
        public void GoForwards() {
            _direction = 1f;
        }
        public void GoBackwards() {
            _direction = -1f;
        }
        public void SetSpeed(float speed) {
            _speed = speed;
        }
        public void Stop() {
            _direction = 0f;
        }
        public void TeleportForwardEnd() {
            _pathEnumerator.GoToEndPoint();
            SetPosition(_pathEnumerator.CurrentPosition);
        }
        public void TeleportBackwardEnd() {
            _pathEnumerator.Reset();
            SetPosition(_pathEnumerator.CurrentPosition);
        }
        private void SetPosition(Vector2 position) {
            body.position = position;
            body.velocity = Vector2.zero;
        }
        public override void GetInputSlots(SlotList slots) {
            slots.Add("set body", 0, typeof(Rigidbody2D));
            slots.Add("go forwards", 1);
            slots.Add("go backwards", 2);
            slots.Add("set speed", 3, typeof(float));
            slots.Add("stop", 4);
            slots.Add("teleport forward end", 5);
            slots.Add("teleport backward end", 6);
        }
        public override void GetOutputSlots(SlotList slots) {
            slots.Add("body", 0, null, typeof(Rigidbody2D));
            slots.Add("forwards finished", 1);
            slots.Add("backwards finished", 2);
        }
    }
}
