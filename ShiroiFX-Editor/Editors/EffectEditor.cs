using System;
using System.Linq;
using Shiroi.FX.Effects;
using Shiroi.FX.Effects.Requirements;
using UnityEditor;
using UnityEngine;
using UnityUtilities.Editor;

namespace Shiroi.FX.Editor.Editors {
    [CustomEditor(typeof(Effect), true)]
    public class EffectEditor : UnityEditor.Editor {
        private Effect effect;
        private EffectHeader header;

        private void OnEnable() {
            effect = (Effect) target;
            header = new EffectHeader(effect.GetType());
        }


        public override void OnInspectorGUI() {
            header.DoLayout();
            DrawDefaultInspector();
        }
    }
}