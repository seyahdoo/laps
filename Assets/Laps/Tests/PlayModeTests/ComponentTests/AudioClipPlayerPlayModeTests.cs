using System.Collections;
using LapsEditModeTests;
using LapsEditor;
using LapsRuntime;
using NUnit.Framework;
using UnityEditor;
using UnityEngine;
using UnityEngine.TestTools;

namespace LapsPlayModeTests {
    public class AudioClipPlayerPlayModeTests {
        public AudioListener audioListener;
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
            audioListener = new GameObject().AddComponent<AudioListener>();
            audioClipPlayer = new GameObject().AddComponent<AudioClipPlayer>();
            audioSource = audioClipPlayer.GetComponent<AudioSource>();
        }
        [TearDown]
        public void Teardown() {
            Object.DestroyImmediate(audioClipPlayer.gameObject);
            Object.DestroyImmediate(audioListener.gameObject);
        }
        [UnityTest]
        public IEnumerator OneClipInStartClip() {
            audioClipPlayer.startClips.Add(TestClipOne);
            audioClipPlayer.Play();
            yield return new WaitForEndOfFrame();
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
            Assert.AreEqual(TestClipTwo, audioClipPlayer.CurrentPlayingClip);
        }
        [UnityTest]
        public IEnumerator StartAndLoop() {
            audioClipPlayer.startClips.Add(TestClipOne);
            audioClipPlayer.loopClips.Add(TestClipTwo);
            audioClipPlayer.Play();
            Assert.AreEqual(TestClipOne, audioClipPlayer.CurrentPlayingClip);
            yield return new WaitForSeconds(TestClipOne.length);
            yield return new WaitForSeconds(TestClipTwo.length/2f);
            Assert.AreEqual(TestClipTwo, audioClipPlayer.CurrentPlayingClip);
            yield return new WaitForSeconds(TestClipTwo.length);
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
            yield return new WaitForSeconds(TestClipTwo.length/2f);
            Assert.AreEqual(TestClipTwo, audioClipPlayer.CurrentPlayingClip);
            yield return new WaitForSeconds(TestClipTwo.length);
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
        public IEnumerator StopImmediately() {
            audioClipPlayer.startClips.Add(TestClipOne);
            audioClipPlayer.loopClips.Add(TestClipTwo);
            audioClipPlayer.endClips.Add(TestClipOne);
            audioClipPlayer.Play();
            Assert.AreEqual(TestClipOne, audioClipPlayer.CurrentPlayingClip);
            yield return new WaitForSeconds(TestClipOne.length);
            yield return new WaitForSeconds(TestClipTwo.length/2f);
            Assert.AreEqual(TestClipTwo, audioClipPlayer.CurrentPlayingClip);
            yield return new WaitForSeconds(TestClipTwo.length);
            Assert.AreEqual(TestClipTwo, audioClipPlayer.CurrentPlayingClip);
            Assert.AreEqual(true, audioSource.isPlaying);
            audioClipPlayer.StopImmidiately();
            Assert.AreEqual(false, audioSource.isPlaying);
            Assert.AreEqual(null, audioClipPlayer.CurrentPlayingClip);
            yield return new WaitForSeconds(TestClipTwo.length);
            yield return new WaitForSeconds(TestClipOne.length);
            Assert.AreEqual(null, audioClipPlayer.CurrentPlayingClip);
            Assert.AreEqual(false, audioSource.isPlaying);
        }
        [UnityTest]
        public IEnumerator PauseAndContunie() {
            audioClipPlayer.startClips.Add(TestClipOne);
            audioClipPlayer.Play();
            Assert.AreEqual(TestClipOne, audioClipPlayer.CurrentPlayingClip);
            yield return new WaitForSeconds(TestClipOne.length/2f);
            audioClipPlayer.Pause();
            Assert.AreEqual(false ,audioSource.isPlaying);
            audioClipPlayer.Play();
            Assert.AreEqual(true ,audioSource.isPlaying);
            Assert.AreEqual(TestClipOne, audioClipPlayer.CurrentPlayingClip);
            yield return new WaitForSeconds(TestClipOne.length);
            Assert.AreEqual(false ,audioSource.isPlaying);
        }
        [UnityTest]
        public IEnumerator StartAndEndClips() {
            audioClipPlayer.startClips.Add(TestClipOne);
            audioClipPlayer.endClips.Add(TestClipTwo);
            audioClipPlayer.Play();
            Assert.AreEqual(TestClipOne, audioClipPlayer.CurrentPlayingClip);
            yield return new WaitForSeconds(TestClipOne.length);
            yield return new WaitForSeconds(TestClipTwo.length/2f);
            Assert.AreEqual(TestClipTwo, audioClipPlayer.CurrentPlayingClip);
            yield return new WaitForSeconds(TestClipTwo.length);
            Assert.AreEqual(null, audioClipPlayer.CurrentPlayingClip);
            Assert.AreEqual(false, audioSource.isPlaying);
        }
        [Test]
        public void PlayOnAwake() {
            audioClipPlayer.playOnAwake = true;
            audioClipPlayer.startClips.Add(TestClipOne);
            audioClipPlayer.Awake();
            Assert.AreEqual(true, audioSource.isPlaying);
            Assert.AreEqual(TestClipOne, audioClipPlayer.CurrentPlayingClip);
        }
        [Test]
        public void PlaySlot() {
            audioClipPlayer.playOnAwake = false;
            audioClipPlayer.startClips.Add(TestClipOne);
            audioClipPlayer.HandleInput(0, null, null);
            Assert.AreEqual(true, audioSource.isPlaying);
            Assert.AreEqual(TestClipOne, audioClipPlayer.CurrentPlayingClip);
        }
        [UnityTest]
        public IEnumerator PauseSlot() {
            audioClipPlayer.startClips.Add(TestClipOne);
            audioClipPlayer.Play();
            Assert.AreEqual(TestClipOne, audioClipPlayer.CurrentPlayingClip);
            yield return new WaitForSeconds(TestClipOne.length/2f);
            audioClipPlayer.HandleInput(1, null, null);
            Assert.AreEqual(false ,audioSource.isPlaying);
            audioClipPlayer.Play();
            Assert.AreEqual(true ,audioSource.isPlaying);
            Assert.AreEqual(TestClipOne, audioClipPlayer.CurrentPlayingClip);
            yield return new WaitForSeconds(TestClipOne.length);
            Assert.AreEqual(false ,audioSource.isPlaying);
        }
        [UnityTest]
        public IEnumerator StopSlot() {
            audioClipPlayer.startClips.Add(TestClipOne);
            audioClipPlayer.loopClips.Add(TestClipTwo);
            audioClipPlayer.endClips.Add(TestClipOne);
            audioClipPlayer.Play();
            Assert.AreEqual(TestClipOne, audioClipPlayer.CurrentPlayingClip);
            yield return new WaitForSeconds(TestClipOne.length);
            yield return new WaitForSeconds(TestClipTwo.length/2f);
            Assert.AreEqual(TestClipTwo, audioClipPlayer.CurrentPlayingClip);
            yield return new WaitForSeconds(TestClipTwo.length);
            Assert.AreEqual(TestClipTwo, audioClipPlayer.CurrentPlayingClip);
            Assert.AreEqual(true, audioSource.isPlaying);
            audioClipPlayer.HandleInput(2, null, null);
            yield return new WaitForSeconds(TestClipTwo.length);
            Assert.AreEqual(TestClipOne, audioClipPlayer.CurrentPlayingClip);
            Assert.AreEqual(true, audioSource.isPlaying);
            yield return new WaitForSeconds(TestClipOne.length);
            Assert.AreEqual(false, audioSource.isPlaying);
            Assert.AreEqual(null, audioClipPlayer.CurrentPlayingClip);
        }
        [UnityTest]
        public IEnumerator StopImmidiatelySlot() {
            audioClipPlayer.startClips.Add(TestClipOne);
            audioClipPlayer.loopClips.Add(TestClipTwo);
            audioClipPlayer.endClips.Add(TestClipOne);
            audioClipPlayer.Play();
            Assert.AreEqual(TestClipOne, audioClipPlayer.CurrentPlayingClip);
            yield return new WaitForSeconds(TestClipOne.length);
            yield return new WaitForSeconds(TestClipTwo.length/2f);
            Assert.AreEqual(TestClipTwo, audioClipPlayer.CurrentPlayingClip);
            yield return new WaitForSeconds(TestClipTwo.length);
            Assert.AreEqual(TestClipTwo, audioClipPlayer.CurrentPlayingClip);
            Assert.AreEqual(true, audioSource.isPlaying);
            audioClipPlayer.HandleInput(3, null, null);
            Assert.AreEqual(false, audioSource.isPlaying);
            Assert.AreEqual(null, audioClipPlayer.CurrentPlayingClip);
            yield return new WaitForSeconds(TestClipTwo.length);
            yield return new WaitForSeconds(TestClipOne.length);
            Assert.AreEqual(null, audioClipPlayer.CurrentPlayingClip);
            Assert.AreEqual(false, audioSource.isPlaying);
        }
        [UnityTest]
        public IEnumerator OnEndSlot() {
            audioClipPlayer.startClips.Add(TestClipOne);
            audioClipPlayer.Play();
            var testComponent = new GameObject().AddComponent<TestComponent>();
            LogicModule.Connect(audioClipPlayer, 0, testComponent, 0);
            yield return new WaitForSeconds(TestClipOne.length / 2f);
            Assert.AreEqual(0, testComponent.inputCallCount);
            yield return new WaitForSeconds(TestClipOne.length);
            Assert.AreEqual(1, testComponent.inputCallCount);
        }
        [UnityTest]
        public IEnumerator StopImmidiatelyOnEndSlot() {
            audioClipPlayer.startClips.Add(TestClipOne);
            audioClipPlayer.Play();
            var testComponent = new GameObject().AddComponent<TestComponent>();
            LogicModule.Connect(audioClipPlayer, 0, testComponent, 0);
            yield return new WaitForSeconds(TestClipOne.length / 2f);
            Assert.AreEqual(0, testComponent.inputCallCount);
            audioClipPlayer.StopImmidiately();
            Assert.AreEqual(1, testComponent.inputCallCount);
            yield return new WaitForSeconds(TestClipOne.length);
            Assert.AreEqual(1, testComponent.inputCallCount);
        }
        [UnityTest]
        public IEnumerator StopImmidiatelyWhileStopped() {
            audioClipPlayer.startClips.Add(TestClipOne);
            audioClipPlayer.Play();
            var testComponent = new GameObject().AddComponent<TestComponent>();
            LogicModule.Connect(audioClipPlayer, 0, testComponent, 0);
            yield return new WaitForSeconds(TestClipOne.length / 2f);
            Assert.AreEqual(0, testComponent.inputCallCount);
            audioClipPlayer.StopImmidiately();
            Assert.AreEqual(1, testComponent.inputCallCount);
            audioClipPlayer.StopImmidiately();
            Assert.AreEqual(1, testComponent.inputCallCount);
            yield return new WaitForSeconds(TestClipOne.length);
            audioClipPlayer.StopImmidiately();
            Assert.AreEqual(1, testComponent.inputCallCount);
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
