using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using System.Threading;
using System.Threading.Tasks;
using System;

/// <summary>
/// misoten8Utility 名前空間
/// 製作者：実川
/// </summary>
namespace Misoten8Utility
{
	public static class FanMath
	{
		public static bool OverBorder(int fanPoint, Define.FanLevel fanLevel) => fanPoint >= Define.FanPointArray[(int)fanLevel];
		public static int GetFanScore(Define.FanLevel fanLevel) => Define.FanScoreArray[(int)fanLevel];
	}

	public static class EnumerableExtensions
	{
		/// <summary>
		/// 選択した範囲の要素を取り出す
		/// </summary>
		public static IEnumerable<T> ElementsRange<T>(this IEnumerable<T> element, int beginIndex, int endIndex)
		{
			return element.Skip(beginIndex).Take(endIndex - beginIndex);
		}

		/// <summary>
		/// 最大値の要素が格納されている番号を取得する
		/// </summary>
		public static int FindIndexMax<T>(this IEnumerable<T> element)
		{
			return element
				.Select((v, i) => new { Value = v, Index = i })
				.First(e => e.Value.Equals(element.Select(s => s).Max())).Index;
		}

		/// <summary>
		/// 最大値の要素が格納されている番号を取得する
		/// </summary>
		public static int FindIndexMin<T>(this IEnumerable<T> element)
		{
			return element
				.Select((v, i) => new { Value = v, Index = i })
				.First(e => e.Value.Equals(element.Select(s => s).Min())).Index;
		}
	}
}
