using Shiroi.FX.Colors;
using UnityEngine;
using UnityUtilities;

namespace Shiroi.FX.Effects {
    public class ParticleEffect : Effect {
        public ParticleSystem ParticlePrefab;
        public bool ForceDestroyOnFinished = true;

        private void OnEnable() {
            var main = ParticlePrefab.main;
            main.playOnAwake = false;
        }

        public override void Play(EffectContext context) {
            var position = context.GetRequiredFeature<PositionFeature>().Position;
            // Check if color features are present
            var color = context.GetOptionalFeature<ColorFeature>();
            var gradient = context.GetOptionalFeature<GradientFeature>();
            var minMaxColor = context.GetOptionalFeature<MinMaxColorFeature>();
            var minMaxGradient = context.GetOptionalFeature<MinMaxGradientFeature>();
            // If more than one are provided, 
            this.CheckIncompatibleFeatures(color, gradient, minMaxColor, minMaxGradient);

            var instance = ParticlePrefab.Clone(position);

            if (color != null) {
                SetParticleColor(instance, color.Color);
            }

            if (gradient != null) {
                SetParticleColor(instance, gradient.Gradient);
            }

            if (minMaxColor != null) {
                SetParticleColor(instance, minMaxColor);
            }

            if (minMaxGradient != null) {
                SetParticleColor(instance, minMaxGradient);
            }

            if (!ForceDestroyOnFinished) {
                return;
            }

            var main = instance.main;
            main.stopAction = ParticleSystemStopAction.Destroy;
            if (!main.playOnAwake) {
                instance.Play();
            }
        }


        private static void SetParticleColor(ParticleSystem instance, MinMaxGradientFeature minMaxGradient) {
            var main = instance.main;
            var color = main.startColor;
            color.gradientMin = minMaxGradient.MinGradient;
            color.gradientMax = minMaxGradient.MaxGradient;
            color.mode = ParticleSystemGradientMode.TwoGradients;
        }

        private static void SetParticleColor(ParticleSystem instance, MinMaxColorFeature particleColor) {
            var main = instance.main;
            var color = main.startColor;
            color.colorMin = particleColor.ColorMin;
            color.colorMax = particleColor.ColorMax;
            color.mode = ParticleSystemGradientMode.TwoColors;
        }

        private static void SetParticleColor(ParticleSystem instance, Gradient gradient) {
            var main = instance.main;
            var color = main.startColor;
            color.gradient = gradient;
            color.mode = ParticleSystemGradientMode.Gradient;
        }

        private static void SetParticleColor(ParticleSystem instance, Color particleColor) {
            var main = instance.main;
            var mainColor = main.startColor;
            mainColor.color = particleColor;
            mainColor.mode = ParticleSystemGradientMode.Color;
        }
    }
}