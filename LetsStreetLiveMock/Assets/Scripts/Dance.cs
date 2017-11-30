using System;
using UnityEngine;
using UniRx;
using WiimoteApi;
using System.Linq;
using System.Collections.Generic;
using System.Collections;

/// <summary>
/// ダンス クラス
/// 製作者：実川
/// </summary>
public class Dance : MonoBehaviour
{
	public Define.PlayerType PlayerType
	{
		get { return _player.Type; }
	}

	/// <summary>
	/// ダンス中かどうか
	/// </summary>
	public bool IsPlaying
	{
		get { return _isPlaing; }
	}

	public bool IsSuccess
	{
		get { return _isSuccess; }
	}

	public bool IsRequestShake
	{
		get { return _isRequestShake; }
	}

	public Player Player
	{
		get { return _player; }
	}

	[SerializeField]
	private Player _player;

	/// <summary>
	/// ダンス終了時実行イベント
	/// bool型引数 -> このダンスが中断されたかどうか
	/// </summary>
	public event Action<bool, bool> onEndDance;

	[SerializeField]
	private Camera _camera;

	/// <summary>
	/// ダンスの効果範囲の当たり判定
	/// </summary>
	[SerializeField]
	private SphereCollider _danceCollider;

	[SerializeField]
	private DanceUI _danceUI;

	[SerializeField]
	private MeshRenderer _danceFloor;

	[SerializeField]
	private cameramanager _cameramanager;

	private int _dancePoint = 100;

	private bool _isSuccess = false;

	private bool _isRequestShake = false;

	/// <summary>
	/// 処理中かどうか
	/// </summary>
	private bool _isTransing = false;

	/// <summary>
	/// ダンス中かどうか
	/// </summary>
	private bool _isPlaing = false;

	// wiiリモコン
	private Wiimote _wm;
	private int _wmNum;



	/// <summary>
	/// 各リクエスト事の持続時間
	/// </summary>
	private float[] _requestTime = new float[PlayerManager.REQUEST_COUNT];

	private void Start()
	{
		_danceCollider.enabled = true;
		_danceUI.NotActive();
		_dancePoint = 0;

		_wmNum = (int)_player.Type - 1;
	}


    //初めて当たった時
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag != "DanceRange")
            return;
        Debug.Log("ダンスバトル開始");
    }
    //離れた時
    private void OnTriggerExit(Collider other)
    {
        if (other.tag != "DanceRange")
            return;
        Debug.Log("ダンスバトル終了");
    }



    void Update()
	{
		if (IsPlaying)
		{
			if (_isTransing)
				return;

			if (Input.GetKeyDown("return") || WiimoteManager.GetSwing(_wmNum))
			{
				ChangeFanPoint(_isRequestShake ? 1 : -1);
				ParticleManager.Play(_isRequestShake ? "DanceNowClear" : "DanceNowFailed", new Vector3(), transform);
			}
			_danceUI.SetPointUpdate(_dancePoint);
		}
	}

	private void ChangeFanPoint(int addValue)
	{
		_dancePoint += addValue;
		_danceUI.SetPointColor(addValue > 0 ? new Color(0.0f, 1.0f, 0.0f) : new Color(1.0f, 0.0f, 0.0f));
		if (_dancePoint >= 30)
		{
			_isSuccess = true;
			_danceUI.SetPointColor(new Color(0.0f, 0.0f, 1.0f));
		}
	}

}
