using System.Collections.Generic;
using LapsEditor.Utility;
using LapsRuntime;
using UnityEditor;
using UnityEngine;

namespace LapsEditor { 
    public class PathEditor {
        private static readonly int HandleHintHash = nameof(PathEditor).GetHashCode();
        private static readonly int LineThickness = 5;
        private static readonly Color LineColor = Color.white;
        private static readonly Color SelectedPointColor = Color.blue;
        private static readonly Color PointColor = Color.white;
        private static readonly float PointPressDistance = 20f;
        private static readonly float PointRadius = .4f;
        
        private Path _path;
        private int _lastControlID;
        private int _pressedPoint;

        private HashSet<int> _selection = new HashSet<int>();
        private bool _dragged = false;
        private Vector2 _startMousePosition;
        public PathEditor(Path path) {
            _path = path;
        }
        public void OnSceneGUI(bool editEnabled) {
            if (editEnabled) {
                CustomHandle();
            }
            else {
                DrawTheLine();
            }
        }
        private void DrawTheLine() {
            for (var i = 0; i < _path.points.Count - 1; i++) {
                var p1 = _path.points[i];
                var p2 = _path.points[i + 1];
                using (Scopes.HandlesColor(LineColor)) {
                    Handles.DrawLine(p1, p2, LineThickness);
                }
            }
        }
        private void FreeMove() {
            for (var i = 0; i < _path.points.Count; i++) {
                var position = _path.points[i];
                var size = HandleUtility.GetHandleSize(position);
                size *= .2f;
                position = Handles.FreeMoveHandle(position, Quaternion.identity, size, Vector3.one, Handles.SphereHandleCap);
                _path.points[i] = position;
            }
        }
        private void CustomHandle() {
            int id = GUIUtility.GetControlID(HandleHintHash, FocusType.Passive);
            _lastControlID = id;
            switch (Event.current.GetTypeForControl(id)) {
                case EventType.MouseDown:
                    if (Event.current.button == 0 && HandleUtility.nearestControl == id) {
                        GUIUtility.hotControl = id;
                        OnMouseDown();
                        Event.current.Use();
                        EditorGUIUtility.SetWantsMouseJumping(1);
                    }
                    break;
                case EventType.MouseUp:
                    if (id == GUIUtility.hotControl) {
                        GUIUtility.hotControl = 0;
                        OnMouseUp();
                        Event.current.Use();
                        EditorGUIUtility.SetWantsMouseJumping(0);
                    }
                    break;
                case EventType.MouseDrag:
                    if (id == GUIUtility.hotControl) {
                        OnMouseDrag();
                        Event.current.Use();
                    }
                    break;
                case EventType.Repaint:
                    var isHotControl = id == GUIUtility.hotControl;
                    var isNearestControl = HandleUtility.nearestControl == id;
                    OnRepaint(isHotControl, isNearestControl);
                    break;
                case EventType.Layout:
                    var distance = CalculateDistance();
                    HandleUtility.AddControl(id, distance);
                    break;
            }
        }
        private float CalculateDistance() {
            var minDistance = float.PositiveInfinity;
            for (var i = 0; i < _path.points.Count; i++) {
                var position = _path.points[i];
                var guiPoint = HandleUtility.WorldToGUIPoint(position);
                var distance = Vector2.Distance(Event.current.mousePosition, guiPoint);
                if (distance < minDistance) {
                    minDistance = distance;
                }
            }
            if (minDistance < PointPressDistance) {
                return 0;
            }
            return 5f;
        }
        private void OnRepaint(bool isHotControl, bool isNearestControl) {
            for (var i = 0; i < _path.points.Count - 1; i++) {
                var p1 = _path.points[i];
                var p2 = _path.points[i + 1];
                using (Scopes.HandlesColor(LineColor)) {
                    Handles.DrawLine(p1, p2, LineThickness);
                }
            }
            for (var i = 0; i < _path.points.Count; i++) {
                var position = _path.points[i];
                var color = _selection.Contains(i) ? SelectedPointColor : PointColor;
                DrawOurSphere(position, color);
            }
            if (Event.current.shift) {
                GetAddRemovePoint(out var shouldRemove, out var shouldAdd, out int index, out Vector3 addPosition);
                if (shouldRemove) {
                    DrawOurSphere(_path.points[index], Color.red);
                }
                if (shouldAdd) {
                    DrawOurSphere(addPosition, Color.green);
                }
            }
        }
        private void DrawOurSphere(Vector3 position, Color color) {
            var size = HandleUtility.GetHandleSize(position);
            size *= PointRadius;
            using (Scopes.HandlesColor(color)) {
                Handles.SphereHandleCap(_lastControlID, position, Quaternion.identity, size, EventType.Repaint);
            }
        }
        private void OnMouseDrag() {
            _dragged = true;
            //if point pressed, drag that
            if (_pressedPoint >= 0) {
                var delta = Event.current.delta;
                // var selectionCenterPosition = GetSelectionLocalCenterPosition();
                var selectionCenterPosition = _path.points[_pressedPoint];
                var cameraTransform = Camera.current.transform;
                var worldToLocalMatrix = Handles.matrix.inverse;
                var cameraLocalNormal = worldToLocalMatrix.MultiplyVector(cameraTransform.forward);
                var pointOnRay = Vector3.Project(selectionCenterPosition, cameraLocalNormal);
                var localPlane = new Plane(cameraLocalNormal, pointOnRay);
                var oldGUIPoint = HandleUtility.WorldToGUIPoint(selectionCenterPosition);
                var newGUIPoint = oldGUIPoint + delta;
                var newPositionWorldRay = HandleUtility.GUIPointToWorldRay(newGUIPoint);
                var newPositionLocalRay = new Ray(
                    worldToLocalMatrix.MultiplyPoint(newPositionWorldRay.origin),
                    worldToLocalMatrix.MultiplyVector(newPositionWorldRay.direction));
                if (localPlane.Raycast(newPositionLocalRay, out float enter)) {
                    var newLocalPosition = newPositionLocalRay.GetPoint(enter);
                    // ShiftSelectionCenter(newLocalPosition - selectionCenterPosition);
                    _path.points[_pressedPoint] += newLocalPosition - selectionCenterPosition;
                    GUI.changed = true;
                }
            }
            //else, do selection rect logic
        }
        private void OnMouseUp() {
            if (TryGetPressedPointIndex(out var upPoint)) {
                if (_pressedPoint == upPoint && !_dragged) {
                    if (Event.current.control || Event.current.shift) {
                        if (_selection.Contains(_pressedPoint)) {
                            _selection.Remove(_pressedPoint);
                        }
                        else {
                            _selection.Add(_pressedPoint);
                        }
                    }
                    else {
                        _selection.Clear();
                        _selection.Add(_pressedPoint);
                    }
                    
                }
                
                
            }
        }
        private void OnMouseDown() {
            _dragged = false;
            _startMousePosition = Event.current.mousePosition;
            TryGetPressedPointIndex(out _pressedPoint);
            if (Event.current.shift) {
                GetAddRemovePoint(out var shouldRemove, out var shouldAdd, out int index, out Vector3 addPosition);
                if (shouldAdd) {
                    _path.points.Insert(index + 1, addPosition);
                    _selection.Clear();
                    _pressedPoint = index + 1;
                }
                if (shouldRemove) {
                    _selection.Clear();
                    _path.points.RemoveAt(index);
                    _pressedPoint = -1;
                }
            }
        }
        private bool TryGetPressedPointIndex(out int index) {
            var minDistance = float.PositiveInfinity;
            index = -1;
            for (var i = 0; i < _path.points.Count; i++) {
                var position = _path.points[i];
                var guiPoint = HandleUtility.WorldToGUIPoint(position);
                var distance = Vector2.Distance(Event.current.mousePosition, guiPoint);
                if (distance < minDistance && distance < PointPressDistance) {
                    minDistance = distance;
                    index = i;
                }
            }
            return index >= 0;
        }
        private Vector3 GetSelectionLocalCenterPosition() {
            if (_path.points.Count < 0) return Vector3.zero;
            Vector3 center = Vector3.zero;
            foreach (var i in _selection) {
                center += _path.points[i];
            }
            return center / _selection.Count;
        }
        private void ShiftSelectionCenter(Vector3 delta) {
            foreach (var i in _selection) {
                _path.points[i] += delta;
            }
        }
        private void GetAddRemovePoint(out bool shouldRemove, out bool shouldAdd, out int index, out Vector3 addPosition) {
            //if close to an existing point, remove that
            if (TryGetPressedPointIndex(out var pressedIndex)) {
                index = pressedIndex;
                addPosition = Vector3.zero;
                shouldRemove = true;
                shouldAdd = false;
                return;
            }
            //if close to line add in between that
            index = -1;
            var closestDistance = float.MaxValue;
            for (var i = 0; i < _path.points.Count - 1; i++) {
                var l1 = _path.points[i];
                var l1gui = HandleUtility.WorldToGUIPoint(l1);
                var l2 = _path.points[i + 1];
                var l2gui = HandleUtility.WorldToGUIPoint(l2);
                var distance = HandleUtility.DistancePointLine(Event.current.mousePosition, l1gui, l2gui);
                if (distance < closestDistance) {
                    closestDistance = distance;
                    index = i;
                }
            }
            shouldAdd = true;
            shouldRemove = false;

            var worldToLocalMatrix = Handles.inverseMatrix;
            var point = _path.points[index];
            var forwardWorld = Camera.current.transform.forward;
            var forwardLocal = worldToLocalMatrix.MultiplyVector(forwardWorld);
            var localPlane = new Plane(forwardLocal, point);
            var worldRay = HandleUtility.GUIPointToWorldRay(Event.current.mousePosition);
            var localRay = new Ray(
                worldToLocalMatrix.MultiplyPoint(worldRay.origin),
                worldToLocalMatrix.MultiplyVector(worldRay.direction));
            if (localPlane.Raycast(localRay, out float enter)) {
                addPosition = localRay.GetPoint(enter);
                return;
            }
            addPosition = (_path.points[index] + _path.points[index + 1]) / 2f;
            return;
        }
    }
}
