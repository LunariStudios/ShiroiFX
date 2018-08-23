using Shiroi.FX.Utilities;
using UnityEditor;
using UnityEngine;

namespace Shiroi.FX.Editor.Drawers {
    [CustomPropertyDrawer(typeof(ModularFloat), true)]
    public class ModularFloatDrawer : PropertyDrawer {
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label) {
            var source = (PropertySource) property.FindPropertyRelative("Source").enumValueIndex;
            var lines = 3;
            if (source != PropertySource.Context) {
                if (property.type != typeof(TimelessModularFloat).Name)
                    return lines * EditorGUIUtility.singleLineHeight;
                var mode = (ModularRange.RangeMode) property.FindPropertyRelative("Value").FindPropertyRelative("Mode").enumValueIndex;
                if (mode == ModularRange.RangeMode.RandomBetweenConstants) {
                    lines = 5;
                }
            }

            return lines * EditorGUIUtility.singleLineHeight;
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
            EditorGUI.LabelField(GetSubRectAtLine(position, 0), label);
            EditorGUI.indentLevel++;
            var source = property.FindPropertyRelative("Source");
            EditorGUI.PropertyField(GetSubRectAtLine(position, 1), source);
            var isProvidedByEffect = source.enumValueIndex == (int) PropertySource.Effect;
            var valRect = GetSubRectAtLine(position, 2);

            EditorGUI.PropertyField(valRect, isProvidedByEffect ? property.FindPropertyRelative("Value") : property.FindPropertyRelative("TagName"));


            EditorGUI.indentLevel--;
        }

        private static Rect GetSubRectAtLine(Rect rect, int line) {
            rect.yMin += line * EditorGUIUtility.singleLineHeight;
            rect.size = new Vector2(rect.size.x, EditorGUIUtility.singleLineHeight);
            return rect;
        }
    }
}