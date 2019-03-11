using UnityEngine;

namespace Shiroi.FX.Features {
    public sealed class PositionFeature : EffectFeature {
        public PositionFeature(Vector3 position, params PropertyName[] tags) : base(tags) {
            Position = position;
        }

        public Vector3 Position { get; }
    }
}