using System;
using System.Linq;
using Shiroi.FX.Effects.BuiltIn;
using UnityEditor;
using UnityEditor.AnimatedValues;
using UnityEngine;

namespace Shiroi.FX.Editor {
    public static class ShiroiFXGUI {
        public static void DrawAnimatedOrConstantValue(
            GUIContent label,
            GUIContent modeLabel,
            ref ValueControlMode current,
            ref AnimBool usesContantValue,
            ref float constant,
            ref AnimationCurve curve) {
            current = (ValueControlMode) EditorGUILayout.EnumPopup(modeLabel, current);
            usesContantValue.target = current == ValueControlMode.Constant;
            var fadeValue = usesContantValue.faded;

            if (EditorGUILayout.BeginFadeGroup(fadeValue)) {
                constant = EditorGUILayout.FloatField(label, constant);
            }

            EditorGUILayout.EndFadeGroup();
            if (EditorGUILayout.BeginFadeGroup(1 - fadeValue)) {
                curve = EditorGUILayout.CurveField(label, curve);
            }

            EditorGUILayout.EndFadeGroup();
        }

        public static void DrawTitle(GUIContent title, GUIContent subtitle) {
            GUILayout.Space(5);
            EditorGUILayout.LabelField(title, EditorStyles.boldLabel);
            EditorGUILayout.LabelField(subtitle, ShiroiFXEditorResources.Subtitle);
            var rect = GUILayoutUtility.GetRect(10, 200, 1, 1, GUILayout.ExpandWidth(true));
            var oldColor = GUI.color;
            GUI.color = ShiroiFXEditorResources.TitleLineColor;
            GUI.DrawTexture(rect, EditorGUIUtility.whiteTexture);
            GUI.color = oldColor;
            GUILayout.Space(5);
        }

        public static void DrawFadeProperty(ref AnimBool loops, SerializedProperty property, Action draw) {
            EditorGUILayout.PropertyField(property);
            loops.target = property.boolValue;
            if (EditorGUILayout.BeginFadeGroup(loops.faded)) {
                draw();
            }

            EditorGUILayout.EndFadeGroup();
        }

        public static EditorGUILayout.VerticalScope EffectBox(out GUISkin skin) {
            skin = EditorGUIUtility.GetBuiltinSkin(EditorSkin.Inspector);
            return new EditorGUILayout.VerticalScope(skin.box);
        }

        public static void DrawAndApplyProperties(SerializedObject obj, params string[] properties) {
            DrawAndApplyProperties(obj, properties.Select(obj.FindProperty).ToArray());
        }

        public static void DrawAndApplyProperties(SerializedObject obj, params SerializedProperty[] properties) {
            if (DrawProperties(properties)) {
                obj.ApplyModifiedProperties();
            }
        }

        public static bool DrawProperties(params SerializedProperty[] properties) {
            var modified = false;
            foreach (var property in properties) {
                EditorGUI.BeginChangeCheck();
                EditorGUILayout.PropertyField(property);
                if (EditorGUI.EndChangeCheck()) {
                    modified = true;
                }
            }

            return modified;
        }
    }
}