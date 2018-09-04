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
        [MenuItem(ShiroiFXEditorResources.BaseCreatePath + "Composite Effect", false, 5)]
        public static void CreateCompositeEffect() {
            CreateEffect<CompositeEffect>();
        }

        [MenuItem(ShiroiFXEditorResources.BaseCreatePath + "Particle Effect", false, 5)]
        public static void CreateParticleEffect() {
            CreateEffect<ParticleEffect>();
        }

        [MenuItem(ShiroiFXEditorResources.BaseCreatePath + "Freeze Frame Effect", false, 5)]
        public static void CreateFreezeFrameEffect() {
            CreateEffect<FreezeFrameEffect>();
        }


        [MenuItem(ShiroiFXEditorResources.BaseCreatePath + "Audio Effect", false, 5)]
        public static void CreateAudioEffect() {
            CreateEffect<AudioEffect>();
        }

        [MenuItem(ShiroiFXEditorResources.BaseCreatePath + "Field Of View Effect", false, 5)]
        public static void CreateFOVEffect() {
            CreateEffect<FOVEffect>();
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
    }
}