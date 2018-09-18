using Shiroi.FX.Effects;
using UnityEditor;

namespace Shiroi.FX.Editor.Editors {
    [CustomEditor(typeof(Effect), true)]
    public class EffectEditor : UnityEditor.Editor {
        private EffectHeader header;

        private void OnEnable() {
            header = new EffectHeader(target.GetType());
        }


        public override void OnInspectorGUI() {
            header.DoLayout();
            DrawDefaultInspector();
        }
    }
}