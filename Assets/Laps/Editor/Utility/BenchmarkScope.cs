using System;
using System.Diagnostics;
using System.Globalization;
using Debug = UnityEngine.Debug;

namespace LapsEditor {
    public static class BenchmarkScope {
        public static GUIBackgroundColorEnd Benchmark(string name) {
            var sw = new Stopwatch();
            sw.Start();
            return new GUIBackgroundColorEnd(sw, name);
        }
        public struct GUIBackgroundColorEnd : IDisposable {
            private readonly string _name;
            private readonly Stopwatch _stopwatch;
            public GUIBackgroundColorEnd(Stopwatch stopwatch, string name) {
                _stopwatch = stopwatch;
                _name = name;
            }
            public void Dispose() {
                _stopwatch.Stop();
                var time= _stopwatch.Elapsed.Ticks;
                Debug.Log($"{time.ToString("N1", CultureInfo.InvariantCulture)} {_name}");
            }
        }
    }
}