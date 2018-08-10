using Shiroi.FX.Utilities;

namespace Shiroi.FX.Services {
    /// <summary>
    /// A service of which its execution continues indefinitely until explicit cancelled using <see cref="Cancel"/>.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ContinualService<T> : Service<T> {
        public ContinualService(T meta, ushort priority) : base(meta, priority) {
            Cancelled = false;
        }

        /// <summary>
        /// Whether or not this service should finish it's execution on the next tick.
        /// </summary>
        public bool Cancelled {
            get;
            private set;
        }

        public override bool Tick() {
            var t = Meta as ITickable;
            if (t != null) {
                t.Tick();
            }

            return Cancelled;
        }

        public override void Cancel() {
            Cancelled = true;
        }
    }
}