using Shiroi.FX.Effects.Requirements;
using Shiroi.FX.Features;
using UnityEngine;
using UnityUtilities;

namespace Shiroi.FX.Effects.BuiltIn {
    [RequiresFeature(typeof(PositionFeature))]
    public class AudioEffect : Effect {
        public AudioClip[] Clips;
        public float Volume = 0.6F;

        public override void Play(EffectContext context) {
            var position = context.GetRequiredFeature<PositionFeature>().Position;
            var clip = Clips.RandomElement();
            AudioSource.PlayClipAtPoint(clip, position, Volume);
        }
    }
}