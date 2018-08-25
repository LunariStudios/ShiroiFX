using System;

namespace Shiroi.FX.Effects.Requirements {
    [AttributeUsage(AttributeTargets.Class)]
    public class RequiresFeatureAttribute : Attribute {
        public Type[] FeatureTypes {
            get;
            set;
        }

        public int TotalRequiredFeatures {
            get {
                return FeatureTypes.Length;
            }
        }

        public RequiresFeatureAttribute(params Type[] featureTypes) {
            FeatureTypes = featureTypes;
        }
    }

    [AttributeUsage(AttributeTargets.Class)]
    public class OptinalFeatureAttribute : Attribute {
        public Type[] FeatureTypes {
            get;
            set;
        }

        public OptinalFeatureAttribute(params Type[] featureTypes) {
            FeatureTypes = featureTypes;
        }
    }

    [AttributeUsage(AttributeTargets.Class)]
    public sealed class RequirementsDescription : Attribute {
        private readonly string[] descriptions;

        public RequirementsDescription(params string[] descriptions) {
            this.descriptions = descriptions;
        }

        public string[] Descriptions {
            get {
                return descriptions;
            }
        }
    }
}