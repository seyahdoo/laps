using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngineInternal;

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
            private float _progress;
            
            public PathEnumerator(Path path) {
                _path = path;
                _previousIndex = 0;
                _currentIndex = 0;
                _nextIndex = 1;
                _progress = 0f;
            }
            public int PreviousPointIndex => _previousIndex;
            public int CurrentPointIndex => _currentIndex;
            public int NextPointIndex => _nextIndex;
            public Vector3 CurrentPosition => Vector3.LerpUnclamped(_path.points[_currentIndex], _path.points[_nextIndex], _progress);
            public bool GoToNextPoint() {
                var nextIndex = GetNextIndex(_currentIndex);
                if (nextIndex == _currentIndex) {
                    return false;
                }
                _previousIndex = _currentIndex;
                _currentIndex = nextIndex;
                _nextIndex = GetNextIndex(_currentIndex);
                _progress = 0f;
                return true;
            }
            public bool GoToPreviousPoint() {
                var prevIndex = GetPrevIndex(_currentIndex);
                if (prevIndex == _currentIndex) {
                    if(_progress > 0) {
                        _progress = 0f;
                        return true;
                    }
                    return false;
                }
                _nextIndex = _currentIndex;
                _currentIndex = prevIndex;
                _previousIndex = GetPrevIndex(_currentIndex);
                _progress = 1f;
                return true;
            }
            public bool GoForward(float distance) {
                int killSwitch = _path.points.Count + 1;
                while (killSwitch-- > 0 && distance > 0) {
                    var distanceOfCurrentAndNextPoint = Vector3.Distance(_path.points[_currentIndex], _path.points[_nextIndex]);
                    var leftDistance = (1f - _progress) * distanceOfCurrentAndNextPoint;
                    if (distance > leftDistance) {
                        _progress = 0;
                        distance -= leftDistance;
                        if (!GoToNextPoint()) {
                            return false;
                        }
                    }
                    else {
                        var p = distance / distanceOfCurrentAndNextPoint;
                        _progress += p;
                        return true;
                    }
                }
                return true;
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
            private int GetPrevIndex(int current) {
                if (current >= 1) {
                    return current - 1;
                }
                else {
                    return current;
                }
            }
        }
    }
}
