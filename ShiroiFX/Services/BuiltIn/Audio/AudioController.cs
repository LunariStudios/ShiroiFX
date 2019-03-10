using System.Collections;
using UnityEngine;
using Lunari.Tsuki.Singletons;

namespace Shiroi.FX.Services.BuiltIn.Audio {
    [RequireComponent(typeof(AudioPool))]
    public class AudioController : Singleton<AudioController> {
        public AudioPool Pool;

        public void PlayAudioEvent(AudioEvent audioEvent) {
            var source = Pool.Get();
            if (source == null) {
                Debug.LogWarning(string.Format("Unable to play audio event {0} because the audio pool was unable to provide an Audio Source", audioEvent));
                return;
            }


            source.PlayOneShot(audioEvent.Clip);
            StartCoroutine(RunAudioEvent(source, audioEvent));
        }

        private IEnumerator RunAudioEvent(AudioSource source, AudioEvent audio) {
            float currentTime = 0;
            var attachment = audio.Attachment;
            if (attachment != null) {
                source.transform.parent = attachment;
            } else {
                source.transform.position = audio.Position;
            }

            source.outputAudioMixerGroup = audio.Group;
            var loop = audio.Loop;
            source.loop = loop;
            var duration = loop ? audio.LoopDuration : audio.Clip.length;
            while ((currentTime += Time.deltaTime) < duration) {
                source.transform.position += audio.Velocity * Time.deltaTime;
                var pos = currentTime / duration;
                source.volume = audio.Volume.Evaluate(pos);
                source.pitch = audio.Pitch.Evaluate(pos);
                yield return null;
            }

            Pool.Return(source);
        }
    }
}