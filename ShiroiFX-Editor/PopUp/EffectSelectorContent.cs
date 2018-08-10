using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityUtilities;

namespace Shiroi.FX.Editor.PopUp {
    public class EffectSelectorContent<T> : PopupWindowContent {
        public const float Width = 300;
        private List<Type> types;
        private Action<Type> onSelected;

        public EffectSelectorContent(Action<Type> onSelected) {
            this.types = TypeUtility.GetAllSubtypesOf<T>().ToList();
            this.onSelected = onSelected;
        }

        public override Vector2 GetWindowSize() {
            return new Vector2(Width, types.Count * EditorGUIUtility.singleLineHeight);
        }

        public override void OnGUI(Rect rect) {
            for (var i = 0; i < types.Count; i++) {
                var type = types[i];
                if (GUI.Button(GetSubRect(rect, i), GetContent(type))) {
                    onSelected(type);
                }
            }
        }

        private GUIContent GetContent(Type type) {
            return new GUIContent(ObjectNames.NicifyVariableName(type.Name));
        }

        private static Rect GetSubRect(Rect rect, int index) {
            return new Rect(rect.x, rect.y + (index * EditorGUIUtility.singleLineHeight), Width, EditorGUIUtility.singleLineHeight);
        }
    }
}