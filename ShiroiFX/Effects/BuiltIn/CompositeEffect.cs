using System.Collections.Generic;

namespace Shiroi.FX.Effects.BuiltIn {
    public class CompositeEffect : Effect {
        public List<Effect> Effects = new List<Effect>();

        public override void Play(EffectContext context) {
            foreach (var effect in Effects) {
                effect.Play(context);
            }
        }
    }
}