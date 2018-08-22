using System;
using System.IO;
using System.Linq;
using Shiroi.FX.Effects;
using Shiroi.FX.Effects.BuiltIn;
using UnityEditor;
using UnityEngine;
using UnityUtilities.Editor;

namespace Shiroi.FX.Editor {
    public static class ShiroiFXEditor {
        [MenuItem(ShiroiFXEditorResources.BaseCreatePath + "Create Composite Effect", false, 5)]
        public static void CreateCompositeEffect() {
            CreateEffect<CompositeEffect>();
        }

        [MenuItem(ShiroiFXEditorResources.BaseCreatePath + "Create Particle Effect", false, 5)]
        public static void CreateParticleEffect() {
            CreateEffect<ParticleEffect>();
        }

        [MenuItem(ShiroiFXEditorResources.BaseCreatePath + "Create Freeze Frame Effect", false, 5)]
        public static void CreateFreezeFrameEffect() {
            CreateEffect<FreezeFrameEffect>();
        }

        private static void CreateEffect<T>() where T : ScriptableObject {
            var effect = ScriptableObject.CreateInstance<T>();
            var path = AssetDatabase.GetAssetPath(Selection.activeObject);
            if (path == "") {
                path = "Assets";
            } else if (Path.GetExtension(path) != "") {
                path = path.Replace(Path.GetFileName(AssetDatabase.GetAssetPath(Selection.activeObject)), "");
            }

            var assetPathAndName =
                AssetDatabase.GenerateUniqueAssetPath(string.Format("{0}/{1}.asset", path, typeof(T).Name));
            AssetDatabase.CreateAsset(effect, assetPathAndName);
            AssetDatabase.SaveAssets();
        }

        public static void DrawTitle(GUIContent title, GUIContent subtitle) {
            GUILayout.Space(5);
            EditorGUILayout.LabelField(title, EditorStyles.boldLabel);
            EditorGUILayout.LabelField(subtitle, ShiroiFXEditorResources.Subtitle);
            var rect = GUILayoutUtility.GetRect(10, 200, 1, 1, GUILayout.ExpandWidth(true));
            var oldColor = GUI.color;
            GUI.color = ShiroiFXEditorResources.TitleLineColor;
            GUI.DrawTexture(rect, EditorGUIUtility.whiteTexture);
            GUI.color = oldColor;
            GUILayout.Space(5);
        }
    }
}