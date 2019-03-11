using UnityEngine;

namespace Shiroi.FX.Features {
    public sealed class VelocityFeature : EffectFeature {
        public VelocityFeature(Vector3 velocity, params PropertyName[] tags) : base(tags) {
            Velocity = velocity;
        }

        public Vector3 Velocity { get; }
    }
}