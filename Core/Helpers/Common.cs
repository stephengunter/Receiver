using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Helpers
{
	public static class CommonHelpers
	{
		public static bool HasValue(this string text) => !String.IsNullOrEmpty(text);

		public static bool EqualTo(this string val, string other) => String.Compare(val, other, true) == 0;
		
		public static int ToInt(this string str)
		{
			int value = 0;
			if (!int.TryParse(str, out value)) value = 0;

			return value;
		}

		public static bool ToBoolean(this string str)
		{
			if (String.IsNullOrEmpty(str)) return false;

			return str.ToLower() == "true";
		}

		public static double ToDouble(this string str)
		{
			double value = 0;
			if (!double.TryParse(str, out value)) value = 0;

			return value;
		}

        public static decimal ToDecimal(this string str)
        {
            decimal value = 0;
            if (!decimal.TryParse(str, out value)) value = 0;

            return value;
        }

        public static bool IsNullOrEmpty<T>(this IEnumerable<T> enumerable)
		{
			if (enumerable == null)
			{
				return true;
			}
			var collection = enumerable as ICollection<T>;
			if (collection != null)
			{
				return collection.Count < 1;
			}
			return !enumerable.Any();
		}

		public static bool HasItems<T>(this IEnumerable<T> enumerable) => !IsNullOrEmpty(enumerable);

		public static IEnumerable<T> TakeLast<T>(this IEnumerable<T> source, int N)
		{
			return source.Skip(Math.Max(0, source.Count() - N));
		}

		public static T DeepCloneByJson<T>(this T source)
		{
			if (Object.ReferenceEquals(source, null))
			{
				return default(T);
			}
			var deserializeSettings = new JsonSerializerSettings { ObjectCreationHandling = ObjectCreationHandling.Replace };
			return JsonConvert.DeserializeObject<T>(JsonConvert.SerializeObject(source), deserializeSettings);
		}
	}
}
