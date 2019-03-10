using System;
using Shiroi.FX.Utilities;
using UnityEditor;
using UnityEngine;
using Lunari.Tsuki.Editor;

namespace Shiroi.FX.Editor.Drawers {
    [CustomPropertyDrawer(typeof(ModularRange))]
    public class ModularRangeDrawer : PropertyDrawer {
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label) {
            var mode = (ModularRange.RangeMode) property.FindPropertyRelative("Mode").enumValueIndex;
            int lines;
            switch (mode) {
                case ModularRange.RangeMode.Constant:
                    lines = 1;
                    break;
                case ModularRange.RangeMode.RandomBetweenConstants:
                    lines = 3;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            return lines * EditorGUIUtility.singleLineHeight;
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
            Rect fieldRect = position;
            var modeProperty = property.FindPropertyRelative("Mode");
            var mode = (ModularRange.RangeMode) modeProperty.enumValueIndex;

            // Mode
            fieldRect.width -= Styles.stateButtonWidth;
            var modeRect = new Rect(fieldRect.xMax, fieldRect.y, Styles.stateButtonWidth, fieldRect.height);
            EditorGUI.BeginProperty(modeRect, GUIContent.none, modeProperty);
            EditorGUI.BeginChangeCheck();
            var skin = EditorGUIUtility.GetBuiltinSkin(EditorSkin.Inspector);
            var style = skin.GetStyle("IN MinMaxStateDropdown");
            int newSelection = EditorGUI.Popup(modeRect, null, (int) mode, Styles.modes, style);
            if (EditorGUI.EndChangeCheck()) {
                modeProperty.enumValueIndex = newSelection;
            }

            switch (mode) {
                case ModularRange.RangeMode.Constant:
                    EditorGUI.PropertyField(fieldRect, property.FindPropertyRelative("Constant"), label);
                    break;

                case ModularRange.RangeMode.RandomBetweenConstants:
                    EditorGUI.PropertyField(fieldRect, property.FindPropertyRelative("Range"), label);
                    break;
            }
        }

        public static class Styles {
            public static readonly float stateButtonWidth = 18;

            public static readonly GUIContent[] modes = new[] {
                EditorGUIUtility.TrTextContent("Constant"),
                EditorGUIUtility.TrTextContent("Random Between Two Constants")
            };
        }
    }
}