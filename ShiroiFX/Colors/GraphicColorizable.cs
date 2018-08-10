using UnityEngine;
using UnityEngine.UI;

namespace Shiroi.FX.Colors {
    public class GraphicColorizable : Colorizable {
        [SerializeField]
        private Color defaultColor = Color.white;

        public Graphic Graphic;

        public override Color DefaultColor {
            get {
                return defaultColor;
            }
            set {
                defaultColor = value;
            }
        }

        public override Color Color {
            get {
                return Graphic.color;
            }
            set {
                Graphic.color = value;
            }
        }
    }
}