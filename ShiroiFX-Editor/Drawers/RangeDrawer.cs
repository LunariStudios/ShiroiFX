using Shiroi.FX.Utilities;
using UnityEditor;
using UnityEngine;

namespace Shiroi.FX.Editor.Drawers {
    [CustomPropertyDrawer(typeof(Range))]
    public class RangeDrawer : PropertyDrawer {
        public const float PitchRadius = 2;

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label) {
            return 3 * EditorGUIUtility.singleLineHeight;
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
            var startProp = property.FindPropertyRelative("start");
            var start = startProp.floatValue;
            var endProp = property.FindPropertyRelative("end");
            var end = endProp.floatValue;
            EditorGUI.MinMaxSlider(GetSubRectAtLine(position, 0), label, ref start, ref end, -PitchRadius, PitchRadius);

            startProp.floatValue = start;
            endProp.floatValue = end;
            EditorGUI.indentLevel++;

            EditorGUI.PropertyField(GetSubRectAtLine(position, 1), startProp);
            EditorGUI.PropertyField(GetSubRectAtLine(position, 2), endProp);
            EditorGUI.indentLevel--;
            if (startProp.floatValue > endProp.floatValue) {
                endProp.floatValue = startProp.floatValue;
            }

            if (endProp.floatValue < startProp.floatValue) {
                startProp.floatValue = endProp.floatValue;
            }
        }

        private static Rect GetSubRectAtLine(Rect rect, int line) {
            rect.yMin += line * EditorGUIUtility.singleLineHeight;
            rect.size = new Vector2(rect.size.x, EditorGUIUtility.singleLineHeight);
            return rect;
        }
    }
}