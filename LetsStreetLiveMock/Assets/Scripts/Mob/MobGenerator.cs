using System.Collections;
using UnityEngine;
using Misoten8Utility;
using System.Collections.Generic;
using System.Linq;

/// <summary>
/// モブキャラ生成 クラス
/// 製作者：実川
/// </summary>
public class MobGenerator : MonoBehaviour
{
	/// <summary>
	/// 人のプレハブ
	/// </summary>
	[SerializeField]
	private GameObject[] _peplePrefab;

	/// <summary>
	/// 生成数
	/// </summary>
	[SerializeField]
	private int _createNum;

	/// <summary>
	/// 1フレームで生成する数
	/// </summary>
	[SerializeField]
	private int _frameCreateNum;

	/// <summary>
	/// 生成範囲
	/// </summary>
	[SerializeField]
	private Vector2 _rangeSize;

	/// <summary>
	/// 人のリスト
	/// </summary>
	private List<Mob> _list = new List<Mob>();

	public void CreateStart()
	{
		StartCoroutine(Enumerator());
	}

	void Start()
	{
		CreateStart();
	}

	private void Create()
	{
		GameObject people = Instantiate(_peplePrefab[Random.Range(0, 3)]);
		people.transform.position = transform.position + new Vector3(Random.Range(-_rangeSize.x, _rangeSize.x), 0, Random.Range(-_rangeSize.y, _rangeSize.y));
		_list.Add(people.GetComponent<Mob>());
		//foreach(Mob element in _list )
		//{
		//	element.FanPointArray.Sum();
		//}
	//	_list.Select(e => e.FanPointArray).ToArray().SumDoubleArray(4)
		//.SumArray(3).ToArray();	
	}

	private IEnumerator Enumerator()
	{
		if (_frameCreateNum.IsOutRange(0, _createNum))
		{
			Debug.LogWarning("1フレームで生成する数が異常です。処理を中断します\n　生成数：" + _frameCreateNum.ToString());
			yield break;
		}

		for (int i = 0; i < _createNum; i += _frameCreateNum)
		{
			_frameCreateNum.Loop(x => Create());
			yield return null;
		}
	}
}
