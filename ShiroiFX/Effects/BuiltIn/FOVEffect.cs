using System;
using Shiroi.FX.Effects.Requirements;
using Shiroi.FX.Features;
using Shiroi.FX.Services;
using Shiroi.FX.Services.BuiltIn.Camera;
using Shiroi.FX.Utilities;
using UnityEngine;

namespace Shiroi.FX.Effects.BuiltIn {
    [RequiresFeature(
        typeof(ObjectFeature<FOVController>))
    ]
    [RequirementsDescription(
        "The controller on which to play the service"
    )]
    [Icon(Icons.FOVIcon)]
    public class FOVEffect : Effect {
        public ValueControlMode Mode;
        public float ConstantFOV = 60;
        public AnimationCurve AnimatedFOV = AnimationCurve.EaseInOut(0, 65, 1, 60);
        public float Duration = 0.5F;
        public ushort ServicePriority = Service.DefaultPriority;

        public override void Play(EffectContext context) {
            var controller = context.GetRequiredFeature<ObjectFeature<FOVController>>().Value;
            FOVMeta meta;
            switch (Mode) {
                case ValueControlMode.Constant:
                    meta = new ConstantFOVMeta(ConstantFOV);
                    break;
                case ValueControlMode.Animated:
                    meta = new AnimatedFOVMeta(AnimatedFOV);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            controller.RegisterTimedService(Duration, meta, priority: ServicePriority);
        }
    }
}