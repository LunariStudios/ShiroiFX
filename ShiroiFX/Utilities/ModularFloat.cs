using System;
using Shiroi.FX.Effects;
using Shiroi.FX.Features;
using UnityEngine;

namespace Shiroi.FX.Utilities {
    public enum PropertySource {
        Effect,
        Context
    }

    public abstract class ModularFloat {
        public PropertySource Source;
        public string TagName;
    }

    [Serializable]
    public sealed class ContinousModularFloat : ModularFloat {
        public ParticleSystem.MinMaxCurve Value;

        public static implicit operator ContinousModularFloat(float value) {
            return new ContinousModularFloat {Value = value};
        }

        public float Evaluate(EffectContext context, float time) {
            switch (Source) {
                case PropertySource.Effect:
                    return Value.Evaluate(time);
                case PropertySource.Context:
                    return context.GetRequiredFeatureWithTags<FloatFeature>(TagName).Value;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }


    [Serializable]
    public sealed class TimelessModularFloat : ModularFloat {
        public ModularRange Value;

        public static implicit operator TimelessModularFloat(float value) {
            return new TimelessModularFloat {Value = value};
        }

        public float Evaluate(EffectContext context) {
            switch (Source) {
                case PropertySource.Effect:
                    return Value.Evaluate();
                case PropertySource.Context:
                    return context.GetRequiredFeatureWithTags<FloatFeature>(TagName).Value;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}