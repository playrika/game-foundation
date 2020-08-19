using System;

namespace Playrika.GameFoundation.Helpers
{
    public static class TypeHelper
    {
        public static bool IsPrimitiveOrString(Type type)
        {
            return type.IsPrimitive || type == typeof(string);
        }
    }
}