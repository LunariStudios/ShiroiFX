﻿using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Shiroi.FX.Utilities {
    [Serializable]
    public struct Range {
        [SerializeField]
        private float start;

        [SerializeField]
        private float end;

        public Range(float start, float end) : this() {
            this.start = start;
            this.end = end;
        }

        public float Start {
            get {
                return start;
            }
            set {
                start = Mathf.Min(value, end);
                end = Mathf.Max(start, value);
            }
        }

        public float End {
            get {
                return end;
            }
            set {
                end = Mathf.Max(start, value);
                start = Mathf.Min(value, end);
            }
        }

        public float GetValueWithinRange() {
            return Random.Range(start, end);
        }
    }
}