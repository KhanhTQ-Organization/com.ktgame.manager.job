using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Unity.Collections;
using UnityEngine;

namespace com.ktgame.manager.job.math.math
{
	public static class MathExtension
	{
#region Rate Array
		public static float[] ToRateArray(this float[] input) => ToRateArrayLogic(input);
		public static List<float> ToRateArray(this List<float> input) => ToRateArrayLogic(input);
		public static int[] ToRateArray(this int[] input) => ToRateArrayLogic(input);
		public static List<int> ToRateArray(this List<int> input) => ToRateArrayLogic(input);

		public static int RandomIndexRateArray(this List<float> rate)
		{
			if (rate == null || rate.Count == 0)
			{
				return -1;
			}

			float random = Random(0, rate[^1]);

			for (int i = 0; i < rate.Count; i++)
			{
				if (rate[i] >= random)
				{
					return i;
				}
			}

			return 0;
		}

		public static int RandomIndexRateArray(this IList<int> rate)
		{
			if (rate == null || rate.Count == 0)
			{
				return -1;
			}

			float random = Random(0, rate[^1]);
			for (int i = 0; i < rate.Count; i++)
			{
				if (rate[i] >= random)
				{
					return i;
				}
			}

			return 0;
		}

		public static int RandomIndexRate(this List<float> list) => list.ToRateArray().RandomIndexRateArray();
		public static int RandomIndexRate(this List<int> list) => list.ToRateArray().RandomIndexRateArray();
#endregion

#region Sum
		public static float Sum(NativeArray<float> input, NativeArray<float> output) =>
			SumLogic(input, output);

		public static double Sum(NativeArray<double> input, NativeArray<double> output) =>
			SumLogic(input, output);

		public static int Sum(NativeArray<int> input, NativeArray<int> output) => SumLogic(input, output);
		public static float Sum(this float[] input) => SumLogic(input);
		public static float Sum(this IEnumerable<float> input) => SumLogic(input);
		public static double Sum(this double[] input) => SumLogic(input);
		public static double Sum(this IEnumerable<double> input) => SumLogic(input);
		public static int Sum(this int[] input) => SumLogic(input);
		public static int Sum(this IEnumerable<int> input) => SumLogic(input);
#endregion

#region Random
		public static int Random(int from, int to) => RandomLogic(from, to);
		public static float Random(float from, float to) => RandomLogic(from, to);
		public static double Random(double from, double to) => RandomLogic(from, to);
		public static int RandomTo(this int from, int to) => Random(from, to);
		public static float RandomTo(this float from, float to) => Random(from, to);
		public static double RandomTo(this double from, double to) => Random(from, to);
		public static int Random(this Vector2Int vector) => Random(vector.x, vector.y);
		public static float Random(this Vector2 vector) => Random(vector.x, vector.y);
		public static bool RandomBool() => RandomLogic(0, 2) == 0;
		public static bool RandomInRange(this float range) => Random(0f, 1f) <= range;
		public static bool RandomOutRange(this float range) => !RandomInRange(range);
		public static bool RandomInRange(this double range) => Random(0f, 1f) <= range;
		public static bool RandomOutRange(this double range) => !RandomInRange(range);

		public static T Random<T>(this T defaultValue) where T : struct, IConvertible
		{
			if (typeof(T).IsEnum)
			{
				var values = Enum.GetValues(typeof(T));
				return (T)values.GetValue(Random(0, values.Length - 1));
			}

			return defaultValue;
		}
#endregion

#region Random In List
		public static T Random<T>(this T[] input) =>
			input == null || input.Length == 0 ? default : input[input.RandomIndex()];

		public static T Random<T>(this IList<T> input) =>
			input == null || input.Count == 0 ? default : input[input.RandomIndex()];

		public static T Random<T>(this ICollection<T> input) =>
			input == null || input.Count == 0 ? default : input.ElementAt(input.RandomIndex());

		public static T Random<T>(this IEnumerable<T> input)
		{
			var enumerable = input as T[] ?? input.ToArray();
			return !enumerable.Any() ? default : enumerable.ElementAt(enumerable.RandomIndex());
		}
		
