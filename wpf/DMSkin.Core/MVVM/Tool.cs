using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMSkin.Core.MVVM
{
    public static class Tool
    {
        public static Dictionary<string, string> ToDictionary<T>(this T model)
        {
            var dictionary = new Dictionary<string, string>();

            var properties = typeof(T).GetProperties();

            foreach (var property in properties)
            {
                var value = property.GetValue(model);
                var stringValue = value != null ? value.ToString() : null;
                dictionary.Add(property.Name, stringValue);
            }

            return dictionary;
        }

    }
}
