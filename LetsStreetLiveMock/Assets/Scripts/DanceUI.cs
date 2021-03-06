﻿using UnityEngine;

/// <summary>
/// DanceUI クラス
/// 製作者：実川
/// </summary>
public class DanceUI : MonoBehaviour
{
	[SerializeField]
	private MeshRenderer _mesh;

	[SerializeField]
	private DisplayMediator _dm;

	private void Start()
	{
		_mesh.enabled = true;
		_dm.DanceSuccess.enabled = false;
		_dm.DanceFailure.enabled = false;
		_dm.DancePoint.enabled = false;
		_dm.DanceShake.enabled = false;
		_dm.DanceStop.enabled = false;
	}

	public void Active()
	{
		_mesh.enabled = false;
		_dm.DancePoint.enabled = true;
	}

	public void NotActive()
	{
		_mesh.enabled = true;
		_dm.DancePoint.enabled = false;
		_dm.DanceSuccess.enabled = false;
		_dm.DanceFailure.enabled = false;
		_dm.DanceShake.enabled = false;
		_dm.DanceStop.enabled = false;
	}

	public void SetResult(bool success)
	{
		if(success)
		{
			ParticleManager.Play("DanceEndClear", new Vector3(), transform);
			_dm.DanceSuccess.enabled = true;
		}
		else
		{
			ParticleManager.Play("DanceEndFailed", new Vector3(), transform);
			_dm.DanceFailure.enabled = true;
		}
	}

	public void SetPointUpdate(int value)
	{
		_dm.DancePoint.text = "DancePoint :" + value.ToString();
	}

	public void SetPointColor(Color color)
	{
		_dm.DancePoint.color = color;
	}

	public void SetRequestShake(bool isRequestShake)
	{
		if(isRequestShake)
		{
			_dm.DanceShake.enabled = true;
			_dm.DanceStop.enabled = false;
		}
		else
		{
			_dm.DanceShake.enabled = false;
			_dm.DanceStop.enabled = true;
		}
	}
}
