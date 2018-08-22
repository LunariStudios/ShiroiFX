using UnityEngine;

namespace Shiroi.FX.Features {
    public sealed class VelocityFeature : EffectFeature {
        private readonly Vector3 velocity;


        public VelocityFeature(Vector3 velocity, params PropertyName[] tags) : base(tags) {
            this.velocity = velocity;
        }

        public Vector3 Velocity {
            get {
                return velocity;
            }
        }
    }
}