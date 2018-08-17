using UnityEngine;

namespace Shiroi.FX.Features {
    public abstract class EffectFeature {
        private PropertyName[] tags;

        protected EffectFeature(params PropertyName[] tags) {
            this.tags = tags;
        }
    }
}