using System;
using Shiroi.FX.Editor.PopUp;
using Shiroi.FX.Effects;
using UnityEditor;
using UnityEngine;
using UnityUtilities.Editor;

namespace Shiroi.FX.Editor {
    [CustomEditor(typeof(CompositeEffect))]
    public class CompositeEffectEditor : UnityEditor.Editor {
        private CompositeEffect effect;
        private EffectSelectorContent<GameEffect> gameEffectSelector;
        private EffectSelectorContent<WorldEffect> worldEffectSelector;

        private void OnEnable() {
            effect = (CompositeEffect) target;
            gameEffectSelector = new EffectSelectorContent<GameEffect>(OnGameEffectAdded);
            worldEffectSelector = new EffectSelectorContent<WorldEffect>(OnWorldEffectAdded);
        }

        private void OnWorldEffectAdded(Type obj) {
            effect.Effects.Add((WorldEffect) effect.AddToAssetFile(obj));
        }

        private void OnGameEffectAdded(Type obj) {
            effect.GameEffects.Add((GameEffect) effect.AddToAssetFile(obj));
        }

        public override void OnInspectorGUI() {
            var skin = EditorGUIUtility.GetBuiltinSkin(EditorSkin.Inspector);
            Rect worldRect;
            if (DrawEffectListHeader(ShiroiFXEditorResources.WorldFXHeader, skin, out worldRect)) {
                PopupWindow.Show(worldRect, worldEffectSelector);
            }

            effect.Effects.RemoveAll(
                gameEffect => {
                    bool result;
                    DrawFX(gameEffect, skin, out result);
                    return result;
                });

            Rect gameRect;
            if (DrawEffectListHeader(ShiroiFXEditorResources.GameFXHeader, skin, out gameRect)) {
                PopupWindow.Show(gameRect, gameEffectSelector);
            }

            effect.GameEffects.RemoveAll(
                gameEffect => {
                    bool result;
                    DrawFX(gameEffect, skin, out result);
                    return result;
                }
            );
        }

        private static bool DrawEffectListHeader(GUIContent label, GUISkin skin, out Rect buttonRect) {
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.PrefixLabel(label, EditorStyles.boldLabel);
            var r = GUILayout.Button(ShiroiFXEditorResources.AddEffect);
            buttonRect = Event.current.type == EventType.Repaint ? GUILayoutUtility.GetLastRect() : default(Rect);

            EditorGUILayout.EndHorizontal();
            return r;
        }


        private void DrawFX<T>(T worldFX, GUISkin skin, out bool remove) where T : ScriptableObject {
            var nullFx = worldFX == null;
            var errorStyle = skin.GetStyle(GUISkinProperties.CNStatusError);
            EditorGUILayout.BeginHorizontal(nullFx ? errorStyle : skin.box);
            if (nullFx) {
                EditorGUILayout.LabelField(ShiroiFXEditorResources.NullFXError, errorStyle);
            } else {
                worldFX.name = EditorGUILayout.TextField(ShiroiFXEditorResources.FXName, worldFX.name);
                if (GUILayout.Button(ShiroiFXEditorResources.EditFX)) {
                    Selection.SetActiveObjectWithContext(worldFX, this);
                }
            }

            remove = GUILayout.Button(ShiroiFXEditorResources.RemoveFX);

            EditorGUILayout.EndHorizontal();
        }
    }
}