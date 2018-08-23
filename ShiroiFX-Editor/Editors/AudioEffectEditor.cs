using Shiroi.FX.Effects.BuiltIn;
using UnityEditor;
using UnityEditor.AnimatedValues;
using UnityEditorInternal;
using UnityEngine;

namespace Shiroi.FX.Editor.Editors {
    [CustomEditor(typeof(AudioEffect))]
    [CanEditMultipleObjects]
    public class AudioEffectEditor : UnityEditor.Editor {
        public static readonly GUIContent HeaderTitle = new GUIContent("Audio Effect");
        public static readonly GUIContent HeaderSubtitle = new GUIContent("An audio effect plays an audio dynamically");
        public static readonly GUIContent GeneralTitle = new GUIContent("General");
        public static readonly GUIContent GeneralSubtitle = new GUIContent("Clips, volume and pitch configuration");
        public static readonly GUIContent LoopTitle = new GUIContent("Looping");
        public static readonly GUIContent LoopSubtitle = new GUIContent("Loop information, such as duration and mode");
        private AnimBool loops;
        private ReorderableList clipList;

        private void OnEnable() {
            loops = new AnimBool();
            loops.valueChanged.AddListener(Repaint);
            var clips = serializedObject.FindProperty("Clips");
            clipList = new ReorderableList(serializedObject, clips) {
                drawElementCallback = DrawClip,
                drawHeaderCallback = DrawClipsHeader
            };
        }

        private void DrawClipsHeader(Rect rect) {
            EditorGUI.LabelField(rect, "Audio Clips");
        }

        private void DrawClip(Rect rect, int index, bool isactive, bool isfocused) {
            EditorGUI.PropertyField(
                new Rect(rect.x, rect.y, rect.width, EditorGUIUtility.singleLineHeight),
                serializedObject.FindProperty("Clips").GetArrayElementAtIndex(index),
                GUIContent.none);
        }

        public override void OnInspectorGUI() {
            var skin = EditorGUIUtility.GetBuiltinSkin(EditorSkin.Inspector);
            EditorGUILayout.BeginVertical(skin.box);
            ShiroiFXGUI.DrawTitle(HeaderTitle, HeaderSubtitle);
            EditorGUI.BeginChangeCheck();
            DrawVolume();
            DrawLoop();
            if (EditorGUI.EndChangeCheck()) {
                serializedObject.ApplyModifiedProperties();
            }

            EditorGUILayout.EndVertical();
        }

        private void DrawLoop() {
            ShiroiFXGUI.DrawTitle(LoopTitle, LoopSubtitle);

            ShiroiFXGUI.DrawFadeProperty(
                ref loops,
                serializedObject.FindProperty("Loop"),
                delegate {
                    EditorGUI.indentLevel++;
                    EditorGUILayout.PropertyField(serializedObject.FindProperty("LoopDuration"));
                    EditorGUI.indentLevel--;
                }
            );
        }

        private void DrawVolume() {
            ShiroiFXGUI.DrawTitle(GeneralTitle, GeneralSubtitle);
            clipList.DoLayoutList();
            EditorGUILayout.PropertyField(serializedObject.FindProperty("Volume"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("Pitch"));
        }
    }
}