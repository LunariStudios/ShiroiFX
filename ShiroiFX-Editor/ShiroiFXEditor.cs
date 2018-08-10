using System.IO;
using Shiroi.FX.Effects;
using UnityEditor;
using UnityEngine;

namespace Shiroi.FX.Editor {
    public static class ShiroiFXEditor {
        [MenuItem(ShiroiFXEditorResources.BaseCreatePath + "Create Composite Effect", false, 5)]
        public static void CreateCompositeEffect() {
            var effect = ScriptableObject.CreateInstance<CompositeEffect>();
            var path = AssetDatabase.GetAssetPath(Selection.activeObject);
            if (path == "") {
                path = "Assets";
            } else if (Path.GetExtension(path) != "") {
                path = path.Replace(Path.GetFileName(AssetDatabase.GetAssetPath(Selection.activeObject)), "");
            }

            var assetPathAndName = AssetDatabase.GenerateUniqueAssetPath(path + "/New Composite Effect.asset");
            AssetDatabase.CreateAsset(effect, assetPathAndName);
            AssetDatabase.SaveAssets();
        }
    }
}