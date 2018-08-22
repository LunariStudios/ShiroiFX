using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using Shiroi.FX.Colors;
using Shiroi.FX.Features;
using UnityEngine;

namespace Shiroi.FX.Effects {
    [Serializable]
    public sealed class EffectContext {
        private readonly MonoBehaviour host;
        private readonly EffectFeature[] features;

        public EffectContext(MonoBehaviour host, params EffectFeature[] features) {
            this.features = features;
            this.host = host;
        }

        public Coroutine StartCoroutine(IEnumerator routine) {
            return host.StartCoroutine(routine);
        }

        [CanBeNull]
        public F GetOptionalFeature<F>() where F : EffectFeature {
            return features.OfType<F>().FirstOrDefault();
        }

        [NotNull]
        public F GetRequiredFeature<F>() where F : EffectFeature {
            foreach (var feature in features) {
                var f = feature as F;
                if (f != null) {
                    return f;
                }
            }

            throw new FeatureNotPresentException<F>();
        }

        [CanBeNull]
        public F GetOptinalFeatureWithTags<F>(params PropertyName[] tags) where F : EffectFeature {
            foreach (var feature in features) {
                var f = feature as F;
                if (f == null) {
                    continue;
                }
            }

            return null;
        }

        [NotNull]
        public F GetRequiredFeatureWithTags<F>(params PropertyName[] tags) where F : EffectFeature {
            foreach (var feature in features) {
                var f = feature as F;
                if (f != null) {
                    return f;
                }
            }

            throw new FeatureNotPresentException<F>();
        }
    }
}