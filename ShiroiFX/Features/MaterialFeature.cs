using UnityEngine;

namespace Shiroi.FX.Features {
    public class MaterialFeature : EffectFeature {
        private readonly Material material;

        public MaterialFeature(Material material, params PropertyName[] tags) : base(tags) {
            this.material = material;
        }

        public Material Material {
            get {
                return material;
            }
        }
    }
}