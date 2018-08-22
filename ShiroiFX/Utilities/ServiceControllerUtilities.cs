using Shiroi.FX.Services;

namespace Shiroi.FX.Utilities {
    public static class ServiceControllerUtilities {
        public static TimedService<T> RegisterTimedService<T>(this ServiceController<T> controller, float duration,
            T meta, bool ignoreTimeScale = false, ushort priority = Service.DefaultPriority) {
            var service = new TimedService<T>(duration, meta, ignoreTimeScale, priority);
            controller.RegisterService(service);
            return service;
        }

        public static ContinualService<T> RegisterContinualService<T>(this ServiceController<T> controller, T meta,
            ushort priority = Service.DefaultPriority) {
            var service = new ContinualService<T>(meta, priority);
            controller.RegisterService(service);
            return service;
        }
    }
}