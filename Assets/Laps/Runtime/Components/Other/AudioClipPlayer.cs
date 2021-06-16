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
        private bool _stopClipPlaying;
        private enum State {
            Stopped,
            Starting,
            Start,
            Loop,
            ReadyToStop,
            Stopping,
        }
        public AudioClip CurrentPlayingClip => _currentClip;
        private void Awake() {
            _source = GetComponent<AudioSource>();
            _state = State.Stopped;
            _stopClipPlaying = false;
            if (playOnAwake) {
                Play();
            }
        }
        public void Play() {
            _state = State.Start;
            _source.Stop();
            _currentClip = PickNewClip();
            if (_currentClip == null) _state = State.Stopped;
            _source.PlayOneShot(_currentClip);
        }
        public void Pause() {
            
        }
        public void Stop() {
            _state = State.Stopping;
        }
        public void StopImmidiately() {
            _currentClip = null;
            _state = State.Stopped;
            _source.Stop();
        }
        private void Update() {
            if (_state != State.Stopped && !_source.isPlaying) {
                if (_stopClipPlaying) {
                    _state = State.Stopped;
                    return;
                }
                if (_state == State.Start) _state = State.Loop;
                _currentClip = PickNewClip();
                if (_state == State.Stopping) {
                    _stopClipPlaying = true;
                }
                if (_currentClip == null) {
                    _state = State.Stopped; 
                    return;
                }
                _source.PlayOneShot(_currentClip);
            }
        }
        
        public AudioClip PickNewClip() {
            switch (_state) {
                case State.Starting:
                    _state = State.Start;
                    break;
                case State.Start:
                    
                    break;
                case State.Loop:
                    break;
                case State.ReadyToStop:
                    break;
                case State.Stopping:
                    break;
                case State.Stopped:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            if (_state == State.Start && startClips.Count <= 0) _state = State.Loop;
            if (_state == State.Loop && loopClips.Count <= 0) _state = State.Stopping;
            switch (_state) {
                case State.Start:
                    return LapsMath.PickRandomFromList(startClips);
                case State.Loop:
                    return LapsMath.PickRandomFromList(loopClips);
                case State.Stopping:
                    return LapsMath.PickRandomFromList(endClips);
                default: return null;
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
