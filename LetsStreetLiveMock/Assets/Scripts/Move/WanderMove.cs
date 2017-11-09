using System;
using UniRx;
using UnityEngine;

/// <summary>
/// 徘徊移動 クラス
/// 製作者：実川
/// </summary>
public class WanderMove : MonoBehaviour
{
	private enum State
	{
		Move,
		Stop
	}

	private State _state = State.Move;

	/// <summary>
	/// 遷移条件判定イベント
	/// Updateのタイミングで呼ばれます
	/// </summary>
	public Action OnCheck
	{
		set { _onCheck += value; }
	}

	private Action _onCheck;

	/// <summary>
	/// 遷移時実行イベント
	/// </summary>
	public Action OnTrans
	{
		set { _onTrans += value; }
	}

	private Action _onTrans;

	[SerializeField]
	private Rigidbody _rb;

	/// <summary>
	/// 移動する速度
	/// </summary>
	[SerializeField]
	private float _velocity;

	/// <summary>
	/// 移動方向
	/// </summary>
	private Vector3 _moveDirection;

	/// <summary>
	/// 最初に呼ばれるフレームかどうか
	/// </summary>
	private bool _isFirstFrame = true;

	/// <summary>
	/// 初期化処理
	/// </summary>
	public void OnStart()
	{
		enabled = true;
		_isFirstFrame = true;
	}

	void Update()
	{
		switch (_state)
		{
			case State.Move:
				if (_isFirstFrame)
				{
					_isFirstFrame = false;
					// 移動方向決める
					float rotationY = UnityEngine.Random.Range(-10.0f, 10.0f);
					_moveDirection = new Vector3(Mathf.Sin(rotationY) * 1, 0, Mathf.Cos(rotationY) * 1);

					Observable
						.Timer(TimeSpan.FromSeconds(UnityEngine.Random.Range(6, 18)))
						.Subscribe(e => 
						{
							_state = State.Stop;
							_isFirstFrame = true;
						});
				}
				// 回転処理
				transform.rotation = Quaternion.LookRotation(_moveDirection);

				// 移動処理
				_rb.AddForce(_moveDirection * _velocity);

				// 遷移チェック
				_onCheck?.Invoke();
				break;
			case State.Stop:
				if (_isFirstFrame)
				{
					_isFirstFrame = false;

					Observable
						.Timer(TimeSpan.FromSeconds(UnityEngine.Random.Range(3, 9)))
						.Subscribe(e =>
						{
							_state = State.Move;
							_isFirstFrame = true;
						});
				}
				// 遷移チェック
				_onCheck?.Invoke();
				break;
		}
	}
}