using UnityEngine;

namespace Shiroi.FX.Effects {
    public abstract class Effect : ScriptableObject {
        public abstract void Play(EffectContext context);
    }
}