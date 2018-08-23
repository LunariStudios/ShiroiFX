using UnityEngine;

namespace Shiroi.FX.Features {
    public class MinMaxCurveFeature : EffectFeature {
        private readonly ParticleSystem.MinMaxCurve curve;

        public MinMaxCurveFeature(ParticleSystem.MinMaxCurve curve, params PropertyName[] tags) : base(tags) {
            this.curve = curve;
        }

        public ParticleSystem.MinMaxCurve Curve {
            get {
                return curve;
            }
        }
    }
}