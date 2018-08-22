using System;

namespace Shiroi.FX.Effects.Requirements {
    public class RequiresFeatureAttribute : Attribute {
        public Type[] FeatureTypes {
            get;
            set;
        }

        public RequiresFeatureAttribute(params Type[] featureTypes) {
            FeatureTypes = featureTypes;
        }
    }

    public class OptinalFeatureAttribute : Attribute {
        public Type[] FeatureTypes {
            get;
            set;
        }

        public OptinalFeatureAttribute(params Type[] featureTypes) {
            FeatureTypes = featureTypes;
        }
    }
}