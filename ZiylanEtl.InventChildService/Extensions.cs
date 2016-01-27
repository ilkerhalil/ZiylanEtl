using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZiylanEtl.InventChildService
{

    public static class Extenstions
    {
        public static string ToFormattedString<TKey, TValue>(this IDictionary<TKey, TValue> dic, string format,
            string separator)
        {
            return String.Join(
                !String.IsNullOrEmpty(separator) ? separator : " ",
                dic.Select(p => String.Format(
                    !String.IsNullOrEmpty(format) ? format : "{0}='{1}'",
                    p.Key, p.Value)));
        }
    }
}