		public static T GetRandomValueSatisfyingCondition<T>(this IEnumerable<T> inputList, Func<T, bool> conditionFunc)
		{
			var filteredList = inputList.Where(conditionFunc).ToList();
			if (filteredList.Count > 0)
			{
				return filteredList[Random(0, filteredList.Count - 1)];
			}
			else
			{
				// Trả về giá trị mặc định của kiểu T nếu không tìm thấy giá trị thỏa mãn điều kiện
				return default(T);
			}
		}
#endregion

#region Random Index In List
		public static int RandomIndex<T>(this T[] array) => Random(0, array.Length - 1);
		public static int RandomIndex<T>(this IList<T> list) => Random(0, list.Count - 1);
		public static int RandomIndex<T>(this ICollection<T> collection) => Random(0, collection.Count - 1);
		public static int RandomIndex<T>(this IEnumerable<T> enumerable) => Random(0, enumerable.Count() - 1);
#endregion

#region Shuffle And Swap
		public static void Shuffle<T>(this IList<T> list)
		{
			if (list is not { Count: > 1 })
			{
				return;
			}

			for (int i = 0; i < list.Count - 1; i++)
			{
				int random = Random(i, list.Count - 1);
				list.Swap(i, random);
			}
		}

		public static void Swap<T>(this IList<T> array, int a, int b)
		{
			(array[a], array[b]) = (array[b], array[a]);
		}
#endregion

		public static bool Compare(this double a, double b) => a >= b;

		public static double Clamp(this double value, double min, double max)
		{
			if (value > max)
			{
				return max;
			}

			return value < min ? min : value;
		}

		public static double Clamp(this double value, Vector2 vector) => value.Clamp(vector.x, vector.y);
		public static double Max(this double value, double target) => value.Compare(target) ? value : target;
		public static double Min(this double value, double target) => !value.Compare(target) ? value : target;

		public static float ToFloat(this double value) => Math.Min((float)value, float.MaxValue);
		public static int ToInt(this double value) => value > int.MaxValue ? int.MaxValue : Convert.ToInt32(value);
		public static double ToDouble(this double value) => Math.Min(value, 1E+308);

		public static readonly string[] Scores = new[]
		{
			"", "k", "M", "B", "T",
			"aa", "ab", "ac", "ad", "ae", "af", "ag", "ah", "ai", "aj", "ak", "al", "am", "an", "ao", "ap", "aq", "ar",
			"as", "at", "au", "av", "aw", "ax", "ay", "az",
			"ba", "bb", "bc", "bd", "be", "bf", "bg", "bh", "bi", "bj", "bk", "bl", "bm", "bn", "bo", "bp", "bq", "br",
			"bs", "bt", "bu", "bv", "bw", "bx", "by", "bz",
			"ca", "cb", "cc", "cd", "ce", "cf", "cg", "ch", "ci", "cj", "ck", "cl", "cm", "cn", "co", "cp", "cq", "cr",
			"cs", "ct", "cu", "cv", "cw", "cx", "cy", "cz",
			"da", "db", "dc", "dd", "de", "df", "dg", "dh", "di", "dj", "dk", "dl", "dm", "dn", "do", "dp", "dq", "dr",
			"ds", "dt", "du", "dv", "dw", "dx", "dy", "dz",
		};

		public static string Show(this long value) => ShowLogic(value);
		public static string Show(this ulong value) => ShowLogic(value);
		public static string Show(this int value) => ShowLogic(value);
		public static string Show(this uint value) => ShowLogic(value);
		public static string Show(this float value) => ShowLogic(value);
		public static string Show(this double value) => ShowLogic(value);

