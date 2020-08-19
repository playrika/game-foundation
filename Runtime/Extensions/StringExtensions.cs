using System.Linq;

namespace Playrika.GameFoundation.Extensions
{
    public static class StringExtensions
    {
        public static string ToFirstUpperChar(this string value)
        {
            return value.First().ToString().ToUpper() + value.Substring(1).ToLower();
        }
    }
}