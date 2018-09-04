using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Shiroi.FX.Services.BuiltIn.Camera {
    public class FOVController : ServiceController<FOVMeta> {
        public float DefaultFOV = 60;
        public UnityEngine.Camera Camera;

        private void Start() {
            if (Camera == null) {
                Camera = UnityEngine.Camera.main;
            }
        }

        protected override void UpdateGameToDefault() {
            if (Camera == null) {
                return;
            }

            Camera.fieldOfView = DefaultFOV;
        }

        protected override void UpdateGameTo(IEnumerable<WeightnedMeta<FOVMeta>> activeMetas) {
            if (Camera == null) {
                return;
            }

            Camera.fieldOfView = activeMetas.Sum(meta => meta.Weight * meta.Meta.GetFOV());
        }

        protected override void UpdateGameTo(FOVMeta meta) {
            Camera.fieldOfView = meta.GetFOV();
        }
    }

    public abstract class FOVMeta {
        public abstract float GetFOV();
    }

    public class ConstantFOVMeta : FOVMeta {
        public float fieldOfView;

        public ConstantFOVMeta(float fieldOfView) {
            this.fieldOfView = fieldOfView;
        }

        public override float GetFOV() {
            return fieldOfView;
        }
    }

    public class AnimatedFOVMeta : FOVMeta, ITimedServiceTickable {
        public AnimationCurve Curve;

        public AnimatedFOVMeta(AnimationCurve curve) {
            Curve = curve;
            currentPosition = 0;
        }

        private float currentPosition;

        public override float GetFOV() {
            return Curve.Evaluate(currentPosition);
        }

        public void Tick(ITimedService service) {
            currentPosition = service.PercentageCompleted;
        }
    }
}