using System.Collections;
using UnityEngine;
using Misoten8Utility;

/// <summary>
/// PeopleGenerator クラス
/// 製作者：実川
/// </summary>
public class PeopleGenerator : MonoBehaviour
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
		Instantiate(_peplePrefab[Random.Range(0, 3)]).transform.position = transform.position + new Vector3(Random.Range(-_rangeSize.x, _rangeSize.x), 0, Random.Range(-_rangeSize.y, _rangeSize.y));
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
