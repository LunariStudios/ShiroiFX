using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityUtilities;

namespace Shiroi.FX.Services {
    public class WeightnedMeta<T> {
        public WeightnedMeta(float weight, T meta) {
            this.Weight = weight;
            this.Meta = meta;
        }

        public float Weight {
            get;
            private set;
        }

        public T Meta {
            get;
            private set;
        }
    }

    /// <summary>
    /// The abstract representation of a feature that can be controlled by multiple different "sources" in your game.
    /// <br/>
    /// An example of a controller: 
    /// </summary>
    /// <typeparam name="T">The type of meta data this controller accepts</typeparam>
    /// <seealso cref="Service{T}"/>
    public abstract class ServiceController<T> : MonoBehaviour {
        private readonly List<Service> activeServices = new List<Service>();
        private bool hadActiveServiceLastFrame;

        private void Update() {
            if (activeServices.IsEmpty()) {
                if (hadActiveServiceLastFrame) {
                    ResetState();
                }

                return;
            }

            hadActiveServiceLastFrame = true;
            activeServices.RemoveAll(ServiceUpdater);
            var highestPriority = activeServices.Max(service => service.Priority);
            var metas = GetAllActiveServices().Select(service
                => new WeightnedMeta<T>((float) service.Priority / highestPriority, service.Meta));
            UpdateGameTo(metas);
        }

        private void ResetState() {
            hadActiveServiceLastFrame = false;
            UpdateGameToDefault();
        }

        /// <summary>
        /// Called when the last active service finishes execution, and the feature this <see cref="ServiceController{T}"/>
        /// controls should return to it's default state. 
        /// </summary>
        protected abstract void UpdateGameToDefault();

        protected abstract void UpdateGameTo(IEnumerable<WeightnedMeta<T>> activeMetas);

        private static bool ServiceUpdater(Service obj) {
            return obj.Tick();
        }

        public TimedService<T> RegisterTimedService(float duration, T meta, ushort priority = Service.DefaultPriority) {
            var service = new TimedService<T>(duration, meta, priority);
            RegisterService(service);
            return service;
        }

        public ContinualService<T> RegisterContinualService(T meta, ushort priority = Service.DefaultPriority) {
            var service = new ContinualService<T>(meta, priority);
            RegisterService(service);
            return service;
        }

        public void RegisterService(Service service) {
            activeServices.Add(service);
        }

        public IEnumerable<Service<T>> GetAllActiveServices() {
            foreach (var service in activeServices) {
                var s = service as Service<T>;
                if (s != null) {
                    yield return s;
                }
            }
        }
    }
}