using System.Collections.Generic;
using System.Linq;
using Shiroi.FX.Features;
using UnityEngine;

namespace Shiroi.FX.Effects {
    public static class EffectExtensions {
        public delegate EffectContext ContextCreator();

        public static void PlayIfPresent(this Effect effect, EffectContext context) {
            if (effect != null) {
                effect.Play(context);
            }
        }

        public static void PlayIfPresent(this Effect effect, ContextCreator contextCreator) {
            if (effect != null) {
                effect.Play(contextCreator());
            }
        }

        public static void PlayIfPresent(this Effect effect, MonoBehaviour behaviour,
            bool includePositionFeature = true, params EffectFeature[] features) {
            var feature = new List<EffectFeature>();
            feature.AddRange(features);
            if (includePositionFeature) {
                feature.Add(new PositionFeature(behaviour.transform.position));
            }

            effect.PlayIfPresent(new EffectContext(behaviour, feature.ToArray()));
        }

        private static void ThrowIncompatibleFeaturesException(
            this Effect effect,
            params EffectFeature[] incompatibleFeatures) {
            throw new IncompatibleFeaturesException(effect, incompatibleFeatures);
        }

        public static void CheckIncompatibleFeatures(this Effect effect, params EffectFeature[] toCheck) {
            var found = (from feature in toCheck where feature != null select feature).ToList();
            if (found.Count > 1) {
                effect.ThrowIncompatibleFeaturesException(found.ToArray());
            }
        }
    }
}