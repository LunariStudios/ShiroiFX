using UnityEngine;

namespace Shiroi.FX.Effects {
    public abstract class WorldEffect : ScriptableObject {
        public abstract void Execute(Vector3 position);
    }
}