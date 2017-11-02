using System.Collections;
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

	[SerializeField]
	private Define.PlayerType _type;

	[SerializeField]
	private Rigidbody _rb;

	[SerializeField]
	private float _power;

	[SerializeField]
	private Animator _animator;

	[SerializeField]
	private Dance _dance;

	private Wiimote _wm;
	private bool _wmActive = false;

	void Start()
	{
		// wiiリモコン初期化処理
		WiimoteManager.FindWiimotes();
		_wm = WiimoteManager.Wiimotes[0];
		_wm.InitWiiMotionPlus();
		_wm.SendPlayerLED(true, false, false, false);
		_wmActive = false;
	}

	void Update()
	{
		if (!_dance.IsPlaying)
		{
			_wm = WiimoteManager.Wiimotes[0];
			_wm.ReadWiimoteData();
			if (Input.GetKey("up") || _wm.Button.d_right) _rb.AddForce(Vector3.forward * _power);
			if (Input.GetKey("left") || _wm.Button.d_up) _rb.AddForce(Vector3.left * _power);
			if (Input.GetKey("right") || _wm.Button.d_down) _rb.AddForce(Vector3.right * _power);
			if (Input.GetKey("down") || _wm.Button.d_left) _rb.AddForce(Vector3.back * _power);

			//if (Input.GetKeyDown("j")) _rb.AddForce(Vector3.up * _power / 20, ForceMode.Impulse);
			if (Input.GetKeyDown("k"))
			{
				_dance.Begin();
				//_animator.SetBool("PlayDance", true);
			}
		}
		else
		{
			if (Input.GetKeyDown("k"))
			{
				//_animator.SetBool("PlayDance", false);
				_dance.Cancel();
			}
		}
		//_animator.SetFloat("Velocity", (Mathf.Abs(_rb.velocity.x) + Mathf.Abs(_rb.velocity.z)) / 2.0f);
	}
}
