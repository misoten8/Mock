using System;
using UnityEngine;

/// <summary>
/// 追従移動 クラス
/// 製作者：実川
/// </summary>
public class FollowMove : MonoBehaviour
{
	/// <summary>
	/// 遷移条件判定イベント
	/// Updateのタイミングで呼ばれます
	/// </summary>
	public Action OnTransCheck
	{
		set { _onTransCheck += value; }
	}

	private Action _onTransCheck;

	[SerializeField]
	private Rigidbody _rb;

	/// <summary>
	/// 移動する速度
	/// </summary>
	[SerializeField]
	private float _velocity;

	/// <summary>
	/// 移動を中止する距離
	/// </summary>
	[SerializeField]
	private float _stopDistance;

	/// <summary>
	/// 速度の減少を開始する距離
	/// </summary>
	[SerializeField]
	private float _slowDistance;

	private Transform _target = null;

	/// <summary>
	/// 初期化処理
	/// </summary>
	public void OnStart(Transform target)
	{
		_target = target;
		enabled = true;
	}

	void OnDisable()
	{
		_target = null;
	}

	void Update()
	{
		if (_target == null)
		{
			Debug.Log("追従対象が見つからない為、非アクティブになります");
			enabled = false;
			return;
		}

		Vector3 moveDirection = Vector3.Normalize(_target.position - transform.position);
		float distance = Vector3.Distance(_target.position, transform.position);

		// 回転処理
		transform.rotation = Quaternion.LookRotation(moveDirection);

		// 移動処理
		_rb.AddForce(moveDirection * _velocity * Mathf.Min((distance - _stopDistance) / _slowDistance, 1.0f));

		// 遷移チェック
		_onTransCheck?.Invoke();
	}
}