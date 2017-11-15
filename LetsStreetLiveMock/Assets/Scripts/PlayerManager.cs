using System;
using System.Collections;
using UnityEngine;

/// <summary>
/// プレイヤー管理 クラス
/// プレイヤー全体に対するイベント等の発信や、変数の保管等を行う
/// 製作者：実川
/// </summary>
public class PlayerManager : MonoBehaviour
{
	/// <summary>
	/// ダンス時間
	/// </summary>
	public const float DANCE_TIME = 30.0f;

	/// <summary>
	/// ダンス開始するまでの間隔
	/// </summary>
	public const float DANCE_START_INTERVAL = 20.0f;

	/// <summary>
	/// 一回のダンスで発生するリクエスト回数
	/// </summary>
	public const int REQUEST_COUNT = 3;

	// プレイヤーのスコアが取得できるようにする

	/// <summary>
	/// ダンス開始実行イベント
	/// </summary>
	public event Action onDanceStart;

	/// <summary>
	/// 現在がダンスモードかどうか
	/// </summary>
	public bool IsDanceMode
	{
		get { return _isDanceMode; }
	}

	private bool _isDanceMode = false;

	/// <summary>
	/// プレイヤーキャッシュ配列
	/// いずれプレイヤーを動的生成する仕組みに変更するため、一時措置
	/// </summary>
	[SerializeField]
	private Player[] _players;

	/// <summary>
	/// バトル再生状態かどうか
	/// </summary>
	private bool _isBattleActive = true;

	private void Awake()
	{
		_isBattleActive = true;
		StartCoroutine("DanceStartEvent");
	}

	private void OnDisable()
	{
		_isBattleActive = false;
	}

	public Player GetPlayer(Define.PlayerType playerType)
	{
		if (playerType == Define.PlayerType.None)
			return null;
		
		return _players[(int)playerType - 1];
	}

	/// <summary>
	/// ダンス開始処理
	/// </summary>
	private IEnumerator DanceStartEvent()
	{
		do
		{
			yield return new WaitForSeconds(DANCE_START_INTERVAL);

			_isDanceMode = true;

			// ダンス開始イベントを実行
			onDanceStart?.Invoke();

			yield return new WaitForSeconds(DANCE_TIME);

			_isDanceMode = false;

			// 全体通知イベントを実行する

		} while (_isBattleActive);

		yield return null;
	}
}