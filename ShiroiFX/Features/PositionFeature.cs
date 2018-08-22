using UnityEngine;

namespace Shiroi.FX.Features {
    public sealed class PositionFeature : EffectFeature {
        private readonly Vector3 position;


        public PositionFeature(Vector3 position, params PropertyName[] tags) : base(tags) {
            this.position = position;
        }

        public Vector3 Position {
            get {
                return position;
            }
        }
    }
}