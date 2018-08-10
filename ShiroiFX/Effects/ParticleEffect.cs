using UnityEngine;
using UnityUtilities;

namespace Shiroi.FX.Effects {
    public class ParticleEffect : WorldEffect {
        public ParticleSystem ParticlePrefab;
        public bool ForceDestroyOnFinished = true;

        public override void Execute(Vector3 position) {
            var instance = ParticlePrefab.Clone(position);
            if (!ForceDestroyOnFinished) {
                return;
            }

            var main = instance.main;
            main.stopAction = ParticleSystemStopAction.Destroy;
        }
    }
}