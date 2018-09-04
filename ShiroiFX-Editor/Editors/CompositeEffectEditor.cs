using System;
using System.Collections.Generic;
using System.Linq;
using Shiroi.FX.Editor.PopUp;
using Shiroi.FX.Effects;
using Shiroi.FX.Effects.BuiltIn;
using UnityEditor;
using UnityEngine;
using UnityUtilities.Editor;

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
        private EffectSelectorContent effectSelector;

        private void OnEnable() {
            effect = (CompositeEffect) target;
            effectSelector = new EffectSelectorContent(OnEffectAdded);
        }

        private void OnEffectAdded(Type obj) {
            var fx = (Effect) effect.AddToAssetFile(obj);
            fx.name = ObjectNames.GetUniqueName(effect.Effects.Select(effect1 => effect1.name).ToArray(), obj.Name);
            effect.Effects.Add(fx);
            AssetDatabase.SaveAssets();
        }

        public override void OnInspectorGUI() {
            var skin = EditorGUIUtility.GetBuiltinSkin(EditorSkin.Inspector);
            EditorGUILayout.BeginVertical(skin.box);
            ShiroiFXGUI.DrawTitle(CompositeEffectTitle, CompositeEffectSubtitle);
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.PrefixLabel("Sub Effects", EditorStyles.boldLabel);
            var r = GUILayout.Button(ShiroiFXEditorResources.AddEffect);
            var buttonRect = Event.current.type == EventType.Repaint ? GUILayoutUtility.GetLastRect() : default(Rect);

            if (r) {
                PopupWindow.Show(buttonRect, effectSelector);
            }

            EditorGUILayout.EndHorizontal();
            var toRemove = new List<Effect>();
            foreach (var subEffect in effect.Effects) {
                bool remove;
                DrawFX(subEffect, skin, out remove);
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

            EditorGUILayout.EndVertical();
        }

        private void DrawFX<T>(T worldFX, GUISkin skin, out bool remove) where T : ScriptableObject {
            EditorGUILayout.BeginHorizontal();
            if (worldFX == null) {
                var errorStyle = skin.GetStyle(GUISkinProperties.CNStatusError);
                EditorGUILayout.LabelField(ShiroiFXEditorResources.NullFXError, errorStyle,
                    GUILayout.ExpandWidth(true));
            } else {
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