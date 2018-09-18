using System;
using System.Collections;
using Shiroi.FX.Services;
using Shiroi.FX.Services.BuiltIn;
using Shiroi.FX.Utilities;
using UnityEngine;

namespace Shiroi.FX.Effects.BuiltIn {
    [Icon(Icons.TimeIcon)]
    public class FreezeFrameEffect : Effect {
        [Tooltip("The effect scale to be used when in the Constant mode")]
        public float ConstantTimeScale;

        [Tooltip("The effect scale to be used when in the Animated mode")]
        public AnimationCurve AnimatedTimeScale = AnimationCurve.EaseInOut(0, 0, 1, 1);

        [Tooltip("The total duration in seconds of the effect. (Ignoring the time scale)")]
        public float Duration = 0.1F;

        [Tooltip(
            "How should the new time scale be selected: " +
            "Constant you set ConstantTimeScale to. " +
            "Animated evaluates AnimatedTimeScale using PassedDuration / Duration"
        )]
        public ValueControlMode Mode;

        [Tooltip(
            "If there is an active TimeController on the scene, this effect will be run as a service instead." +
            " (This allows time scale blending if more than one FreezeFrameEffect is playing)"
        )]
        public bool UseTimeControllerIfPresent = true;

        [Tooltip(
            "When the effect is executed using a Time Controller, this is the priority used for the service."
        )]
        public ushort ServicePriority = Service.DefaultPriority;

        public override void Play(EffectContext context) {
            var controller = TimeController.Instance;
            if (UseTimeControllerIfPresent && controller != null) {
                controller.RegisterTimedService(Duration, CreateTimeMeta(), priority: ServicePriority);
            } else {
                context.StartCoroutine(PlayDefault());
            }
        }

        private TimeMeta CreateTimeMeta() {
            switch (Mode) {
                case ValueControlMode.Constant:
                    return new ConstantTimeMeta(ConstantTimeScale);
                case ValueControlMode.Animated:
                    return new AnimatedTimeMeta(AnimatedTimeScale);
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private float GetTimeScale(float time) {
            switch (Mode) {
                case ValueControlMode.Constant:
                    return ConstantTimeScale;
                case ValueControlMode.Animated:
                    return AnimatedTimeScale.Evaluate(time);
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private IEnumerator PlayDefault() {
            float currentTime = 0;
            while (currentTime < Duration) {
                Time.timeScale = GetTimeScale(currentTime / Duration);
                currentTime += Time.unscaledDeltaTime;
                yield return null;
            }
        }
    }
}