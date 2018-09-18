using System;

namespace Shiroi.FX.Utilities {
    [AttributeUsage(AttributeTargets.Class)]
    public class IconAttribute : Attribute {
        public IconAttribute(string icon) {
            Icon = icon;
        }

        public string Icon {
            get;
            private set;
        }
    }

    public static class Icons {
        public const string TimeIcon = "Shiroi.FX.Editor.Icons.TimeIcon.png";
        public const string ListIcon = "Shiroi.FX.Editor.Icons.ListIcon.png";
        public const string AudioIcon = "Shiroi.FX.Editor.Icons.AudioIcon.png";
        public const string ShakeIcon = "Shiroi.FX.Editor.Icons.ShakeIcon.png";
        public const string DefaultIcon = "Shiroi.FX.Editor.Icons.DefaultIcon.png";
        public const string ParticleIcon = "Shiroi.FX.Editor.Icons.ParticleIcon.png";
        public const string FOVIcon = "Shiroi.FX.Editor.Icons.FOVIcon.png";
    }
}