using System;
using Shiroi.FX.Effects.Requirements;
using Shiroi.FX.Features;
using Shiroi.FX.Services.BuiltIn.Audio;
using Shiroi.FX.Utilities;
using UnityEngine;
using UnityEngine.Audio;
using UnityUtilities;
using Random = UnityEngine.Random;

namespace Shiroi.FX.Effects.BuiltIn {
    [OptinalFeature(
        typeof(PositionFeature),
        typeof(VelocityFeature),
        typeof(MinMaxCurveFeature),
        typeof(FloatFeature)
    )]
    [RequirementsDescription(
        "The position at which the sound will be played, if not present, the source will be attached to the context's host.",
        "The velocity of the audio source (causes doppler effect).",
        "Used for pitch and/or volume if their sources are set to be the context",
        "Used for loop duration if it's in use and source is set to be the context"
    )]
    public class AudioEffect : Effect {
        public AudioClip[] Clips;
        public ContinousModularFloat Volume = 0.6F;
        public ContinousModularFloat Pitch = 1;
        public bool Loop;
        public TimelessModularFloat LoopDuration = 2;
        public bool UseAudioControllerIfPresent;
        public AudioMixerGroup Mixer;

        public override void Play(EffectContext context) {
            var position = context.GetOptionalFeature<PositionFeature>();
            var clip = Clips.RandomElement();
            AudioController controller;
            if (UseAudioControllerIfPresent && (controller = AudioController.Instance) != null) {
                PlayWithController(position, context, clip, controller);
            } else {
                var pos = position == null ? context.Host.transform.position : position.Position;
                PlayStandalone(pos, clip, context);
            }
        }

        private void PlayStandalone(Vector3 position, AudioClip clip, EffectContext context) {
            AudioSource.PlayClipAtPoint(clip, position, LoadVolume(context).Evaluate(Random.value));
        }

        private void PlayWithController(PositionFeature position, EffectContext context, AudioClip clip,
            AudioController controller) {
            AudioEvent audioEvent;
            var volume = LoadVolume(context);
            var pitch = LoadPitch(context);
            var loopDuration = LoadLoopDuration(context);
            if (position == null) {
                audioEvent = new AudioEvent(clip, pitch, volume, Loop, loopDuration, Mixer, context.Host.transform);
            } else {
                var velFeature = context.GetOptionalFeature<VelocityFeature>();
                var velocity = velFeature != null ? velFeature.Velocity : Vector3.zero;
                audioEvent = new AudioEvent(clip, pitch, volume, position.Position, Loop, loopDuration, Mixer,
                    velocity);
            }

            controller.PlayAudioEvent(audioEvent);
        }

        private float LoadLoopDuration(EffectContext context) {
            switch (LoopDuration.Source) {
                case PropertySource.Effect:
                    return LoopDuration.Value.Evaluate();
                case PropertySource.Context:
                    return context.GetRequiredFeatureWithTags<FloatFeature>(LoopDuration.TagName).Value;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private ParticleSystem.MinMaxCurve LoadPitch(EffectContext context) {
            switch (Pitch.Source) {
                case PropertySource.Effect:
                    return Pitch.Value;
                case PropertySource.Context:
                    return context.GetRequiredFeatureWithTags<MinMaxCurveFeature>(Pitch.TagName).Curve;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private ParticleSystem.MinMaxCurve LoadVolume(EffectContext context) {
            switch (Volume.Source) {
                case PropertySource.Effect:
                    return Volume.Value;
                case PropertySource.Context:
                    return context.GetRequiredFeatureWithTags<MinMaxCurveFeature>(Volume.TagName).Curve;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}