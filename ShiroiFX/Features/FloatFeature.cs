using UnityEngine;

namespace Shiroi.FX.Features {
    public class FloatFeature : EffectFeature {
        public FloatFeature(float value, params PropertyName[] tags) : base(tags) {
            Value = value;
        }

        public float Value { get; }
    }
}