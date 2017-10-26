using UnityEngine;

/// <summary>
/// DanceUI クラス
/// 製作者：実川
/// </summary>
public class DanceUI : MonoBehaviour
{
	[SerializeField]
	private MeshRenderer _mesh;

	[SerializeField]
	private DisplayMediator _displayMediator;

	private void Start()
	{
		_mesh.enabled = true;
		_displayMediator.DanceSuccess.enabled = false;
		_displayMediator.DanceFailure.enabled = false;
		_displayMediator.DancePoint.enabled = false;
	}

	public void Active()
	{
		_mesh.enabled = false;
		_displayMediator.DancePoint.enabled = true;
	}

	public void NotActive()
	{
		_mesh.enabled = true;
		_displayMediator.DancePoint.enabled = false;
		_displayMediator.DanceSuccess.enabled = false;
		_displayMediator.DanceFailure.enabled = false;
	}

	public void SetResult(bool success)
	{
		if(success)
		{
			_displayMediator.DanceSuccess.enabled = true;
		}
		else
		{
			_displayMediator.DanceFailure.enabled = true;
		}
	}

	public void DancePointUpdate(int value)
	{
		_displayMediator.DancePoint.text = "DancePoint :" + value.ToString();
	}

	public void DancePointColor(Color color)
	{
		_displayMediator.DancePoint.color = color;
	}
}
