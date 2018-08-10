using Shiroi.FX.Effects;
using UnityEngine;

namespace Shiroi.FX.Utilities {
    public static class EffectUtilities {
        public static void ExecuteIfPresent(this GameEffect effect) {
            if (effect != null) {
                effect.Execute();
            }
        }

        public static void ExecuteIfPresent(this WorldEffect effect, Vector3 position) {
            if (effect != null) {
                effect.Execute(position);
            }
        }
    }
}