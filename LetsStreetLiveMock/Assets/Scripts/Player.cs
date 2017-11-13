﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WiimoteApi;
using WiimoteApi.Internal;
using WiimoteApi.Util;

/// <summary>
/// Player クラス
/// 製作者：実川
/// </summary>
public class Player : MonoBehaviour
{
	public Define.PlayerType Type
	{
		get { return _type; }
	}

	public Color PlayerColor
	{
		get { return _playerColor; }
	}

	[SerializeField]
	private Color _playerColor;

	[SerializeField]
	private Define.PlayerType _type;

	[SerializeField]
	private Rigidbody _rb;

	[SerializeField]
	private float _power;

	[SerializeField]
	private float _rotatePower;

	[SerializeField]
	private Animator _animator;

	[SerializeField]
	private Dance _dance;

	[SerializeField]
	private PlayerManager _playerManager;

	// wiiリモコン
	private Wiimote _wm;
	private int _wmNum;

	void Start()
	{
		// wiiリモコン初期化処理
		WiimoteManager.FindWiimotes();
		_wmNum = (int)_type - 1;
		if (WiimoteManager.HasWiimote(_wmNum))
		{
            _wm = WiimoteManager.Wiimotes[_wmNum];
            _wm.InitWiiMotionPlus();
			int i = _wmNum + 1;
			_wm.SendPlayerLED(i == 1, i == 2, i == 3, i == 4);
		}

		_playerManager.onDanceStart += () =>
		{
			_dance.Begin();
		};
	}

	void Update()
	{
		if (!_dance.IsPlaying)
		{
			if (Input.GetKey("up") || WiimoteManager.GetButton(_wmNum, ButtonData.WMBUTTON_RIGHT))
				_rb.AddForce(transform.forward * _power);
			if (Input.GetKey("left") || WiimoteManager.GetButton(_wmNum, ButtonData.WMBUTTON_UP))
				transform.Rotate(Vector3.up, -_rotatePower);
			if (Input.GetKey("right") || WiimoteManager.GetButton(_wmNum, ButtonData.WMBUTTON_DOWN))
				transform.Rotate(Vector3.up, _rotatePower);
			if (Input.GetKey("down") || WiimoteManager.GetButton(_wmNum, ButtonData.WMBUTTON_LEFT))
				_rb.AddForce(-transform.forward * _power);
		}
	}

    void OnApplicationQuit()
    {
		// 一括で実行させる
        if (WiimoteManager.Wiimotes.Count > 0)
        {
            _wm = WiimoteManager.Wiimotes[0];
            WiimoteManager.Cleanup(_wm);
            _wm = null;
            WiimoteManager.Wiimotes.Clear();
        }
    }
}
