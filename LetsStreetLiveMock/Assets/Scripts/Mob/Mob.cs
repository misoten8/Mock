using System.Linq;
using UnityEngine;
using Misoten8Utility;
using System;

/// <summary>
/// モブキャラ クラス
/// 製作者：実川
/// </summary>
public class Mob : MonoBehaviour
{
	/// <summary>
	/// ファンポイント
	/// 配列の合計で人のファンポイント最大値になるようにする
	/// ファンポイント最大値はファンタイプに応じて変わる
	/// </summary>
	public float[] FanPointArray
	{
		get { return _fanPointArray; }
	}

	/// <summary>
	/// 合計を1.0にしたファンポイント
	/// </summary>
	public float[] InterpolationFanPointArray
	{
		get { return _fanPointArray.Select(x => x / Define.FanPointArray[(int)_fanLevel]).ToArray(); }
	}

	private float[] _fanPointArray = new float[Define.METER_NUM_MAX] { 0, 0, 0, 0, 0 };

	/// <summary>
	/// 一押しのプレイヤー(無所属の場合もある)
	/// </summary>
	public Define.PlayerType FunType
	{
		get { return _funType; }
	}

	[SerializeField]
	private Define.PlayerType _funType = Define.PlayerType.None;

	/// <summary>
	/// 現在モブキャラが推しているプレイヤー
	/// </summary>
	public Player funPlayer
	{
		get { return _funPlayer; }
	}

	private Player _funPlayer = null;

	/// <summary>
	/// モブが移動状態になった時に呼ぶイベント
	/// </summary>
	public event Action onMoveMob;

	/// <summary>
	/// モブがダンス視聴状態になった時に呼ぶイベント
	/// </summary>
	public event Action onDanceWatchMob;

	/// <summary>
	/// プレイヤー追従対象が変化した時に呼ぶイベント
	/// </summary>
	public event Action onChangeFllowPlayer;

	[SerializeField]
	private Define.FanLevel _fanLevel;

	[SerializeField]
	private MobController _mobController;

	/// <summary>
	/// マテリアルを設定する対象メッシュ
	/// </summary>
	[SerializeField]
	private MeshRenderer _meshRenderer;

	/// <summary>
	/// モブ管理クラス
	/// </summary>
	private MobManager _mobManager;

	/// <summary>
	/// プレイヤー管理クラス
	/// </summary>
	public PlayerManager PlayerManager
	{
		get { return _playerManager; }
	}

	private PlayerManager _playerManager;


	/// <summary>
	/// 追従する対象プレイヤー
	/// </summary>
	public Define.PlayerType FllowTarget
	{
		get { return _fllowTarget; }
	}

	private Define.PlayerType _fllowTarget = Define.PlayerType.None;

	/// <summary>
	/// モブ生成番号
	/// </summary>
	public int InstanceID
	{
		get { return _instanceID; }
	}

	private int _instanceID;

	/// <summary>
	/// ダンス視聴中エフェクト
	/// </summary>
	private GameObject _danceNowEffect;

	/// <summary>
	/// 既にダンスを視聴中かどうか
	/// </summary>
	private bool _isViewingInDance;


	/// <summary>
	/// モブ生成時に呼ばれる
	/// </summary>
	public void OnAwake(MobGenerator.MobCaches mobCaches)
	{
		_mobManager = mobCaches.mobManager;
		_playerManager = mobCaches.playerManager;
		_instanceID = mobCaches.instanceID;
	}

	private void Start()
	{
		// 無所属に全てのファンポイントを設定
		_fanPointArray[0] = Define.FanPointArray[(int)_fanLevel];

		// ファンタイプの更新
		_funType = (Define.PlayerType)_fanPointArray.FindIndexMax();

		// アウトラインの更新
		_meshRenderer.materials[1].color = new Color(0.2f, 0.2f, 0.2f);

		// モブ再生イベント実行
		onMoveMob?.Invoke();
	}

	private void OnTriggerStay(Collider other)
	{
		if (other.tag != "DanceRange")
			return;

		// 既に他のプレイヤーのダンスを視聴している場合無視する
		if (_isViewingInDance)
			return;

		Dance playerDance = other.gameObject.GetComponent<Dance>();

		// プレイヤーがダンス中であれば、視聴する
		if (playerDance.IsPlaying)
		{
			//Debug.Log(_instanceID.ToString() + "番のモブは視聴するドン！");
			// モブ停止イベント実行
			onDanceWatchMob?.Invoke();

			// ダンス視聴中エフェクト再生
			_danceNowEffect = ParticleManager.Play("DanceNow", new Vector3(), transform);

			_isViewingInDance = true;

			// ダンス終了イベントにメソッドを登録する
			playerDance.onEndDance += (isCancel, isSuccess) =>
			{
				_isViewingInDance = false;

				if (!isCancel)
				{
					// モブキャラ管理クラスにスコア変更を通知
					_mobManager.OnScoreChange();

					// ファンタイプが変更したかチェックする
					Define.PlayerType newFunType = isSuccess ? playerDance.Player.Type : Define.PlayerType.None;
					if (_funType != newFunType)
					{
						// ファンタイプの更新
						_funType = newFunType;

						// 推しているプレイヤーの更新
						_funPlayer = playerDance.Player;

						// 追従対象の更新
						_fllowTarget = playerDance.PlayerType;

						// アウトラインの更新
						_meshRenderer.materials[1].color = playerDance.Player.PlayerColor;
					}
				}

				// プレイヤーが客引き状態の場合、追従判定を行う
				if (_mobManager.GetFunCount(_fllowTarget) < _mobManager.GetFunCount(playerDance.PlayerType)
						|| _fllowTarget == Define.PlayerType.None)
				{
					if (FunType == Define.PlayerType.None)
					{
						// 追従対象の更新
						_fllowTarget = playerDance.PlayerType;
					}
				}

				// モブ再生イベント実行
				onMoveMob?.Invoke();

				Destroy(_danceNowEffect);
			};
		}
		else
		{
			// プレイヤーが客引き状態の場合、追従判定を行う
			if (_mobManager.GetFunCount(_fllowTarget) < _mobManager.GetFunCount(playerDance.PlayerType)
				|| _fllowTarget == Define.PlayerType.None)
			{
				if (FunType != Define.PlayerType.None)
					return;
				_fllowTarget = playerDance.PlayerType;
				onChangeFllowPlayer?.Invoke();
			}
		}
	}
}
