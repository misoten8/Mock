using UnityEngine;

/// <summary>
/// モブキャラ操作 クラス
/// 移動処理等を行います
/// 製作者：実川
/// </summary>
public class MobController : MonoBehaviour
{
	[SerializeField]
	private Mob _mob;

	[SerializeField]
	private FollowMove _followMove;

	[SerializeField]
	private WanderMove _wanderMove;

	private IMove _currentMove;

	void Start()
	{
		_mob.onChangeFun += () =>
		{
			if (_mob.FunType != Define.PlayerType.None)
			{
				_followMove.OnStart(_mob.funPlayer.transform);
				_wanderMove.enabled = false;
				_currentMove = _followMove;
			}
			else
			{
				_wanderMove.OnStart();
				_followMove.enabled = false;
				_currentMove = _wanderMove;
			}
		};

		_followMove.OnTransCheck = () =>
		{
			// 条件式

			// 実行処理(別のイベント変数に定義するべき)
			//_followMove.enabled = false;
		};

		// 最初は徘徊移動モードにする
		_wanderMove.OnStart();
	}

}