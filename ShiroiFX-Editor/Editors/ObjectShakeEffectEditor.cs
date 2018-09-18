using Shiroi.FX.Effects.BuiltIn;
using UnityEditor;
using UnityEngine;

namespace Shiroi.FX.Editor.Editors {
    [CustomEditor(typeof(ObjectShakeEffect))]
    public class ObjectShakeEffectEditor : UnityEditor.Editor {
        public static readonly GUIContent Title = new GUIContent("Object Shake Effect");

        public static readonly GUIContent Subtitle =
            new GUIContent("Shakes an object using the provided parameters, useful to display damage");

        public static readonly GUIContent DimensionsContent =
            new GUIContent("Dimensions", "If a dimension is not set, it will be ignored by the shake");

        public static readonly EffectHeader EffectHeader = new EffectHeader(typeof(ObjectShakeEffect));
        private ObjectShakeEffect effect;

        private void OnEnable() {
            effect = (ObjectShakeEffect) target;
        }

        public override void OnInspectorGUI() {
            EffectHeader.DoLayout();
            GUISkin skin;
            using (var boxScope = ShiroiFXGUI.EffectBox(out skin)) {
                ShiroiFXGUI.DrawTitle(Title, Subtitle);
                ShiroiFXGUI.DrawAndApplyProperties(
                    serializedObject,
                    "UseShakeControllerIfPresent",
                    "Frequency",
                    "Intensity",
                    "Duration",
                    "Mode"
                );
                effect.Dimensions =
                    (ObjectShakeEffect.ShakeDimension) EditorGUILayout.EnumFlagsField(DimensionsContent,
                        effect.Dimensions);
                serializedObject.Update();
            }
        }
    }
}