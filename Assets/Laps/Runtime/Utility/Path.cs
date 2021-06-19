using System;
using System.Collections.Generic;
using UnityEngine;

namespace LapsRuntime {
    [Serializable]
    public class Path {
        public List<Vector3> points = new List<Vector3>();
        public bool closed = false;

        public bool IsValid => points.Count >= 2;
        public PathEnumerator GetEnumerator() {
            return new PathEnumerator(this);
        }
        public struct PathEnumerator {
            private Path _path;
            private int _index;
            private float _progress;
            public PathEnumerator(Path path) {
                _path = path;
                _index = 0;
                _progress = 0;
            }

            public void GoForward(float distance) {
                
            }
            public void GoBackward(float distance) {
                
            }
            public void GoToNextPoint() {
                
            }
            public void GoToPreviousPoint() {
                
            }
        }
    }
}
