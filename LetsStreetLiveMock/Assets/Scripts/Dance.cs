using System;
using UnityEngine;
using UniRx;
using WiimoteApi;

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

	public bool IsPlaying
	{
		get { return _danceCollider.enabled; }
	}

	public int GiveFanPoint
	{
		get { return _giveFanPoint; }
	}

	public bool IsSuccess
	{
		get { return _isSuccess; }
	}

	public bool IsRequestShake
	{
		get { return _isRequestShake; }
	}

	/// <summary>
	/// ダンス終了時に呼ばれる
	/// </summary>
	public Action OnEndDance;

	public Player Player
	{
		get { return _player; }
	}

	[SerializeField]
	private Player _player;

	[SerializeField]
	private Camera _camera;

	/// <summary>
	/// ダンスの効果範囲の当たり判定
	/// </summary>
	[SerializeField]
	private SphereCollider _danceCollider;

	[SerializeField]
	private DanceUI _danceUI;

	/// <summary>
	/// 人々に渡すファンポイント...ダンスの出来によって変動する
	/// </summary>
	private int _giveFanPoint = 100;

	private bool _isSuccess = false;

	private bool _isRequestShake = false;

	private GameObject _particle;

	/// <summary>
	/// 処理中かどうか
	/// </summary>
	private bool _isTransing = false;

	private SingleAssignmentDisposable _disposable = null;

	// wiiリモコン
	private Wiimote _wm;
	private int _wmNum;

	private void Start()
	{
		_danceCollider.enabled = false;
		_danceUI.NotActive();
		_giveFanPoint = 0;

		_wmNum = (int)_player.Type - 1;
    }

	void Update()
	{
        if (IsPlaying)
		{
			if (_isTransing)
				return;
			if (Input.GetKeyDown("return") || WiimoteManager.GetSwing( _wmNum))
			{
				ChangeFanPoint(_isRequestShake ? 1 : -1);
				ParticleManager.Play(_isRequestShake ? "DanceNowClear" : "DanceNowFailed", new Vector3(), transform);
			}
			_danceUI.SetPointUpdate(_giveFanPoint);
		}
	}

	/// <summary>
	/// ダンス開始
	/// </summary>
	public void Begin()
	{
		_isTransing = false;
		_isSuccess = false;
		_giveFanPoint = 0;
		_danceCollider.enabled = true;
		SetCamera(true);
		_danceUI.Active();
		_disposable = new SingleAssignmentDisposable();
		// 最初は1秒待機する
		_disposable.Disposable = Observable
			.Timer(TimeSpan.FromSeconds(1))
			.Subscribe(_ =>
			{
				_danceCollider.enabled = true;
				_particle = ParticleManager.Play("DanceNow", new Vector3(), transform);
				_isRequestShake = true;
				_danceUI.SetRequestShake(_isRequestShake);
				Observable
					.Timer(TimeSpan.FromSeconds(10))
					.Subscribe(__ =>
					{
						_isRequestShake = false;
						_danceUI.SetRequestShake(_isRequestShake);
						Observable
							.Timer(TimeSpan.FromSeconds(10))
							.Subscribe(___ =>
							{
								_isRequestShake = true;
								_danceUI.SetRequestShake(_isRequestShake);
								Observable
									.Timer(TimeSpan.FromSeconds(10))
									.Subscribe(e => End());
							});
					});
			});
	}

	/// <summary>
	/// ダンス終了
	/// </summary>
	public void End()
	{
		if (_isTransing)
			return;

		OnEndDance.Invoke();
		_danceUI.SetResult(IsSuccess);
		Destroy(_particle);
		_isTransing = true;
		Observable
			.Timer(TimeSpan.FromSeconds(3))
			.Subscribe(_ =>
			{
				_isTransing = false;
				_danceUI.NotActive();
				SetCamera(false);
				_danceCollider.enabled = false;
				// スコアを設定する
				_giveFanPoint = 0;
			});
	}

	/// <summary>
	/// ダンスを中断する
	/// </summary>
	public void Cancel()
	{
		if (IsPlaying)
		{
			_isTransing = false;
			_danceUI.NotActive();
			SetCamera(false);
			_danceCollider.enabled = false;
			Destroy(_particle);
			// スコアを設定する
			_giveFanPoint = 0;
			_disposable.Dispose();
		}
	}

	/// <summary>
	/// カメラ動作制御
	/// カメラの位置、向きを設定する
	/// </summary>
	private void SetCamera(bool isPlay)
	{
		Transform camera = _camera.gameObject.transform;
		Vector3
			localPos = camera.localPosition,
			localAngle = camera.localEulerAngles;
		camera.localPosition = new Vector3(localPos.x, localPos.y, isPlay ? 2.0f : -2.0f);
		camera.localEulerAngles = new Vector3(localAngle.x, isPlay ? 180.0f : 0.0f, localAngle.z);
	}

	private void ChangeFanPoint(int addValue)
	{
		_giveFanPoint += addValue;
		_danceUI.SetPointColor(addValue > 0 ? new Color(0.0f, 1.0f, 0.0f) : new Color(1.0f, 0.0f, 0.0f));
		if (_giveFanPoint >= 30)
		{
			_isSuccess = true;
			_danceUI.SetPointColor(new Color(0.0f, 0.0f, 1.0f));
		}
	}
}
