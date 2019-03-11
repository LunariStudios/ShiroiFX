using System;
using Shiroi.FX.Utilities;
using UnityEngine.Rendering;

namespace Shiroi.FX.Services {
    /// <summary>
    /// A service of which its execution continues indefinitely until explicit cancelled using <see cref="Cancel"/>.
    /// </summary>
    /// <typeparam name="T">The type of the meta this service carries.</typeparam>
    public class ContinualService<T> : Service<T> {
        public ContinualService(T meta, ushort priority) : base(meta, priority) {
            Cancelled = false;
        }

        /// <summary>
        /// Whether or not this service should finish it's execution on the next tick.
        /// </summary>
        public bool Cancelled { get; private set; }

        public override bool Tick() {
            (Meta as ITickable)?.Tick();
            return Cancelled;
        }

        public override void Cancel() {
            Cancelled = true;
        }
    }
}