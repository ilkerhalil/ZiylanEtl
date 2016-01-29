using System.Collections.Generic;
using System.Linq;

namespace ZiylanEtl.Abstraction.Helper
{

    public static class Extenstions
    {
        public static string ToFormattedString<TKey, TValue>(this IDictionary<TKey, TValue> dic, string format,
            string separator)
        {
            return string.Join(
                !string.IsNullOrEmpty(separator) ? separator : " ",
                dic.Select(p => string.Format(
                    !string.IsNullOrEmpty(format) ? format : "{0}='{1}'",
                    p.Key, p.Value)));
        }

        public static bool IsNullAndWhiteSpace(this string parameter)
        {
            return string.IsNullOrWhiteSpace(parameter);
        }
    }
}
