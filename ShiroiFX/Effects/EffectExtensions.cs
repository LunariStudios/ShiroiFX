using System.Linq;
using UnityUtilities;

namespace Shiroi.FX.Effects {
    public static class EffectExtensions {
        public static void ThrowIncompatibleFeaturesException(this Effect effect,
            params EffectFeature[] incompatibleFeatures) {
            throw new IncompatibleFeaturesException(effect, incompatibleFeatures);
        }

        public static void CheckIncompatibleFeatures(this Effect effect, params EffectFeature[] toCheck) {
            var found = toCheck.Where(feature => feature != null).ToList();
            if (found.IsEmpty()) {
                return;
            }

            effect.ThrowIncompatibleFeaturesException(found.ToArray());
        }
    }
}