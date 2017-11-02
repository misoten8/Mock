﻿using System.Linq;
using UnityEngine;

/// <summary>
/// People クラス
/// 製作者：実川
/// </summary>
public class People : MonoBehaviour
{
	[SerializeField]
	private Define.FanLevel _fanLevel;

	[SerializeField]
	private Score _score;

	[SerializeField]
	private FanPoint _fanPoint;

	/// <summary>
	/// マテリアルを設定する対象メッシュ
	/// </summary>
	[SerializeField]
	private MeshRenderer _meshRenderer;

	/// <summary>
	/// インポートするマテリアル(複製するので、このマテリアルは直接使用しない)
	/// </summary>
	[SerializeField]
	private Material _importMaterial;

	/// <summary>
	/// ファンポイント
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

	/// <summary>
	/// 影響力
	/// 配列の合計で人のファンポイント最大値になるようにする
	/// ファンポイント最大値はファンタイプに応じて変わる
	/// 0...無所属 1~...プレイヤー
	/// </summary>
	private float[] _fanPointArray = new float[Define.METER_NUM_MAX] { 0, 0, 0, 0, 0 };

	private void Start()
	{
		_score = GameObject.Find("BattleManager").GetComponent<Score>();
		// 無所属に全てのファンポイントを設定
		_fanPointArray[0] = Define.FanPointArray[(int)_fanLevel];
		//_importMaterial = new Material(_importMaterial);
		//_meshRenderer.materials[1] = _importMaterial; 
	}

	private void OnTriggerEnter(Collider other)
	{
		if (other.tag != "DanceRange") return;

		Dance playerDance = other.gameObject.GetComponent<Dance>();

		// メソッドをスタック
		playerDance.OnEndDance += () =>
		{
			// 好感度を設定
			SetFanPoint(playerDance.PlayerType, playerDance.GiveFanPoint);
			// 好感度ビルボードのマテリアルを更新
			_fanPoint.UpdateMaterial();
			// アウトラインの更新
			_importMaterial.color = SetColor(Define.PlayerType.First);
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

	private Color SetColor(Define.PlayerType type)
	{
		switch (type)
		{
			case Define.PlayerType.None:
				return new Color(0.2f, 0.2f, 0.2f);
			case Define.PlayerType.First:
				return new Color(1.0f, 0.0f, 0.0f);
			case Define.PlayerType.Second:
				return new Color(0.0f, 0.0f, 1.0f);
			case Define.PlayerType.Third:
				return new Color(0.0f, 1.0f, 0.0f);
			case Define.PlayerType.Force:
				return new Color(1.0f, 1.0f, 0.0f);
			default:
				return new Color(1.0f, 1.0f, 1.0f);
		}
	}

	
}
