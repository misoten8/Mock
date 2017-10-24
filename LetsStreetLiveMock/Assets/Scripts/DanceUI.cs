using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// DanceUI クラス
/// 製作者：実川
/// </summary>
public class DanceUI : MonoBehaviour
{
	[SerializeField]
	private MeshRenderer _mesh;

	public void Active()
	{
		_mesh.enabled = true;
	}

	public void NotActive()
	{
		_mesh.enabled = false;
	}
}
