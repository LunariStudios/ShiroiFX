using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Shiroi.FX.Services.BuiltIn {
    public class TimeController : ServiceController<TimeMeta> {
        public float DefaultTimeScale = 1;

        public TimedService<T> RequestTimedService<T>(T meta, float duration, ushort priority = Service.DefaultPriority)
            where T : TimeMeta {
            var service = new TimedService<T>(duration, meta, priority);
            RegisterService(service);
            return service;
        }

        protected override void UpdateGameToDefault() {
            Time.timeScale = DefaultTimeScale;
        }

        protected override void UpdateGameTo(IEnumerable<WeightnedMeta<TimeMeta>> activeMetas) {
            Time.timeScale = activeMetas.Sum(weightnedMeta => weightnedMeta.Weight * weightnedMeta.Meta.GetTimeScale());
        }
    }

    public class AnimatedTimeMeta : TimeMeta, ITimedServiceTickable {
        public AnimationCurve Curve;
        private float currentPosition;

        public override float GetTimeScale() {
            return Curve.Evaluate(currentPosition);
        }

        public void Tick(ITimedService service) {
            currentPosition = service.PercentageCompleted;
        }
    }

    public class ConstantTimeMeta : TimeMeta {
        public float TimeScale;

        public ConstantTimeMeta(float timeScale) {
            TimeScale = timeScale;
        }

        public override float GetTimeScale() {
            return TimeScale;
        }
    }

    public abstract class TimeMeta {
        public abstract float GetTimeScale();
    }
}