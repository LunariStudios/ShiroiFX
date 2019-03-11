using System;
using UnityEngine;
using UnityEngine.Audio;

namespace Shiroi.FX.Services.BuiltIn.Audio {
    [Serializable]
    public struct AudioEvent {
        [SerializeField]
        private AudioClip clip;

        [SerializeField]
        private ParticleSystem.MinMaxCurve pitch;

        [SerializeField]
        private ParticleSystem.MinMaxCurve volume;

        [SerializeField]
        private bool loop;

        [SerializeField]
        private float loopDuration;

        [SerializeField]
        private Transform attachment;

        [SerializeField]
        private Vector3 position;

        [SerializeField]
        private Vector3 velocity;

        public AudioEvent(
            AudioClip clip,
            ParticleSystem.MinMaxCurve pitch,
            ParticleSystem.MinMaxCurve volume,
            bool loop,
            float loopDuration,
            AudioMixerGroup @group = null,
            Transform attachment = null
        ) : this() {
            this.clip = clip;
            this.pitch = pitch;
            this.volume = volume;
            Group = @group;
            this.loop = loop;
            this.loopDuration = loopDuration;
            this.attachment = attachment;
        }

        public AudioEvent(
            AudioClip clip,
            ParticleSystem.MinMaxCurve pitch,
            ParticleSystem.MinMaxCurve volume,
            Vector3 position,
            bool loop,
            float loopDuration,
            AudioMixerGroup group = null,
            Vector3 velocity = default(Vector3)) : this() {
            this.clip = clip;
            this.pitch = pitch;
            this.volume = volume;
            this.loop = loop;
            this.loopDuration = loopDuration;
            Group = @group;
            this.position = position;
            this.velocity = velocity;
        }

        public Vector3 Position {
            get {
                return attachment == null ? position : attachment.position;
            }
        }

        public AudioClip Clip {
            get {
                return clip;
            }
        }

        public ParticleSystem.MinMaxCurve Pitch {
            get {
                return pitch;
            }
        }

        public ParticleSystem.MinMaxCurve Volume {
            get {
                return volume;
            }
        }

        public Transform Attachment {
            get {
                return attachment;
            }
        }

        public Vector3 Velocity {
            get {
                return velocity;
            }
        }

        public AudioMixerGroup Group { get; }

        public bool Loop {
            get {
                return loop;
            }
        }

        public float LoopDuration {
            get {
                return loopDuration;
            }
        }
    }
}