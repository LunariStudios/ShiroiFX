using UnityEngine;
using UnityEngine.Playables;

namespace Shiroi.FX {
    public abstract class GameEffect : ScriptableObject {
        public abstract void Execute();
    }

    public abstract class WorldEffect : ScriptableObject {
        public abstract void Execute(Vector3 position);
    }
}