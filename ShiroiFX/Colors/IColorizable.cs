using UnityEngine;

namespace Shiroi.FX.Colors {
    public abstract class Colorizable {
        public abstract Color DefaultColor {
            get;
            set;
        }

        public abstract Color Color {
            get;
            set;
        }
    }
}