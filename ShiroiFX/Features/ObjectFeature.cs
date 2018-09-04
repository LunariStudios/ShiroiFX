using UnityEngine;

namespace Shiroi.FX.Features {
    public class ObjectFeature<T> : EffectFeature {
        private readonly T value;

        public ObjectFeature(T value, params PropertyName[] tags) : base(tags) {
            this.value = value;
        }

        public T Value {
            get {
                return value;
            }
        }
    }
}