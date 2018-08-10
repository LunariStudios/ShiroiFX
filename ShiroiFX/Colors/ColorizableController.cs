using System.Collections.Generic;
using Shiroi.FX.Services;
using UnityEngine;

namespace Shiroi.FX.Colors {
    public class ColorizableController : ServiceController<ColorMeta> {
        public Colorizable Colorizable;

        protected override void UpdateGameToDefault() {
            Colorizable.Color = Colorizable.DefaultColor;
        }

        protected override void UpdateGameTo(IEnumerable<WeightnedMeta<ColorMeta>> activeMetas) {
            var color = Vector4.zero;
            foreach (var weightnedMeta in activeMetas) {
                color += (Vector4) weightnedMeta.Meta.GetColor() * weightnedMeta.Weight;
            }

            color.Normalize();
            Colorizable.Color = color;
        }
    }

    public abstract class ColorMeta {
        public abstract Color GetColor();
    }

    public class ConstantColorMeta : ColorMeta {
        public Color Color;

        public ConstantColorMeta(Color color) {
            Color = color;
        }

        public override Color GetColor() {
            return Color;
        }
    }

    public class GradientColorMeta : ColorMeta, ITimedServiceTickable {
        public Gradient Gradient;
        private float currentPosition;

        public GradientColorMeta(Gradient gradient) {
            Gradient = gradient;
            currentPosition = 0;
        }

        public override Color GetColor() {
            return Gradient.Evaluate(currentPosition);
        }

        public void Tick(ITimedService service) {
            currentPosition = service.PercentageCompleted;
        }
    }
}