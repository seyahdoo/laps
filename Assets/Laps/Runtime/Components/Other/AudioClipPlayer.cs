using System;
using System.Collections.Generic;
using UnityEngine;

namespace LapsRuntime {
    [LapsAddMenuOptions("Other/Audio Clip Player")]
    [RequireComponent(typeof(AudioSource))]
    public class AudioClipPlayer : LapsComponent {
        public List<AudioClip> startClips = new List<AudioClip>();
        public List<AudioClip> loopClips = new List<AudioClip>();
        public List<AudioClip> endClips = new List<AudioClip>();
        public bool playOnAwake = false;
        
        private AudioSource _source;
        private AudioClip _currentClip;
        private State _state = State.Stopped;
        private bool _isPaused = false;
        private enum State {
            Stopped,
            Start,
            Loop,
            StoppingReadyToPlayEnd,
            StoppingPlayingEnd,
        }
        public AudioClip CurrentPlayingClip => _currentClip;
        public void Awake() {
            _source = GetComponent<AudioSource>();
            _state = State.Stopped;
            _isPaused = false;
            if (playOnAwake) {
                Play();
            }
        }
        public void Play() {
            if (_isPaused) {
                _isPaused = false;
                _source.UnPause();
                return;
            }
            _state = State.Start;
            _source.Stop();
            _currentClip = PickNewClip();
            if (_currentClip == null) {
                _state = State.Stopped;
                return;
            }
            _source.PlayOneShot(_currentClip);
        }
        public void Pause() {
            _isPaused = true;
            _source.Pause();
        }
        public void Stop() {
            _state = State.StoppingReadyToPlayEnd;
        }
        public void StopImmidiately() {
            if (_state == State.Stopped) return;
            _currentClip = null;
            _state = State.Stopped;
            _source.Stop();
            OnStopped();
        }
        private void Update() {
            if (_state != State.Stopped && !_source.isPlaying) {
                if (_state == State.Start) _state = State.Loop;
                if (_state == State.StoppingPlayingEnd) _state = State.Stopped;
                if (_state == State.StoppingReadyToPlayEnd) _state = State.StoppingPlayingEnd;
                _currentClip = PickNewClip();
                if (_currentClip == null) {
                    _state = State.Stopped; 
                    OnStopped();
                    return;
                }
                _source.PlayOneShot(_currentClip);
            }
        }
        public AudioClip PickNewClip() {
            if (_state == State.Start && startClips.Count <= 0) _state = State.Loop;
            if (_state == State.Loop && loopClips.Count <= 0) _state = State.StoppingPlayingEnd;
            if (_state == State.StoppingPlayingEnd && endClips.Count <= 0) _state = State.Stopped;
            switch (_state) {
                case State.Start:
                    return LapsMath.PickRandomFromList(startClips);
                case State.Loop:
                    return LapsMath.PickRandomFromList(loopClips);
                case State.StoppingPlayingEnd:
                    return LapsMath.PickRandomFromList(endClips);
                default: return null;
            }
        }
        private void OnStopped() {
            FireOutput(0);
        }
        public override object HandleInput(int slotId, object parameter, LapsComponent eventSource) {
            switch (slotId) {
                case 0: Play();            return null;
                case 1: Pause();           return null;
                case 2: Stop();            return null;
                case 3: StopImmidiately(); return null;
                default:                   return null;
            }
        }
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
