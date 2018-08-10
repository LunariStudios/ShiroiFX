using UnityEngine;

namespace Shiroi.FX.Effects {
    public abstract class GameEffect : ScriptableObject {
        public abstract void Execute();
    }
}