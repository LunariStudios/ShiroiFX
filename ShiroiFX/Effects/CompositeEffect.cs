using System.Collections.Generic;
using UnityEngine;


namespace Shiroi.FX.Effects {
    public class CompositeEffect : WorldEffect {
        public List<WorldEffect> Effects = new List<WorldEffect>();
        public List<GameEffect> GameEffects = new List<GameEffect>();

        public override void Execute(Vector3 position) {
            foreach (var fx in Effects) {
                fx.Execute(position);
            }

            foreach (var fx in GameEffects) {
                fx.Execute();
            }
        }
    }
}