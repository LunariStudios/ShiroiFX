using System;
using System.Collections;
using Shiroi.FX.Effects.Requirements;
using Shiroi.FX.Features;
using Shiroi.FX.Services.BuiltIn;
using Shiroi.FX.Utilities;
using UnityEngine;
using Lunari.Tsuki;
using Random = System.Random;

namespace Shiroi.FX.Effects.BuiltIn {
    [OptinalFeature(typeof(ObjectFeature<ObjectShakeController>))]
    [Icon(Icons.ShakeIcon)]
    public class ObjectShakeEffect : Effect {
        public const ShakeDimension AllDimensions = ShakeDimension.FirstDimension | ShakeDimension.SecondDimension |
                                                    ShakeDimension.ThirdDimension;

        public enum ShakeMode {
            PingPong,
            Random
        }

        [Flags]
        public enum ShakeDimension {
            FirstDimension = 1 << 0,
            SecondDimension = 1 << 1,
            ThirdDimension = 1 << 2
        }

        public bool UseShakeControllerIfPresent;
        public TimelessModularFloat Duration = 1;
        public ContinousModularFloat Frequency = 1;
        public ContinousModularFloat Intensity;
        public ShakeMode Mode;
        public ShakeDimension Dimensions = AllDimensions;

        public override void Play(EffectContext context) {
            if (UseShakeControllerIfPresent) {
                var f = context.GetOptionalFeature<ObjectFeature<ObjectShakeController>>();
                if (f != null) {
                    f.Value.RegisterTimedService(
                        Duration.Evaluate(context),
                        new ShakeMeta()
                    );
                    return;
                }
            }

            PlayStandalone(context);
        }


        private void PlayStandalone(EffectContext context) {
            context.StartCoroutine(PlayEffect(context));
        }

        private IEnumerator PlayEffect(EffectContext context) {
            float t = 0;
            var contextHost = context.Host;
            var tr = contextHost.transform;
            var startPosition = tr.position;
            var values = new PingPongValues();
            values.Randomize();
            var d = Duration.Evaluate(context);
            while (t < d) {
                tr.position = startPosition + GetOffset(context, t, values, Mode, Intensity, Dimensions);
                var waitTime = 1 / Frequency.Evaluate(context, t);
                yield return new WaitForSeconds(waitTime);
                t += waitTime;
                values.Toggle();
            }

            tr.position = startPosition;
        }

        public static Vector3 GetOffset(EffectContext context, float time, PingPongValues values, ShakeMode mode,
            ContinousModularFloat intensity, ShakeDimension dimensions) {
            Vector3 direction;
            switch (mode) {
                case ShakeMode.PingPong:
                    direction = Vector3.one;
                    values.Affect(ref direction);
                    break;
                case ShakeMode.Random:
                    direction = new Vector3(
                        UnityEngine.Random.value,
                        UnityEngine.Random.value,
                        UnityEngine.Random.value
                    );
                    break;
                default:
                    throw new ArgumentOutOfRangeException("mode", mode, null);
            }

            direction.Normalize();
            if ((dimensions & ShakeDimension.FirstDimension) != ShakeDimension.FirstDimension) {
                direction.x = 0;
            }

            if ((dimensions & ShakeDimension.SecondDimension) != ShakeDimension.SecondDimension) {
                direction.y = 0;
            }

            if ((dimensions & ShakeDimension.ThirdDimension) != ShakeDimension.ThirdDimension) {
                direction.z = 0;
            }

            return direction * intensity.Evaluate(context, time);
        }

        public struct PingPongValues {
            public bool x, y, z;

            public void Toggle() {
                x = !x;
                y = !y;
                z = !z;
            }

            public void Affect(ref Vector3 direction) {
                if (x) {
                    direction.x = -direction.x;
                }

                if (y) {
                    direction.y = -direction.y;
                }

                if (z) {
                    direction.z = -direction.z;
                }
            }

            public void Randomize() {
                x = Randomization.NextBool();
                y = Randomization.NextBool();
                z = Randomization.NextBool();
            }
        }
    }
}