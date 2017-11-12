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
	/// モブが再生状態になった時に呼ぶイベント
	/// </summary>
	public event Action onPlayMob;

	/// <summary>
	/// モブが停止状態になった時に呼ぶイベント
	/// </summary>
	public event Action onStopMob;

	/// <summary>
	/// モブの推しが変化した時に呼ぶイベント
	/// </summary>
	public event Action onChangeFun;

	[SerializeField]
	private Define.FanLevel _fanLevel;

	[SerializeField]
	private FanPoint _fanPoint;

	[SerializeField]
	private MobController _mobController;

	/// <summary>
	/// マテリアルを設定する対象メッシュ
	/// </summary>
	[SerializeField]
	private MeshRenderer _meshRenderer;


	private MobManager _mobManager;

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
	}

	private void Start()
	{
		// 無所属に全てのファンポイントを設定
		_fanPointArray[0] = Define.FanPointArray[(int)_fanLevel];

		// ファンタイプの更新
		_funType = (Define.PlayerType)_fanPointArray.FindIndexMax();

		// アウトラインの更新
		_meshRenderer.materials[1].color = GetColor(_funType);

		// モブ再生イベント実行
		onPlayMob?.Invoke();
	}

	private void OnTriggerEnter(Collider other)
	{
		if (other.tag != "DanceRange")
			return;

		// 既に他のプレイヤーのダンスを視聴している場合無視する
		if (_isViewingInDance)
			return;

		Dance playerDance = other.gameObject.GetComponent<Dance>();

		// モブ停止イベント実行
		onStopMob?.Invoke();

		// ダンス視聴中エフェクト再生
		_danceNowEffect = ParticleManager.Play("DanceNow", new Vector3(), transform);

		_isViewingInDance = true;

		// ダンス終了イベントにメソッドを登録する
		playerDance.OnEndDance += (isCancel) =>
		{
			// モブ再生イベント実行
			onPlayMob?.Invoke();

			Destroy(_danceNowEffect);

			_isViewingInDance = false;

			// ダンスが中断された場合は処理を中断する
			if (isCancel)
				return;

			// 好感度を設定
			SetFanPoint(playerDance.PlayerType, playerDance.GiveFanPoint);

			// モブキャラ管理クラスにスコア変更を通知
			_mobManager.OnScoreChange();

			// 好感度ビルボードのマテリアルを更新
			_fanPoint.UpdateMaterial();

			// ファンタイプが変更したかチェックする
			Define.PlayerType newFunType = (Define.PlayerType)_fanPointArray.FindIndexMax();
			if (_funType != newFunType)
			{
				// ファンタイプの更新
				_funType = newFunType;

				// 推しているプレイヤーの更新
				_funPlayer = playerDance.Player;

				// アウトラインの更新
				_meshRenderer.materials[1].color = GetColor(_funType);

				// 一押しプレイヤー変化イベント実行
				onChangeFun?.Invoke();
			}
		};
	}

	// ポイントの計算...dancePoint(ダンスのスコア...効果範囲内の人全員に与える)
	// 人の種類に応じて好感度上昇量は異なる
	private void SetFanPoint(Define.PlayerType type, int dancePoint)
	{
		// ポイントを加算
		_fanPointArray[(int)type] += dancePoint;

		// ファンポイント最大値を取得
		float fanPointMax = Define.FanPointArray[(int)_fanLevel];
		float fanPointSum = _fanPointArray.Sum();

		// 値をファンポイント最大値に合わせてスケーリングする
		_fanPointArray = _fanPointArray.Select(x => fanPointMax * (x / fanPointSum)).ToArray();
	}

	private Color GetColor(Define.PlayerType type)
	{
		return _fanPoint.MeterColor[(int)type];
	}
}
