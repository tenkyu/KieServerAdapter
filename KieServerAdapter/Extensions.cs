using System;
using System.Linq;
using System.Reflection;

namespace KieServerAdapter
{
    internal static class Extensions
    {
        public static TValue GetAttributeValue<TAttribute, TValue>(
            this Type type,
            Func<TAttribute, TValue> valueSelector)
            where TAttribute : Attribute
        {
            var att = type.GetCustomAttributes(
                typeof(TAttribute), true
            ).FirstOrDefault() as TAttribute;
            if (att != null)
            {
                return valueSelector(att);
            }
            return default(TValue);
        }

        public static T ForceType<T>(this object o)
        {
            T res;
            res = Activator.CreateInstance<T>();

            Type x = o.GetType();
            Type y = res.GetType();

            foreach (var destinationProp in y.GetProperties(BindingFlags.Public | BindingFlags.Instance))
            {
                var sourceProp = x.GetProperty(destinationProp.Name);
                if (sourceProp != null)
                {
                    if (destinationProp.CanWrite)
                        destinationProp.SetValue(res, sourceProp.GetValue(o));
                }
            }

            return res;
        }
    }
}