		public static string Show(this TimeSpan time, bool isMilliseconds = false)
		{
			if (time.TotalMilliseconds <= 0)
			{
				return "0s";
			}

			StringBuilder str = new StringBuilder();

			var day = time.Days;
			var hour = time.Hours;

			if (time.TotalHours <= 48)
			{
				hour = (int)time.TotalHours;
			}
			else
			{
				str.Append($"{day}d");
			}

			if (hour > 0)
			{
				str.Append($" {hour}h");
			}

			if (time.Minutes > 0)
			{
				str.Append($" {time.Minutes}m");
			}

			if (time.Hours == 0)
			{
				if (isMilliseconds)
				{
					if (time.Seconds > 0)
					{
						str.Append($" {time.Seconds}");
					}
					else if (time.Milliseconds > 0)
					{
						str.Append(" 0");
					}

					int mils = time.Milliseconds / 100;
					if (time.Minutes == 0 && mils > 0)
					{
						str.Append($".{mils}");
					}

					if (time.Seconds > 0 || mils > 0)
					{
						str.Append("s");
					}
				}
				else
				{
					str.Append($" {time.Seconds}s");
				}
			}

			var rs = str.ToString();
			if (rs.StartsWith(' '))
			{
				rs = rs[1..];
			}

			return rs;
		}

		public static int ParseInt(this string value)
		{
			var result = 0;
			foreach (var letter in value)
			{
				result = 10 * result + letter.ParseInt();
			}

			return result;
		}

		public static int ParseInt(this char value)
		{
			return value - 48;
		}
	
#region Burst Logic Replacements
        private static Unity.Mathematics.Random _rnd = new Unity.Mathematics.Random((uint)UnityEngine.Random.Range(1, 100000));

        public static float[] ToRateArrayLogic(float[] input) {
            float[] output = new float[input.Length];
            float sum = 0;
            for(int i=0; i<input.Length; i++) { sum += input[i]; output[i] = sum; }
            return output;
        }
        public static System.Collections.Generic.List<float> ToRateArrayLogic(System.Collections.Generic.List<float> input) {
            var output = new System.Collections.Generic.List<float>(input.Count);
            float sum = 0;
            for(int i=0; i<input.Count; i++) { sum += input[i]; output.Add(sum); }
            return output;
        }
        public static int[] ToRateArrayLogic(int[] input) {
            int[] output = new int[input.Length];
            int sum = 0;
            for(int i=0; i<input.Length; i++) { sum += input[i]; output[i] = sum; }
            return output;
        }
        public static System.Collections.Generic.List<int> ToRateArrayLogic(System.Collections.Generic.List<int> input) {
            var output = new System.Collections.Generic.List<int>(input.Count);
            int sum = 0;
            for(int i=0; i<input.Count; i++) { sum += input[i]; output.Add(sum); }
            return output;
        }

        public static float SumLogic(NativeArray<float> input, NativeArray<float> output) {
            float s = 0; foreach(var v in input) s += v; output[0] = s; return s;
        }
        public static double SumLogic(NativeArray<double> input, NativeArray<double> output) {
            double s = 0; foreach(var v in input) s += v; output[0] = s; return s;
        }
        public static int SumLogic(NativeArray<int> input, NativeArray<int> output) {
            int s = 0; foreach(var v in input) s += v; output[0] = s; return s;
        }
        public static float SumLogic(float[] input) { float s=0; foreach(var v in input) s+=v; return s; }
        public static float SumLogic(System.Collections.Generic.IEnumerable<float> input) { float s=0; foreach(var v in input) s+=v; return s; }
        public static double SumLogic(double[] input) { double s=0; foreach(var v in input) s+=v; return s; }
        public static double SumLogic(System.Collections.Generic.IEnumerable<double> input) { double s=0; foreach(var v in input) s+=v; return s; }
        public static int SumLogic(int[] input) { int s=0; foreach(var v in input) s+=v; return s; }
        public static int SumLogic(System.Collections.Generic.IEnumerable<int> input) { int s=0; foreach(var v in input) s+=v; return s; }

        public static int RandomLogic(int from, int to) => UnityEngine.Random.Range(from, to + 1);
        public static float RandomLogic(float from, float to) => UnityEngine.Random.Range(from, to);
        public static double RandomLogic(double from, double to) => (double)UnityEngine.Random.Range((float)from, (float)to);

        public static string ShowLogic(long value) => value.ToString();
        public static string ShowLogic(ulong value) => value.ToString();
        public static string ShowLogic(int value) => value.ToString();
        public static string ShowLogic(uint value) => value.ToString();
        public static string ShowLogic(float value) => value.ToString();
        public static string ShowLogic(double value) => value.ToString();
#endregion
    }
}
