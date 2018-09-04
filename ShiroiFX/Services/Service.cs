namespace Shiroi.FX.Services {
    /// <summary>
    /// This represents a request that was made to a <see cref="ServiceController{T}"/>.
    /// <br/>
    /// Since this concept is so abstract, you can also think of this as the "representation of the will of an object to
    /// change a value of a feature", for example: Time Scale, Material Colors, Particle Colors, etc.
    /// </summary>
    public abstract class Service {
        public const ushort DefaultPriority = 100;

        public Service(ushort priority) {
            Priority = priority;
        }

        /// <summary>
        /// Updates this service and checks whether or not it finished execution.
        /// </summary>
        /// <returns>
        /// <see langword="true"/> if the service has finished execution and should be removed from the active services
        /// list, otherwise, <see langword="false"/>.
        /// </returns>
        public abstract bool Tick();

        /// <summary>
        /// Cancels this event, stopping further execution.
        /// </summary>
        public abstract void Cancel();

        /// <summary>
        /// Used to calculate the weight proportinal to other services on the final result of whatever feature this service's
        /// <see cref="ServiceController{T}"/> controls.
        /// <br/>
        /// In other words, when a <see cref="ServiceController{T}"/> has multiple active services, this value is used
        /// to calculate how much this service affects the end result. Meaning services with higher priority will have
        /// a larger impact than services with lower ones.
        /// </summary>
        public ushort Priority {
            get;
            set;
        }
    }

    /// <summary>
    /// An <see cref="Service"/> than can contain an object as metadata which can be used by the
    /// <see cref="ServiceController{T}"/> that executes this service.
    /// </summary>
    /// <typeparam name="T">The type of the meta this service carries.</typeparam>
    public abstract class Service<T> : Service {
        /// <summary>
        /// Additional information that can used by this service's <see cref="ServiceController{T}"/>
        /// </summary>
        public T Meta;

        protected Service(T meta, ushort priority = DefaultPriority) : base(priority) {
            Meta = meta;
        }
    }
}