using System.Linq;
using Shiroi.FX.Effects;
using Shiroi.FX.Effects.BuiltIn;
using Shiroi.FX.Utilities;
using UnityEditor;
using UnityEngine;
using Lunari.Tsuki;

namespace Shiroi.FX.Editor {
    public class ShiroiFXIconHandler : AssetPostprocessor {
        [InitializeOnLoadMethod]
        private static void OnLoaded() {
            foreach (var effectAssets in AssetDatabase.FindAssets("t:Shiroi.FX.Effects.Effect")) {
                foreach (var asset in AssetDatabase.LoadAllAssetsAtPath(AssetDatabase.GUIDToAssetPath(effectAssets))) {
                    ProcessObject(asset);
                }
            }
        }

        private static void OnPostprocessAllAssets(string[] importedAssets, string[] deletedAssets,
            string[] movedAssets,
            string[] movedFromAssetPaths) {
            foreach (var importedAsset in importedAssets) {
                foreach (var o in AssetDatabase.LoadAllAssetsAtPath(importedAsset)) {
                    ProcessObject(o);
                }
            }
        }

        private static void ProcessEffect(Effect e) {
            var type = e.GetType();
            var at = type.GetCustomAttributes(typeof(IconAttribute), false);
            var icName = Icons.DefaultIcon;
            if (!at.IsNullOrEmpty()) {
                icName = ((IconAttribute) at.First()).Icon;
            }

            
            e.SetIcon(icName);
            var cE = e as CompositeEffect;
            if (cE != null) {
                foreach (var subEffect in cE.Effects) {
                    ProcessEffect(subEffect);
                }
            }
        }

        private static void ProcessObject(Object o) {
            var e = o as Effect;
            if (e == null) {
                return;
            }

            ProcessEffect(e);
        }
    }
}