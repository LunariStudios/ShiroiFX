using Shiroi.FX.Effects;
using Shiroi.FX.Features;
using UnityEngine;

namespace Shiroi.FX.Utilities {
    public static class Effects {
        public static bool TryGetOptionalFeature<F>(this EffectContext context, out F feature) where F : EffectFeature {
            return (feature = context.GetOptionalFeature<F>()) != null;
        }

        public static bool TryGetOptionalFeatureWithTags<F>(
            this EffectContext context,
            out F feature,
            params PropertyName[] tags
        ) where F : EffectFeature {
            return (feature = context.GetOptionalFeatureWithTags<F>(tags)) != null;
        }
    }
}