using System.Collections.Generic;
using UnityEngine;

namespace LapsRuntime {
    [RequireComponent(typeof(AudioSource))]
    public class AudioClipPlayer : LapsComponent {
        public List<AudioClip> startClips = new List<AudioClip>();
        public List<AudioClip> loopClips = new List<AudioClip>();
        public List<AudioClip> endClips = new List<AudioClip>();
        public bool playOnAwake = false;
        public override void GetOutputSlots(SlotList slots) {
            slots.Add("on end", 0);
        }
        public override void GetInputSlots(SlotList slots) {
            slots.Add("play", 0);
            slots.Add("pause", 1);
            slots.Add("stop", 2);
            slots.Add("stop immidiately", 3);
        }
    }
}
