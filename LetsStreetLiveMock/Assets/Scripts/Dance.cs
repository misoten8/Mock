using System;
using System.Collections;
using UnityEngine;

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

	[SerializeField]
	private Camera _camera;

	/// <summary>
	/// ダンスの効果範囲の当たり判定
	/// </summary>
	[SerializeField]
	private SphereCollider _danceCollider;

	[SerializeField]
	private Player _player;

	[SerializeField]
	private DanceUI _danceUI;

	/// <summary>
	/// 人々に渡すファンポイント...ダンスの出来によって変動する
	/// </summary>
	private int _giveFanPoint = 100;

	private bool _isSuccess = false;

	private bool _isRequestShake = false;

	private void Start()
	{
		_danceCollider.enabled = false;
		_danceUI.NotActive();
		_giveFanPoint = 0;
	}

	void Update()
	{
		if (IsPlaying)
		{
			if (Input.GetKeyDown("return"))
			{
				ChangeFanPoint(1);
			}
			_danceUI.DancePointUpdate(_giveFanPoint);
		}
	}

	/// <summary>
	/// ダンス開始
	/// </summary>
	public void Begin()
	{
		_giveFanPoint = 0;
		_danceCollider.enabled = true;
		SetCamera(true);
		_danceUI.Active();
	}

	/// <summary>
	/// ダンス終了
	/// </summary>
	public void End()
	{
		OnEndDance?.Invoke();
		_danceUI.SetResult(IsSuccess);
		_danceUI.NotActive();
		SetCamera(false);
		_danceCollider.enabled = false;
		_giveFanPoint = 0;
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

	/////// <summary>
	/////// ファンポイントを減少し続ける
	/////// </summary>
	////private IEnumerator FanPointDecreaser()
	////{
	////	yield return new WaitForSeconds(2.0f);
	////	_danceUI.NotActive();
	////}

	private void ChangeFanPoint(int addValue)
	{
		_giveFanPoint += addValue;
		if(_giveFanPoint < 0)
		{
			_isSuccess = false;
			_danceUI.DancePointColor(new Color(1.0f, 0.0f, 0.0f));
			return;
		}
		else if(_giveFanPoint > 15)
		{
			_isSuccess = true;
			_danceUI.DancePointColor(new Color(0.0f, 1.0f, 0.0f));
		}
		else
		{
			_isSuccess = false;
			_danceUI.DancePointColor(new Color(1.0f, 1.0f, 0.0f));
		}
	}
}
