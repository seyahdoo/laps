using System.Collections.Generic;
using UnityEngine;

namespace LapsRuntime {
    public class LapsMath {
        public const float Epsilon = 0.0001f;
        public static float DistanceFromPointToRect(Rect rect, Vector2 p) {
            //source MultiRRomero at https://stackoverflow.com/questions/5254838/calculating-distance-between-a-point-and-a-rectangular-box-nearest-point
            var dx = Mathf.Max(rect.min.x - p.x, 0, p.x - rect.max.x);
            var dy = Mathf.Max(rect.min.y - p.y, 0, p.y - rect.max.y);
            var distance = Mathf.Sqrt(dx * dx + dy * dy);
            return distance;
        }
        public static bool ApproximatelyClose(float f1, float f2) {
            return Mathf.Abs(f1 - f2) < Epsilon;
        }
        public static bool ApproximatelyClose(Vector2 v1, Vector2 v2) {
            return Vector2.Distance(v1, v2) < Epsilon;
        }
        public static bool ApproximatelyClose(Vector3 v1, Vector3 v2) {
            return Vector3.Distance(v1, v2) < Epsilon;
        }
        public static bool LayerMaskContains(LayerMask layerMask, int layer) {
            return layerMask == (layerMask | (1 << layer));
        }
        public static T PickRandomFromArray<T>(T[] array) {
            if (array.Length <= 0) return default;
            return array[Random.Range(0, array.Length)];
        }
        public static T PickRandomFromList<T>(List<T> list) {
            if (list.Count <= 0) return default;
            return list[Random.Range(0, list.Count)];
        }
    }
}
