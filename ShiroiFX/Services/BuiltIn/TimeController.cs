using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Shiroi.FX.Services.BuiltIn {
    public class TimeController : SingletonServiceController<TimeController, TimeMeta> {
        public float DefaultTimeScale = 1;

        protected override void UpdateGameToDefault() {
            Time.timeScale = DefaultTimeScale;
        }

        protected override void UpdateGameTo(IEnumerable<WeightnedMeta<TimeMeta>> activeMetas) {
            Time.timeScale = activeMetas.Sum(weightnedMeta => weightnedMeta.Weight * weightnedMeta.Meta.GetTimeScale());
        }

        protected override void UpdateGameTo(TimeMeta meta) {
            Time.timeScale = meta.GetTimeScale();
        }

        public override void RegisterService(Service service) {
            base.RegisterService(service);
            var timed = service as ITimedService;
            if (timed != null && !timed.IgnoreTimeScale) {
                timed.IgnoreTimeScale = true;
            }
        }
    }

    public class AnimatedTimeMeta : TimeMeta, ITimedServiceTickable {
        public AnimationCurve Curve;

        public AnimatedTimeMeta(AnimationCurve curve) {
            Curve = curve;
            currentPosition = 0;
        }

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