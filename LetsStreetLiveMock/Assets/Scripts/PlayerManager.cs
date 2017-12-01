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
	public const float BATTLE_TIME = 5.0f;

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

    private Define.PlayerType _type1, _type2;

	private void Awake()
	{
		_isBattleActive = true;
		//StartCoroutine("DanceStartEvent");
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
        Define.PlayerType type1, type2;
        type1 = _type1;
        type2 = _type2;
    
        yield return new WaitForSeconds(BATTLE_TIME);
        Debug.Log("Dancestartevent");
        //プレイヤーのバトル終了を通知
        if (_players[(int)type1 - 1].battlegauge > _players[(int)type2 - 1].battlegauge)
        {
            Debug.Log(_players[(int)type1 - 1].Type + "P勝ち");
            Debug.Log(_players[(int)type2 - 1].Type + "P負け");
            _players[(int)type1 - 1].OnBattleEnd(true);
            _players[(int)type2 - 1].OnBattleEnd(false);
            Debug.Log("1P : "+_players[(int)type1 - 1].battlegauge);
            Debug.Log("2P : "+_players[(int)type2 - 1].battlegauge);
        }
        else if (_players[(int)type1 - 1].battlegauge < _players[(int)type2 - 1].battlegauge)
        {
            Debug.Log(_players[(int)type1 - 1].Type + "P負け");
            Debug.Log(_players[(int)type2 - 1].Type + "P勝ち");
            _players[(int)type1 - 1].OnBattleEnd(false);
            _players[(int)type2 - 1].OnBattleEnd(true);
            Debug.Log("1P : " + _players[(int)type1 - 1].battlegauge);
            Debug.Log("2P : " + _players[(int)type2 - 1].battlegauge);
        }
        else
        {
            Debug.Log("引き分け");
            _players[(int)type1 - 1].OnBattleEnd(false);
            _players[(int)type2 - 1].OnBattleEnd(false);
            Debug.Log("1P : " + _players[(int)type1 - 1].battlegauge);
            Debug.Log("2P : " + _players[(int)type2 - 1].battlegauge);
        }
        yield return null;
	}
    /// <summary>
    /// バトル開始時の処理
    /// </summary>
    /// <param name="type1"></param>
    /// <param name="type2"></param>
    public void OnBattleStart(Define.PlayerType type1, Define.PlayerType type2)
    {
        Debug.Log("Onbattlestart");
        if (_players[(int)type1 - 1].GetPlayerMode() == Player.PLAYERMODE.BATTLE || _players[(int)type2 - 1].GetPlayerMode() == Player.PLAYERMODE.BATTLE)
        {
            return;
        }
        _type1 = type2;
        _type2 = type1;

        Debug.Log("Onbattlestart（）");
        StartCoroutine(DanceStartEvent());
        
        //バトル開始の処理
       
    }


}