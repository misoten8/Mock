using System.Collections;
using UnityEngine;
using Misoten8Utility;
using System.Collections.Generic;
using System.Linq;

/// <summary>
/// モブキャラ生成 クラス
/// 製作者：実川
/// </summary>
public class MobGenerator : Photon.MonoBehaviour
{
	/// <summary>
	/// モブ管理クラス
	/// </summary>
	[SerializeField]
	private MobManager _mobManager;

	/// <summary>
	/// プレイヤー管理クラス
	/// </summary>
	[SerializeField]
	private PlayerManager _playerManager;

	/// <summary>
	/// スコア
	/// </summary>
	[SerializeField]
	private Score _score;

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
	///　初期化時に渡すキャッシュクラス
	/// </summary>
	public struct MobCaches
	{
		public MobManager mobManager;
		public PlayerManager playerManager;
		public int instanceID;

		public MobCaches(MobManager MobManager, PlayerManager PlayerManager, int InstanceID)
		{
			mobManager = MobManager;
			playerManager = PlayerManager;
			instanceID = InstanceID;
		}
	}

	private MobCaches _mobCaches;

	public void CreateStart()
	{
		//if(photonView.ownerId == )
		StartCoroutine(Enumerator());
	}

	void Start()
	{
		_mobCaches = new MobCaches(_mobManager, _playerManager, 0);
		CreateStart();
	}

	private void Create()
	{
		GameObject people = Instantiate(_peplePrefab[Random.Range(0, 3)]);
		people.transform.position = transform.position + new Vector3(Random.Range(-_rangeSize.x, _rangeSize.x), 0, Random.Range(-_rangeSize.y, _rangeSize.y));

		var mob = people.GetComponent<Mob>();
		_mobCaches.instanceID++;
		mob.OnAwake(_mobCaches);
		_mobManager.Mobs.Add(mob);
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
