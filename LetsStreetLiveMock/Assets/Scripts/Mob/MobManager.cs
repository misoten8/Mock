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

		float[][] playerScore = _mobs.Select(e => e.FanPointArray).ToArray();
		_score.SetScore(Define.PlayerType.First, (int)playerScore[(int)Define.PlayerType.First].Sum());
		_score.SetScore(Define.PlayerType.Second, (int)playerScore[(int)Define.PlayerType.Second].Sum());
		_score.SetScore(Define.PlayerType.Third, (int)playerScore[(int)Define.PlayerType.Third].Sum());
		_score.SetScore(Define.PlayerType.Force, (int)playerScore[(int)Define.PlayerType.Force].Sum());
		_isScoreChange = false;
	}
}