using System.Collections.Generic;
using System.Linq;
using Shiroi.FX.Effects;
using Shiroi.FX.Effects.BuiltIn;
using Shiroi.FX.Utilities;
using UnityEngine;

namespace Shiroi.FX.Services.BuiltIn {
    public class ObjectShakeController : ServiceController<ShakeMeta> {
        public GameObject Object;
        public Vector3 DefaultPosition;

        protected override void UpdateGameToDefault() {
            Object.transform.position = DefaultPosition;
        }

        protected override void UpdateGameTo(IEnumerable<WeightnedMeta<ShakeMeta>> activeMetas) {
            var result = activeMetas.Aggregate(Vector3.zero,
                (current, weightedMeta) => current + weightedMeta.Meta.GetShake() * weightedMeta.Weight
            );

            Object.transform.position = result;
        }

        protected override void UpdateGameTo(ShakeMeta meta) {
            Object.transform.position = meta.GetShake();
        }
    }

    public class ShakeMeta : ITimedServiceTickable {
        private float totalDuration;
        private float timePassed;
        private ContinousModularFloat frequency;
        private ContinousModularFloat intensity;
        private EffectContext context;

        private ObjectShakeEffect.ShakeMode mode;
        private ObjectShakeEffect.ShakeDimension dimensions;
        private Vector3 currentShake;

        public void Tick(ITimedService service) {
            var values = new ObjectShakeEffect.PingPongValues();
            values.Randomize();
            float timeUntilNextShake = 0;
            while (timePassed >= totalDuration) {
                if (timeUntilNextShake < 0) {
                    currentShake =
                        ObjectShakeEffect.GetOffset(context, timePassed, values, mode, intensity, dimensions);
                    timeUntilNextShake = 1 / frequency.Evaluate(context, timePassed);
                    values.Toggle();
                }

                timePassed += Time.deltaTime;
                timeUntilNextShake -= Time.deltaTime;
            }
        }

        public Vector3 GetShake() {
            return currentShake;
        }
    }
}