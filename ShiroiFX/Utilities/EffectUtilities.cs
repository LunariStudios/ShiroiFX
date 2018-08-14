using System;
using Shiroi.FX.Effects;
using UnityEngine;

namespace Shiroi.FX.Utilities {
    public static class EffectUtilities {
        public delegate EffectContext ContextCreator();

        public static void PlayIfPresent(this Effect effect, EffectContext context) {
            if (effect != null) {
                effect.Play(context);
            }
        }

        public static void PlayIfPresent(this Effect effect, ContextCreator contextCreator) {
            if (effect != null) {
                effect.Play(contextCreator());
            }
        }
    }
}