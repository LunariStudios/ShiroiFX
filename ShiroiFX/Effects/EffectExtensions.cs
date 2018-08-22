using System.Linq;
using Shiroi.FX.Features;
using UnityUtilities;

namespace Shiroi.FX.Effects {
    public static class EffectExtensions {
        public static void ThrowIncompatibleFeaturesException(
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