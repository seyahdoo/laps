using LapsEditor;
using LapsRuntime;
using NUnit.Framework;
using UnityEditor;
using UnityEngine;
using UnityEngine.TestTools;
using Debug = System.Diagnostics.Debug;

namespace LapsEditModeTests {
    public class AudioClipPlayerEditModeTests {
        public AudioClipPlayer audioClipPlayer;
        public AudioSource audioSource;
        public AudioClip TestClipOne {
            get {
                var guids = AssetDatabase.FindAssets("t:audioclip laps-unhearable-test-audio-one");
                if (guids.Length <= 0) return null;
                return AssetDatabase.LoadAssetAtPath<AudioClip>(AssetDatabase.GUIDToAssetPath(guids[0]));
            }
        }
        public AudioClip TestClipTwo {
            get {
                var guids = AssetDatabase.FindAssets("t:audioclip laps-unhearable-test-audio-two");
                if (guids.Length <= 0) return null;
                return AssetDatabase.LoadAssetAtPath<AudioClip>(AssetDatabase.GUIDToAssetPath(guids[0]));
            }
        }
        [SetUp]
        public void Setup() {
            audioClipPlayer = new GameObject().AddComponent<AudioClipPlayer>();
            audioSource = audioClipPlayer.GetComponent<AudioSource>();
        }
        [TearDown]
        public void Teardown() {
            Object.DestroyImmediate(audioClipPlayer.gameObject);
        }
        [Test]
        public void AudioSourceCannotHavePlayOnAwakeEnabledAndCannotHaveClip() {
            audioSource.playOnAwake = true;
            audioSource.clip = TestClipOne;
            var editor = Editor.CreateEditor(audioClipPlayer) as AudioClipPlayerEditor;
            Debug.Assert(editor != null, nameof(editor) + " != null");
            editor.EnsureSettings();
            LogAssert.Expect(LogType.Error, $"{nameof(AudioClipPlayer)} cannot have an {nameof(AudioSource)} with play on awake enabled. Set play on awake at {nameof(audioClipPlayer)}");
            LogAssert.Expect(LogType.Error, $"{nameof(AudioClipPlayer)} cannot have an {nameof(AudioSource)} with audio clip. Set clips at {nameof(audioClipPlayer)}");
            Assert.AreEqual(false, audioSource.playOnAwake);
            Assert.AreEqual(null, audioSource.clip);
        }
    }
}