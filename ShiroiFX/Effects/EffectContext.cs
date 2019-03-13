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
        private readonly EffectFeature[] features;

        public EffectContext(MonoBehaviour host, params EffectFeature[] features) {
            this.features = features;
            Host = host;
        }

        public MonoBehaviour Host { get; }

        public Coroutine StartCoroutine(IEnumerator routine) {
            return Host.StartCoroutine(routine);
        }

        [CanBeNull]
        public F GetOptionalFeature<F>() where F : EffectFeature {
            return features.OfType<F>().FirstOrDefault();
        }

        [NotNull]
        public F GetRequiredFeature<F>() where F : EffectFeature {
            foreach (var feature in features) {
                if (feature is F f) {
                    return f;
                }
            }

            throw new FeatureNotPresentException<F>();
        }

        [CanBeNull]
        public F GetOptionalFeatureWithTags<F>(params PropertyName[] tags) where F : EffectFeature {
            foreach (var feature in features) {
                if (!(feature is F f)) {
                    continue;
                }

                if (tags.All(name => f.Tags.Contains(name))) {
                    return f;
                }
            }

            return null;
        }

        [NotNull]
        public F GetRequiredFeatureWithTags<F>(params PropertyName[] tags) where F : EffectFeature {
            foreach (var feature in features) {
                if (feature is F f) {
                    return f;
                }
            }

            throw new FeatureNotPresentException<F>();
        }
    }
}