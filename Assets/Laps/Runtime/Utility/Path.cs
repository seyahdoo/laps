using System;
using System.Collections.Generic;
using UnityEngine;

namespace LapsRuntime {
    [Serializable]
    public class Path {
        public List<Vector3> points = new List<Vector3>(){Vector3.zero, Vector3.right};
        public bool closed = false;

        public bool IsValid => points.Count >= 2;
        public PathEnumerator GetEnumerator() {
            return new PathEnumerator(this);
        }
        public struct PathEnumerator {
            private Path _path;
            private int _previousIndex;
            private int _currentIndex;
            private int _nextIndex;
            
            public PathEnumerator(Path path) {
                _path = path;
                _previousIndex = 0;
                _currentIndex = 0;
                _nextIndex = 1;
            }
            public int PreviousPointIndex => _previousIndex;
            public int CurrentPointIndex => _currentIndex;
            public int NextPointIndex => _nextIndex;
            public bool GoToNextPoint() {
                var nextIndex = GetNextIndex(_currentIndex);
                if (nextIndex == _currentIndex) {
                    return false;
                }
                _previousIndex = _currentIndex;
                _currentIndex = nextIndex;
                _nextIndex = GetNextIndex(_currentIndex);
                return true;
            }
            public bool GoToPreviousPoint() {
                throw new NotImplementedException();
            }
            public bool GoForward(float distance) {
                throw new NotImplementedException();
            }
            public bool GoBackward(float distance) {
                throw new NotImplementedException();
            }
            private int GetNextIndex(int current) {
                if (current <= _path.points.Count - 2) {
                    return current + 1;
                }
                else {
                    return current;
                }
            }
        }
    }
}
