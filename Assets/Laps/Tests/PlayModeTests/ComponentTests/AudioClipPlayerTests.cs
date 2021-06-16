using System.Collections;
using LapsRuntime;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace LapsPlayModeTests {
    public class AudioClipPlayerTests {
        public AudioClipPlayer audioClipPlayer;
        public AudioSource audioSource;
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
        public IEnumerator AudioSourceAwakeStatusWontGenerateAudio() {
            Assert.Pass("cannot test this");
            yield return null;
        }
        [UnityTest]
        public IEnumerator OneCLipInStartClip() {
            
            yield return null;
        }
        [UnityTest]
        public IEnumerator TwoClipInStartClipPicksRandom() {
            yield return null;
        }
        [UnityTest]
        public IEnumerator OnlyLoop() {
            yield return null;
        }
        [UnityTest]
        public IEnumerator StartAndLoop() {
            yield return null;
        }
        [UnityTest]
        public IEnumerator StartAndLoopAndEnd() {
            yield return null;
        }
        [UnityTest]
        public IEnumerator PauseAndContunie() {
            yield return null;
        }
        [UnityTest]
        public IEnumerator PlayOnAwake() {
            yield return null;
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
