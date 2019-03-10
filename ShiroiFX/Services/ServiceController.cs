using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Lunari.Tsuki;

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

    public enum ServiceBlendMode {
        Blend,
        UseHighestPriority,
        UseNewest,
        UseHighestPriorityThenNewest
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
        public ServiceBlendMode BlendMode;

        private void Update() {
            if (activeServices.IsEmpty()) {
                if (hadActiveServiceLastFrame) {
                    ResetState();
                }

                return;
            }

            hadActiveServiceLastFrame = true;
            activeServices.RemoveAll(ServiceUpdater);
            if (activeServices.IsNullOrEmpty()) {
                return;
            }

            switch (BlendMode) {
                case ServiceBlendMode.Blend:
                    var totalPriority = activeServices.Sum(service => service.Priority);
                    var metas = GetAllActiveServices().Select(service
                        => new WeightnedMeta<T>((float) service.Priority / totalPriority, service.Meta));
                    UpdateGameTo(metas);
                    break;
                default:
                    var s = GetServiceToUse() as Service<T>;
                    if (s == null) {
                        return;
                    }

                    UpdateGameTo(s.Meta);
                    break;
            }
        }

        private static Service FindNewest(IEnumerable<Service> services) {
            ITimedService timedService = null;
            foreach (var activeService in services) {
                var s = activeService as ITimedService;
                if (s == null) {
                    //Doesn't have time, consider duration always 0
                    return activeService;
                }

                if (timedService == null || timedService.TimeLeft < s.TimeLeft) {
                    timedService = s;
                }
            }

            return (Service) timedService;
        }

        private Service GetServiceToUse() {
            switch (BlendMode) {
                case ServiceBlendMode.UseHighestPriority:
                    return activeServices.MaxBy(service => service.Priority);

                case ServiceBlendMode.UseNewest:
                    return FindNewest(activeServices);

                case ServiceBlendMode.UseHighestPriorityThenNewest:
                    var highestPriority = activeServices.Max(service => service.Priority);
                    var services = new List<Service>();
                    foreach (var activeService in activeServices) {
                        if (activeService.Priority == highestPriority) {
                            services.Add(activeService);
                        }
                    }

                    if (services.Count == 0) {
                        return services.First();
                    }

                    return FindNewest(services);
            }

            return null;
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
        protected abstract void UpdateGameTo(T meta);

        private static bool ServiceUpdater(Service obj) {
            return obj.Tick();
        }

        public virtual void RegisterService(Service service) {
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

    public abstract class SingletonServiceController<S, T> : ServiceController<T>
        where S : SingletonServiceController<S, T> {
        private static S loadedInstance;

        public static S Instance {
            get {
                return loadedInstance == null ? (loadedInstance = FindObjectOfType<S>()) : loadedInstance;
            }
        }
    }
}