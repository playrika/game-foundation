using System;
using System.Collections.Generic;

namespace Playrika.GameFoundation.Helpers
{
    public static class EnumHelper
    {
        public static List<T> GetValues<T>() where T : Enum
        {
            var objects = Enum.GetValues(typeof(T));
            var values = new List<T>(objects.Length);

            for (var i = 0; i < objects.Length; i++)
                values.Add((T) objects.GetValue(i));

            return values;
        }
    }
}