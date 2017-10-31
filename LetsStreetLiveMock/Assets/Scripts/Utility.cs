using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using System.Threading;
using System.Threading.Tasks;

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
		public static IEnumerable<T> ElementsRange<T>(this IEnumerable<T> source, int beginIndex, int endIndex)
		{
			return source.Skip(beginIndex).Take(endIndex - beginIndex);
		}
	}
}
