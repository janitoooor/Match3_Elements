using System;
using System.Collections.Generic;
using System.Linq;

namespace Base
{
	public static class CacheExtensions
	{
		public static TValue GetOrAddFromArray<TKey, TValue, TSource>(
			this Dictionary<TKey, TValue> cache,
			TKey key,
			TSource[] sourceArray,
			Func<TSource, TKey> keySelector,
			Func<TSource, TValue> valueSelector)
			where TValue : class
		{
			if (cache.TryGetValue(key, out var cachedValue))
				return cachedValue;

			var sourceItem = sourceArray.FirstOrDefault(item => 
				EqualityComparer<TKey>.Default.Equals(keySelector(item), key));
        
			if (sourceItem == null)
				return null;

			var value = valueSelector(sourceItem);
			cache[key] = value;
			return value;
		}
	}
}