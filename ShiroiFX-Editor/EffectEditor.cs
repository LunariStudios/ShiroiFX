using System;
using System.Linq;
using Shiroi.FX.Effects;
using Shiroi.FX.Effects.Requirements;
using UnityEditor;
using UnityEngine;
using UnityUtilities.Editor;

namespace Shiroi.FX.Editor {
    [CustomEditor(typeof(Effect), true)]
    public class EffectEditor : UnityEditor.Editor {
        private Effect effect;
        private RequiresFeatureAttribute requiredFeature;
        private OptinalFeatureAttribute optinalFeature;

        private void OnEnable() {
            effect = (Effect) target;
            var type = effect.GetType();
            TryLoadAttribute(type, out requiredFeature);
            TryLoadAttribute(type, out optinalFeature);
        }

        public static void TryLoadAttribute<T>(Type type, out T output) {
            var customAttributes = type.GetCustomAttributes(typeof(T), true) as T[];
            if (customAttributes == null || customAttributes.Length <= 0) {
                output = default(T);
            } else {
                output = customAttributes.First();
            }
        }

        public override void OnInspectorGUI() {
            AttemptDrawAttributes(requiredFeature, optinalFeature);
            DrawDefaultInspector();
        }

        public static void AttemptDrawAttributes(RequiresFeatureAttribute requiredFeature,
            OptinalFeatureAttribute optinalFeature) {
            var hasAttributes = requiredFeature != null || optinalFeature != null;
            var skin = EditorGUIUtility.GetBuiltinSkin(EditorSkin.Inspector);
            if (hasAttributes) {
                EditorGUILayout.BeginVertical(skin.box);
            }

            if (requiredFeature != null) {
                DrawRequiredFeatures(requiredFeature, skin);
            }

            if (optinalFeature != null) {
                DrawOptinalFeatures(optinalFeature, skin);
            }

            if (hasAttributes) {
                EditorGUILayout.EndVertical();
            }
        }

        private static void DrawOptinalFeatures(OptinalFeatureAttribute optinalFeature, GUISkin skin) {
            EditorGUILayout.LabelField("This effect optinally uses:", skin.GetStyle(GUISkinProperties.HeaderLabel));
            foreach (var feature in optinalFeature.FeatureTypes) {
                EditorGUILayout.LabelField(feature.Name);
            }
        }

        private static void DrawRequiredFeatures(RequiresFeatureAttribute requiredFeature, GUISkin skin) {
            EditorGUILayout.LabelField("This effect requires the following features:",
                skin.GetStyle(GUISkinProperties.HeaderLabel));
            foreach (var feature in requiredFeature.FeatureTypes) {
                EditorGUILayout.LabelField(feature.Name);
            }
        }
    }
}