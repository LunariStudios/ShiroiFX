using Shiroi.FX.Effects;
using Shiroi.FX.Features;
using UnityEngine;
using Lunari.Tsuki;

namespace Shiroi.FX.Colors {
    public sealed class ColorFeature : EffectFeature {
        private readonly Color color;

        public ColorFeature(Color color, params PropertyName[] tags) : base(tags) {
            this.color = color;
        }

        public Color Color {
            get {
                return color;
            }
        }
    }

    public sealed class GradientFeature : EffectFeature {
        private readonly Gradient gradient;

        public GradientFeature(Gradient gradient, params PropertyName[] tags) : base(tags) {
            this.gradient = gradient;
        }

        public Gradient Gradient {
            get {
                return gradient;
            }
        }
    }

    public sealed class MinMaxColorFeature : EffectFeature {
        private readonly Color colorMin, colorMax;

        public MinMaxColorFeature(Color colorMin, Color colorMax, params PropertyName[] tags) : base(tags) {
            this.colorMin = colorMin;
            this.colorMax = colorMax;
        }

        public Color ColorMin {
            get {
                return colorMin;
            }
        }

        public Color ColorMax {
            get {
                return colorMax;
            }
        }

        public Color Color {
            get {
                return Color.Lerp(ColorMin, ColorMax, Random.value);
            }
        }
    }

    public sealed class RandomColorFeature : EffectFeature {
        private readonly Color[] colors;

        public RandomColorFeature(Color[] colors, params PropertyName[] tags) : base(tags) {
            this.colors = colors;
        }

        public Color[] Colors {
            get {
                return colors;
            }
        }

        public Color Color {
            get {
                return colors.RandomElement();
            }
        }
    }

    public sealed class MinMaxGradientFeature : EffectFeature {
        private readonly Gradient minGradient, maxGradient;


        public MinMaxGradientFeature(Gradient minGradient, Gradient maxGradient, params PropertyName[] tags) :
            base(tags) {
            this.minGradient = minGradient;
            this.maxGradient = maxGradient;
        }

        public Gradient MinGradient {
            get {
                return minGradient;
            }
        }

        public Gradient MaxGradient {
            get {
                return maxGradient;
            }
        }

        public Color GetColor(float time) {
            var a = MinGradient.Evaluate(time);
            var b = MaxGradient.Evaluate(time);
            return Color.Lerp(a, b, Random.value);
        }
    }
}