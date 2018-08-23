using System;
using UnityEngine;

namespace Shiroi.FX.Utilities {
    [Serializable]
    public struct ModularRange {
        public enum RangeMode {
            Constant,
            RandomBetweenConstants
        }

        public RangeMode Mode;
        public Range Range;
        public float Constant;

        public static implicit operator ModularRange(float constant) {
            return new ModularRange {
                Constant = constant,
                Mode = RangeMode.Constant
            };
        }


        public float Evaluate() {
            switch (Mode) {
                case RangeMode.Constant:
                    return Constant;
                case RangeMode.RandomBetweenConstants:
                    return Range.GetValueWithinRange();
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}