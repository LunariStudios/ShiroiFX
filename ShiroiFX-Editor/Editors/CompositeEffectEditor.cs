using System;
using System.Collections.Generic;
using System.Linq;
using Shiroi.FX.Editor.PopUp;
using Shiroi.FX.Effects;
using Shiroi.FX.Effects.BuiltIn;
using UnityEditor;
using UnityEngine;
using Lunari.Tsuki.Editor;
using Lunari.Tsuki.Editor.Extenders;
using Lunari.Tsuki.Editor.Utilities;
using Shiroi.FX.Features;

namespace Shiroi.FX.Editor.Editors {
    [CustomEditor(typeof(CompositeEffect))]
    public class CompositeEffectEditor : UnityEditor.Editor {
        public static readonly GUIContent CompositeEffectTitle = new GUIContent(
            "Composite Effect"
        );

        public static readonly GUIContent CompositeEffectSubtitle = new GUIContent(
            "A composite effect executes each sub effect using the context that was passed into it."
        );

        private CompositeEffect effect;
        private TypeSelectorButton effectSelector;

        private void OnEnable() {
            effect = (CompositeEffect) target;
            effectSelector =
                TypeSelectorButton.Of<Effect>(new ModularContent<GUIContent>(ShiroiFXEditorResources.AddEffect),
                    OnEffectAdded);
        }

        private void OnEffectAdded(Type obj) {
            var fx = (Effect) effect.AddToAssetFile(obj);
            fx.name = ObjectNames.GetUniqueName(effect.Effects.Select(effect1 => effect1.name).ToArray(), obj.Name);
            effect.Effects.Add(fx);
            AssetDatabase.SaveAssets();
        }

        public override void OnInspectorGUI() {
            var skin = EditorGUIUtility.GetBuiltinSkin(EditorSkin.Inspector);
            using (new EditorGUILayout.VerticalScope()) {
                ShiroiFXGUI.DrawTitle(CompositeEffectTitle, CompositeEffectSubtitle);
                using (new EditorGUILayout.HorizontalScope()) {
                    EditorGUILayout.PrefixLabel("Sub Effects", EditorStyles.boldLabel);
                    effectSelector.OnInspectorGUI();
                }

                var toRemove = new List<Effect>();
                foreach (var subEffect in effect.Effects) {
                    DrawFX(subEffect, skin, out var remove);
                    if (remove) {
                        toRemove.Add(subEffect);
                    }
                }

                foreach (var fx in toRemove) {
                    effect.Effects.Remove(fx);
                    DestroyImmediate(fx, true);
                }

                if (toRemove.Count > 0) {
                    AssetDatabase.SaveAssets();
                }
            }
        }

        private void DrawFX<T>(T worldFX, GUISkin skin, out bool remove) where T : ScriptableObject {
            EditorGUILayout.BeginHorizontal();
            if (worldFX == null) {
                var errorStyle = skin.GetStyle(GUIStyles.CNStatusError);
                EditorGUILayout.LabelField(ShiroiFXEditorResources.NullFXError, errorStyle,
                    GUILayout.ExpandWidth(true));
            }
            else {
                worldFX.name = EditorGUILayout.TextField(worldFX.name);
                if (GUILayout.Button(ShiroiFXEditorResources.EditFX, GUILayout.ExpandWidth(false))) {
                    Selection.SetActiveObjectWithContext(worldFX, this);
                }
            }

            remove = GUILayout.Button(ShiroiFXEditorResources.RemoveFX, GUILayout.ExpandWidth(false));
            EditorGUILayout.EndHorizontal();
        }
    }
}