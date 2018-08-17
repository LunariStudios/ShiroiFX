using System;
using Shiroi.FX.Features;

namespace Shiroi.FX.Effects {
    public class FeatureNotPresentException<T> : Exception {
        public FeatureNotPresentException() : base(
            string.Format(
                "A feature of type {0} was requested from a EffectContext, but there wasn't any effect of that type present.",
                typeof(T).Name)) { }
    }

    public class IncompatibleFeaturesException : Exception {
        public IncompatibleFeaturesException(Effect effect, EffectFeature[] features) : base(
            string.Format("Effect {0} was fed with incompatibles features [{1}]", effect.name, JoinFeatures(features))
        ) { }

        private static string JoinFeatures(EffectFeature[] features) {
            var stringFinal = string.Empty;
            var l = features.Length;
            for (var i = 0; i < l; i++) {
                var feature = features[i];
                stringFinal += feature.GetType().Name;
                if (i < l - 1) { }

                stringFinal += ", ";
            }

            return stringFinal;
        }
    }
}