using UnityEngine;

namespace Shiroi.FX.Colors {
    /// <summary>
    /// The representation of <see cref="Color"/> as HSVA format.
    /// </summary>
    public struct ColorHSV {
        public float h;
        public float s;
        public float v;
        public float a;

        public ColorHSV(float h, float s, float v, float a) {
            this.h = h;
            this.s = s;
            this.v = v;
            this.a = a;
        }

        public static implicit operator Color(ColorHSV color) {
            var c = Color.HSVToRGB(color.h, color.s, color.v, true);
            c.a = color.a;
            return c;
        }

        public static implicit operator ColorHSV(Color color) {
            float h, s, v, a;
            Color.RGBToHSV(color, out h, out s, out v);
            a = color.a;
            return new ColorHSV(h, s, v, a);
        }
    }
}