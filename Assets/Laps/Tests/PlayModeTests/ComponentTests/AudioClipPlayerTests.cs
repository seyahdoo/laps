using System.Collections;
using LapsRuntime;
using NUnit.Framework;
using UnityEditor;
using UnityEngine;
using UnityEngine.TestTools;

namespace LapsPlayModeTests {
    public class AudioClipPlayerTests {
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
        [UnityTest]
        public IEnumerator AudioSourceCannotHavePlayOnAwakeEnabledAndCannotHaveClip() {
            audioSource.playOnAwake = true;
            audioSource.clip = TestClipOne;
            Selection.activeGameObject = audioSource.gameObject;
            yield return null;
            yield return null;
            LogAssert.Expect(LogType.Error, $"{nameof(AudioClipPlayer)} cannot have an {nameof(AudioSource)} with play on awake enabled. Set play on awake at {nameof(audioClipPlayer)}");
            LogAssert.Expect(LogType.Error, $"{nameof(AudioClipPlayer)} cannot have an {nameof(AudioSource)} with audio clip. Set clips at {nameof(audioClipPlayer)}");
            Assert.AreEqual(false, audioSource.playOnAwake);
            Assert.AreEqual(null, audioSource.clip);
        }
        [UnityTest]
        public IEnumerator OneClipInStartClip() {
            audioClipPlayer.startClips.Add(TestClipOne);
            audioClipPlayer.Play();
            yield return null;
            yield return null;
            Assert.AreEqual(true, audioSource.isPlaying);
            Assert.AreEqual(TestClipOne, audioClipPlayer.CurrentPlayingClip);
        }
        [Test]
        public void TwoClipInStartClipPicksRandom() {
            audioClipPlayer.startClips.Add(TestClipOne);
            audioClipPlayer.startClips.Add(TestClipTwo);
            Random.InitState(1231);
            audioClipPlayer.Play();
            Assert.AreEqual(TestClipOne, audioClipPlayer.CurrentPlayingClip);
            audioClipPlayer.StopImmidiately();
            Assert.AreEqual(null, audioClipPlayer.CurrentPlayingClip);
            Random.InitState(42);
            audioClipPlayer.Play();
            Assert.AreEqual(TestClipTwo, audioClipPlayer.CurrentPlayingClip);
        }
        [Test]
        public void TwoClipInLoopClipPicksRandomAtStart() {
            audioClipPlayer.loopClips.Add(TestClipOne);
            audioClipPlayer.loopClips.Add(TestClipTwo);
            Random.InitState(1231);
            audioClipPlayer.Play();
            Assert.AreEqual(TestClipOne, audioClipPlayer.CurrentPlayingClip);
            audioClipPlayer.StopImmidiately();
            Assert.AreEqual(null, audioClipPlayer.CurrentPlayingClip);
            Random.InitState(42);
            audioClipPlayer.Play();
            Assert.AreEqual(TestClipTwo, audioClipPlayer.CurrentPlayingClip);
        }
        [UnityTest]
        public IEnumerator TwoClipInLoopClipPicksRandomAtLoop() {
            audioClipPlayer.loopClips.Add(TestClipOne);
            audioClipPlayer.loopClips.Add(TestClipTwo);
            Random.InitState(1231);
            audioClipPlayer.Play();
            Assert.AreEqual(TestClipOne, audioClipPlayer.CurrentPlayingClip);
            yield return new WaitForSeconds(TestClipOne.length/2f);
            Random.InitState(42);
            yield return new WaitForSeconds(TestClipOne.length/2f);
            yield return new WaitForSeconds(TestClipTwo.length/2f);
            yield return new WaitForEndOfFrame();
            Assert.AreEqual(TestClipTwo, audioClipPlayer.CurrentPlayingClip);
        }
        [UnityTest]
        public IEnumerator StartAndLoop() {
            audioClipPlayer.startClips.Add(TestClipOne);
            audioClipPlayer.loopClips.Add(TestClipTwo);
            audioClipPlayer.Play();
            Assert.AreEqual(TestClipOne, audioClipPlayer.CurrentPlayingClip);
            yield return new WaitForSeconds(TestClipOne.length);
            yield return new WaitForSeconds(TestClipTwo.length);
            yield return new WaitForEndOfFrame();
            Assert.AreEqual(TestClipTwo, audioClipPlayer.CurrentPlayingClip);
            yield return new WaitForSeconds(TestClipTwo.length/2f);
            yield return new WaitForSeconds(TestClipTwo.length/2f);
            Assert.AreEqual(TestClipTwo, audioClipPlayer.CurrentPlayingClip);
            Assert.AreEqual(true, audioSource.isPlaying);
        }
        [UnityTest]
        public IEnumerator StartAndLoopAndEnd() {
            audioClipPlayer.startClips.Add(TestClipOne);
            audioClipPlayer.loopClips.Add(TestClipTwo);
            audioClipPlayer.endClips.Add(TestClipOne);
            audioClipPlayer.Play();
            Assert.AreEqual(TestClipOne, audioClipPlayer.CurrentPlayingClip);
            yield return new WaitForSeconds(TestClipOne.length);
            yield return new WaitForSeconds(TestClipTwo.length);
            yield return new WaitForEndOfFrame();
            Assert.AreEqual(TestClipTwo, audioClipPlayer.CurrentPlayingClip);
            yield return new WaitForSeconds(TestClipTwo.length/2f);
            yield return new WaitForSeconds(TestClipTwo.length/2f);
            Assert.AreEqual(TestClipTwo, audioClipPlayer.CurrentPlayingClip);
            Assert.AreEqual(true, audioSource.isPlaying);
            audioClipPlayer.Stop();
            yield return new WaitForSeconds(TestClipTwo.length);
            Assert.AreEqual(TestClipOne, audioClipPlayer.CurrentPlayingClip);
            Assert.AreEqual(true, audioSource.isPlaying);
            yield return new WaitForSeconds(TestClipOne.length);
            Assert.AreEqual(false, audioSource.isPlaying);
            Assert.AreEqual(null, audioClipPlayer.CurrentPlayingClip);
        }
        [UnityTest]
        public IEnumerator PauseAndContunie() {
            yield return null;
            Assert.Fail("");
        }
        [UnityTest]
        public IEnumerator StartAndEndClips() {
            yield return null;
            Assert.Fail("");
        }
        [UnityTest]
        public IEnumerator PlayOnAwake() {
            yield return null;
            Assert.Fail("");
        }
        [Test]
        public void Slots() {
            var slots = new SlotList();
            audioClipPlayer.GetInputSlots(slots);
            Assert.AreEqual(4, slots.Count);
            Assert.AreEqual(new LogicSlot("play", 0), slots[0]);
            Assert.AreEqual(new LogicSlot("pause", 1), slots[1]);
            Assert.AreEqual(new LogicSlot("stop", 2), slots[2]);
            Assert.AreEqual(new LogicSlot("stop immidiately", 3), slots[3]);
            slots.Clear();
            audioClipPlayer.GetOutputSlots(slots);
            Assert.AreEqual(1, slots.Count);
            Assert.AreEqual(new LogicSlot("on end", 0), slots[0]);
        }
    }
}
