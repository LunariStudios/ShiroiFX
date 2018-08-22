using System;
using System.Linq;
using Shiroi.FX.Effects.BuiltIn;
using Shiroi.FX.Effects.Requirements;
using UnityEditor;
using UnityEditor.AnimatedValues;
using UnityEngine;

namespace Shiroi.FX.Editor {
    [CustomEditor(typeof(FreezeFrameEffect))]
    public class FreezeFrameEffectEditor : UnityEditor.Editor {
        public static readonly GUIContent FreezeFrameTitle = new GUIContent(
            "Freeze Frame Effect"
        );

        public static readonly GUIContent FreezeFrameSubtitle = new GUIContent(
            "Slows down time using the provided parameters"
        );

        public static readonly GUIContent DurationContent = new GUIContent(
            "Duration",
            "The total duration in seconds of the effect. (Ignoring the time scale)"
        );

        public static readonly GUIContent TimeScaleContent =
            new GUIContent("Time Scale", "The time scale to be set when the effect is active");

        public static readonly GUIContent TimeScaleModeContent = new GUIContent(
            "Time Scale Mode",
            "How should the new time scale be selected: either Animated (which is evaluated using PassedDuration / Duration), or Constant."
        );

        public static readonly GUIContent TimeScaleTitle = new GUIContent("Time Scale");

        public static readonly GUIContent TimeScaleSubtitleHeader =
            new GUIContent("Controls how much slowdown there is.");

        public static readonly GUIContent TimeControllerTitle = new GUIContent("Time Controller");

        public static readonly GUIContent TimeControllerSubtitle = new GUIContent(
            "Controls blending between multiple FreezeFrameEffects"
        );

        public static readonly GUIContent UseTimeControllerContent = new GUIContent("Use Time Controller if present",
            "If there is an active TimeController on the scene, this effect will be run as a service instead." +
            " (This allows time scale blending if more than one FreezeFrameEffect is playing)");

        public static readonly GUIContent ServicePriority = new GUIContent("Service priority");

        private FreezeFrameEffect effect;
        private RequiresFeatureAttribute requiredFeature;
        private OptinalFeatureAttribute optinalFeature;
        private AnimBool usesContantValue;
        private AnimBool usesTimeController;

        private void OnEnable() {
            InitAnimBool(out usesContantValue);
            InitAnimBool(out usesTimeController);


            effect = (FreezeFrameEffect) target;
            var type = effect.GetType();
            TryLoadAttribute(type, out requiredFeature);
            TryLoadAttribute(type, out optinalFeature);
        }

        private void InitAnimBool(out AnimBool animBool) {
            animBool = new AnimBool();
            animBool.valueChanged.AddListener(Repaint);
        }

        private static void TryLoadAttribute<T>(Type type, out T output) {
            var customAttributes = type.GetCustomAttributes(typeof(T), true) as T[];
            if (customAttributes == null || customAttributes.Length <= 0) {
                output = default(T);
            } else {
                output = customAttributes.First();
            }
        }

        public override void OnInspectorGUI() {
            EffectEditor.AttemptDrawAttributes(requiredFeature, optinalFeature);
            var skin = EditorGUIUtility.GetBuiltinSkin(EditorSkin.Inspector);
            EditorGUILayout.BeginVertical(skin.box);
            ShiroiFXEditor.DrawTitle(FreezeFrameTitle, FreezeFrameSubtitle);
            DrawEffectTimeScale();
            DrawUseTimeController();
            EditorGUILayout.EndVertical();
        }

        private void DrawUseTimeController() {
            ShiroiFXEditor.DrawTitle(TimeControllerTitle, TimeControllerSubtitle);
            usesTimeController.target = effect.UseTimeControllerIfPresent =
                EditorGUILayout.Toggle(UseTimeControllerContent, effect.UseTimeControllerIfPresent);
            if (EditorGUILayout.BeginFadeGroup(usesTimeController.faded)) {
                effect.ServicePriority = (ushort) EditorGUILayout.IntSlider(ServicePriority, effect.ServicePriority,
                    ushort.MinValue, ushort.MaxValue);
            }

            EditorGUILayout.EndFadeGroup();
        }

        private void DrawEffectTimeScale() {
            ShiroiFXEditor.DrawTitle(TimeScaleTitle, TimeScaleSubtitleHeader);
            effect.Mode =
                (FreezeFrameEffect.FreezeFrameMode) EditorGUILayout.EnumPopup(TimeScaleModeContent, effect.Mode);
            usesContantValue.target = effect.Mode == FreezeFrameEffect.FreezeFrameMode.Constant;
            var fadeValue = usesContantValue.faded;
            if (EditorGUILayout.BeginFadeGroup(fadeValue)) {
                effect.ConstantTimeScale = EditorGUILayout.FloatField(TimeScaleContent, effect.ConstantTimeScale);
            }

            EditorGUILayout.EndFadeGroup();
            if (EditorGUILayout.BeginFadeGroup(1 - fadeValue)) {
                effect.AnimatedTimeScale = EditorGUILayout.CurveField(TimeScaleContent, effect.AnimatedTimeScale);
            }

            EditorGUILayout.EndFadeGroup();
            effect.Duration = EditorGUILayout.FloatField(DurationContent, effect.Duration);
        }
    }
}