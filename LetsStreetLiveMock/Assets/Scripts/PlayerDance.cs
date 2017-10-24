using System;
using UnityEngine;

/// <summary>
/// PlayerDance クラス
/// 製作者：実川
/// </summary>
public class PlayerDance : MonoBehaviour
{
	public Define.PlayerType PlayerType
	{
		get { return _player.Type; }
	}
	
	public bool IsPlaying
	{
		get { return _danceCollider.enabled; }
	}

	public int GiveFanPoint
	{
		get { return _giveFanPoint; }
	}

	/// <summary>
	/// ダンスの効果範囲の当たり判定
	/// </summary>
	[SerializeField]
	private SphereCollider _danceCollider;

	[SerializeField]
	private Player _player;

	[SerializeField]
	private DanceUI _danceUI;

	public Action OnEndDance;

	/// <summary>
	/// 人々に渡すファンポイント
	/// </summary>
	private int _giveFanPoint = 100;

	/// <summary>
	/// ダンス開始
	/// </summary>
	public void Begin()
	{
		_danceCollider.enabled = true;
		_danceUI.Active();
	}

	/// <summary>
	/// ダンス終了
	/// </summary>
	public void End()
	{
		_danceCollider.enabled = false;
		_danceUI.NotActive();
		OnEndDance?.Invoke();
	}
}
