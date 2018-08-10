using UnityEngine;
namespace Shiroi.FX.Services {
    public interface ITimedServiceTickable {
        void Tick(ITimedService service);
    }


    public interface ITimedService {
        float TotalDuration {
            get;
        }

        float TimeLeft {
            get;
        }

        float PercentageCompleted {
            get;
        }
    }

    public class TimedService<T> : Service<T>, ITimedService {
        public TimedService(float duration, T meta, ushort priority = DefaultPriority) : base(meta, priority) {
            TotalDuration = duration;
            TimeLeft = duration;
        }

        public float TotalDuration {
            get;
            private set;
        }

        public float TimeLeft {
            get;
            private set;
        }

        public float PercentageCompleted {
            get {
                return 1 - TimeLeft / TotalDuration;
            }
        }

        public override bool Tick() {
            var timed = Meta as ITimedServiceTickable;
            if (timed != null) {
                timed.Tick(this);
            }

            return (TimeLeft -= Time.deltaTime) <= 0;
        }

        public override void Cancel() {
            TimeLeft = 0;
        }

        public TimedService(ushort priority, T meta) : base(meta, priority) { }
    }
}