using System.Collections.Generic;
using Shiroi.FX.Utilities;

namespace Shiroi.FX.Effects.BuiltIn {
    [Icon(Icons.ListIcon)]
    public class CompositeEffect : Effect {
        public List<Effect> Effects = new List<Effect>();

        public override void Play(EffectContext context) {
            foreach (var effect in Effects) {
                effect.Play(context);
            }
        }
    }
}