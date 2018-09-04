using System;

namespace Shiroi.FX.Editor.Utilities {
    public static class TypeUtility {
        public static string GetFriendlyName(this Type type) {
            var friendlyName = type.Name;
            if (!type.IsGenericType) {
                return friendlyName;
            }

            var iBacktick = friendlyName.IndexOf('`');
            if (iBacktick > 0) {
                friendlyName = friendlyName.Remove(iBacktick);
            }

            friendlyName += "<";
            var typeParameters = type.GetGenericArguments();
            for (var i = 0; i < typeParameters.Length; ++i) {
                var typeParamName = GetFriendlyName(typeParameters[i]);
                friendlyName += (i == 0 ? typeParamName : "," + typeParamName);
            }

            friendlyName += ">";

            return friendlyName;
        }
    }
}