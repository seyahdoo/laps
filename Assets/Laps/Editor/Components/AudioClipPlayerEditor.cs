using LapsRuntime;
using UnityEditor;
using UnityEngine;

namespace LapsEditor {
    [CustomEditor(typeof(AudioClipPlayer))]
    public class AudioClipPlayerEditor : Editor {
        public override void OnInspectorGUI() {
            base.OnInspectorGUI();
            EnsureSettings();
        }
        private void OnSceneGUI() {
            EnsureSettings();
        }
        public void EnsureSettings() {
            var audioClipPlayer = (AudioClipPlayer) target;
            var audioSource = audioClipPlayer.GetComponent<AudioSource>();
            if (audioSource.playOnAwake) {
                Debug.LogError(
                    $"{nameof(AudioClipPlayer)} cannot have an {nameof(AudioSource)} with play on awake enabled. Set play on awake at {nameof(audioClipPlayer)}");
                audioSource.playOnAwake = false;
            }
            if (audioSource.clip != null) {
                Debug.LogError(
                    $"{nameof(AudioClipPlayer)} cannot have an {nameof(AudioSource)} with audio clip. Set clips at {nameof(audioClipPlayer)}");
                audioSource.clip = null;
            }
        }
    }
}
