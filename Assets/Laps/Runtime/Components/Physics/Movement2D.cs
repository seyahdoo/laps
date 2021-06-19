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
        private Rigidbody2D _body;
        private Path.PathEnumerator _pathEnumerator;
        public Rigidbody2D Body => _body;
        public float Direction => _direction;
        public float Speed => _speed;
        public void Awake() {
            _pathEnumerator = path.GetEnumerator();
            _speed = speed;
            _body = body;
            if (FireOutput(0) is Rigidbody2D outputBody) {
                _body = outputBody;
            }
        }
        private void FixedUpdate() {
            var effectiveSpeed = _speed * _direction;
            var distance = effectiveSpeed * Time.fixedDeltaTime;
            if (distance > 0) {
                if (!_pathEnumerator.GoForward(distance)) {
                    Stop();
                    FireOutput(1);
                }
            }
            if (distance < 0) {
                if (!_pathEnumerator.GoBackward(-distance)) {
                    Stop();
                    FireOutput(2);
                }
            }
            SetPosition(_pathEnumerator.CurrentPosition);
        }
        public void GoForwards() {
            _direction = 1f;
        }
        public void GoBackwards() {
            _direction = -1f;
        }
        public void SetSpeed(float speedToSet) {
            _speed = speedToSet;
        }
        public void Stop() {
            _direction = 0f;
        }
        public void TeleportForwardEnd() {
            _pathEnumerator.GoToEndPoint();
            SetPosition(_pathEnumerator.CurrentPosition);
            FireOutput(1);
        }
        public void TeleportBackwardEnd() {
            _pathEnumerator.Reset();
            SetPosition(_pathEnumerator.CurrentPosition);
            FireOutput(2);
        }
        private void SetPosition(Vector2 position) {
            _body.position = position;
            _body.velocity = Vector2.zero;
        }
        public override object HandleInput(int slotId, object parameter, LapsComponent eventSource) {
            switch (slotId) {
                case 0:  _body = (Rigidbody2D) parameter; return null;
                case 1:  GoForwards(); return null;
                case 2:  GoBackwards(); return null;
                case 3:  SetSpeed((float)parameter); return null;
                case 4:  Stop(); return null;
                case 5:  TeleportForwardEnd(); return null;
                case 6:  TeleportBackwardEnd(); return null;
                default: return null;
            }
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
