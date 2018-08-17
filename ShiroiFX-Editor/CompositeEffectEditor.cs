using System;
using System.Linq;
using Shiroi.FX.Editor.PopUp;
using Shiroi.FX.Effects;
using Shiroi.FX.Effects.BuiltIn;
using UnityEditor;
using UnityEngine;
using UnityUtilities.Editor;

namespace Shiroi.FX.Editor {
    [CustomEditor(typeof(CompositeEffect))]
    public class CompositeEffectEditor : UnityEditor.Editor {
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
        }

        public override void OnInspectorGUI() {
            var skin = EditorGUIUtility.GetBuiltinSkin(EditorSkin.Inspector);
            EditorGUILayout.BeginVertical(skin.box);
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.PrefixLabel("Sub Effects", EditorStyles.boldLabel);
            var r = GUILayout.Button(ShiroiFXEditorResources.AddEffect);
            var buttonRect = Event.current.type == EventType.Repaint ? GUILayoutUtility.GetLastRect() : default(Rect);

            if (r) {
                PopupWindow.Show(buttonRect, effectSelector);
            }

            EditorGUILayout.EndHorizontal();

            effect.Effects.RemoveAll(
                gameEffect => {
                    bool result;
                    DrawFX(gameEffect, skin, out result);
                    return result;
                });
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