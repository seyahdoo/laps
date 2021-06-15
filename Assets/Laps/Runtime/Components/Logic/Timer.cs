using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace LapsRuntime {
    public class Timer : LapsComponent {
        public List<float> times = new List<float>() {.5f};
        public bool startOnAwake = false;
        public void Awake() {
            enabled = startOnAwake;
        }
        private void Update() {
            for (int i = times.Count - 1; i >= 0; i--) {
                times[i] -= Time.deltaTime;
                if (times[i] <= 0f) {
                    times.RemoveAt(i);
                    FireOutput(0);
                }
            }
        }
        public override object HandleInput(int slotId, object parameter, LapsComponent eventSource) {
            switch (slotId) {
                case 0:  enabled = true; return null;
                case 1:  enabled = false; return null;
                case 2:  SetTimer((float) parameter); return null;
                default: return null;
            }
        }
        private void SetTimer(float time) {
            times.Clear();
            times.Add(time);
            enabled = true;
        }
        public override void GetInputSlots(SlotList slots) {
            slots.Add("start", 0);
            slots.Add("stop", 1);
            slots.Add("set time", 2, typeof(float));
        }
        public override void GetOutputSlots(SlotList slots) {
            slots.Add("trigger", 0);
        }
    }
}
