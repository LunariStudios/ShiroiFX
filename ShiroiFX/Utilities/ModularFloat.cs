using System;
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
    }


    [Serializable]
    public sealed class TimelessModularFloat : ModularFloat {
        public ModularRange Value;

        public static implicit operator TimelessModularFloat(float value) {
            return new TimelessModularFloat {Value = value};
        }

    }
}