using System.Collections.Generic;
using UnityEngine;

namespace Shiroi.FX.Features {
    public abstract class EffectFeature {
        protected EffectFeature(params PropertyName[] tags) {
            Tags = tags;
        }

        public PropertyName[] Tags { get; }

        private bool Equals(EffectFeature other) {
            return Equals(Tags, other.Tags);
        }

        public override bool Equals(object obj) {
            if (ReferenceEquals(null, obj)) {
                return false;
            }

            if (ReferenceEquals(this, obj)) {
                return true;
            }

            return obj is EffectFeature feature && Equals(feature);
        }

        public override int GetHashCode() {
            return (Tags != null ? Tags.GetHashCode() : 0);
        }
    }
}