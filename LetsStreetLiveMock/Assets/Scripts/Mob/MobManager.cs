using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// MobManager クラス
/// いずれ、ダンスの当たり判定等もここで行うようにする
/// 製作者：実川
/// </summary>
public class MobManager : MonoBehaviour
{
	/// <summary>
	/// スコア変化時に通知する
	/// </summary>
	public Action OnScoreChange
	{
		get { return _onScoreChange; }
	}

	private Action _onScoreChange;

	/// <summary>
	/// モブキャラのリスト
	/// </summary>
	public List<Mob> Mobs
	{
		get { return _mobs; }
	}

	private List<Mob> _mobs = new List<Mob>();

	[SerializeField]
	private Score _score;

	private bool _isScoreChange;

	private void Start()
	{
		_onScoreChange = () => 
		{
			_isScoreChange = true;
		};
	}

	private void Update()
	{
		if (_mobs.Count == 0)
			return;

		// 一括で設定する
		if (!_isScoreChange)
			return;

		//float[][] playerScore = _mobs.Select(e => e.FanPointArray).ToArray();
		_score.SetScore(Define.PlayerType.First, _mobs.Where(e => e.FunType == Define.PlayerType.First).Count());
		_score.SetScore(Define.PlayerType.Second, _mobs.Where(e => e.FunType == Define.PlayerType.Second).Count());
		_score.SetScore(Define.PlayerType.Third, _mobs.Where(e => e.FunType == Define.PlayerType.Third).Count());
		_score.SetScore(Define.PlayerType.Force, _mobs.Where(e => e.FunType == Define.PlayerType.Force).Count());
		_isScoreChange = false;
	}
}