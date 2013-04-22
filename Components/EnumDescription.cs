using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Reflection;
using System.ComponentModel;
namespace WebApp4.Components
{
    public class EnumDescription<T>
    {
        static EnumDescription()
        {
            Cacheabled<T, Dictionary<T, string>>.OnLoaded += Init;
        }

        public static Dictionary<T, string> Current
        {
            get
            {
                return Cacheabled<T, Dictionary<T, string>>.Current;
            }
        }

        private static Dictionary<T, string> Init()
        {
            var type = typeof(T);
            if (!type.IsEnum)
                throw new ApplicationException(type + "不是有效的枚举类型");

            var result = new Dictionary<T, string>();
            var fields = type.GetFields(BindingFlags.Public | BindingFlags.Static);

            foreach (var field in fields)
            {
                var attributes = (DescriptionAttribute[])field.GetCustomAttributes(typeof(DescriptionAttribute), false);
                var description = (attributes.Length > 0) ? attributes[0].Description : field.Name;
                result.Add((T)field.GetValue(null), description);
            }
            return result;
        }

        public static string Description(T value)
        {
            var dic = Current;
            string result;
            dic.TryGetValue(value, out result);

            if (String.IsNullOrWhiteSpace(result))
                result = value.ToString();

            return result;
        }

        public static string CombineDescricption(int value)
        {
            var type = typeof(T);
            if (!type.IsEnum)
                throw new ApplicationException(type + "不是有效的枚举类型");

            var result = new Dictionary<int, string>();
            var reResult = new Dictionary<int, string>();
            var fields = type.GetFields(BindingFlags.Public | BindingFlags.Static);

            foreach (var field in fields)
            {
                var attributes = (DescriptionAttribute[])field.GetCustomAttributes(typeof(DescriptionAttribute), false);
                var description = (attributes.Length > 0) ? attributes[0].Description : field.Name;
                result.Add(Convert.ToInt32(field.GetValue(null)), description);
            }

            foreach (var item in result)
            {
                if ((item.Key & value) == item.Key)
                    reResult.Add(item.Key, item.Value);
            }
            if (reResult.Count > 0)
            {
                var reFirst = reResult.OrderByDescending(e => e.Key).First();
                return reFirst.Key == value ? reFirst.Value : string.Join(",", reResult.Select(e => e.Value).ToArray());
            }
            else return string.Join(",", reResult.Select(e => e.Value).ToArray());
        }
    }
}