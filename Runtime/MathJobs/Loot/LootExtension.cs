// using System.Collections.Generic;
//
// namespace com.ktgame.manager.job.math.loot
// {
// 	public static class LootExtension
// 	{
// 		public static List<T> Clone<T>(this IEnumerable<BaseLootData<T>> target) where T : BaseLootData
// 		{
// 			var result = new List<T>();
// 			if (target == null)
// 			{
// 				return result;
// 			}
//
// 			foreach (var item in target)
// 			{
// 				result.Add(item.Clone());
// 			}
//
// 			return result;
// 		}
//
// 		public static List<LootData> Clone(this IEnumerable<LootData> target)
// 		{
// 			var result = new List<LootData>();
// 			if (target == null)
// 			{
// 				return result;
// 			}
//
// 			foreach (var item in target)
// 			{
// 				result.Add(item.Clone());
// 			}
//
// 			return result;
// 		}
//
// 		public static List<T> Merge<T>(this IEnumerable<T> target) where T : BaseLootData
// 		{
// 			var result = new List<T>();
// 			if (target == null)
// 			{
// 				return result;
// 			}
//
// 			foreach (var item in target)
// 			{
// 				var same = result.Find(x => x.Same(item));
// 				if (same != default)
// 				{
// 					if (item.Value >= 0)
// 					{
// 						same.Increase(item.Value);
// 					}
// 					else
// 					{
// 						same.Subtract(-item.Value, false);
// 					}
// 				}
// 				else
// 				{
// 					result.Add((T)item.Clone());
// 				}
// 			}
//
// 			return result;
// 		}
//
// 		public static List<LootData> Extract(this IEnumerable<LootData> target)
// 		{
// 			var result = new List<LootData>();
// 			if (target == null)
// 			{
// 				return result;
// 			}
//
// 			foreach (var item in target)
// 			{
// 				result.AddRange(item.Extract());
// 			}
//
// 			return result;
// 		}
//
// 		public static void Multiply<T>(this IEnumerable<T> list, double value) where T : BaseLootData
// 		{
// 			if (list == null || value.Equals(1))
// 			{
// 				return;
// 			}
//
// 			foreach (var item in list)
// 			{
// 				item.Multiply(value);
// 			}
// 		}
//
// 		public static List<LootData> ToLootData<T>(this IEnumerable<T> list) where T : BaseLootData
// 		{
// 			var result = new List<LootData>();
// 			if (list == null)
// 			{
// 				return result;
// 			}
//
// 			foreach (var item in list)
// 			{
// 				var loot = item.ToLootData();
// 				if (loot != null)
// 				{
// 					result.Add(loot);
// 				}
// 			}
//
// 			return result;
// 		}
//
// 		public static List<T> ToBaseData<T>(this IEnumerable<LootData> list) where T : BaseLootData
// 		{
// 			var result = new List<T>();
// 			if (list == null)
// 			{
// 				return result;
// 			}
//
// 			foreach (var item in list)
// 			{
// 				var loot = item.ToBaseData<T>();
// 				if (loot != null)
// 				{
// 					result.Add(loot);
// 				}
// 			}
//
// 			return result;
// 		}
// 	}
// }
