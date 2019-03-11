using UnityEngine;

namespace Shiroi.FX.Features {
    public class ObjectFeature<T> : EffectFeature {
        public ObjectFeature(T value, params PropertyName[] tags) : base(tags) {
            Value = value;
        }

        public T Value { get; }
    }
}