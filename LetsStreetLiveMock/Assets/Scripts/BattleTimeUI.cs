using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// BattleTimeUI クラス
/// 製作者：実川
/// </summary>
public class BattleTimeUI : MonoBehaviour
{
	[SerializeField]
	private Text _text;

	[SerializeField]
	private BattleTime _battleTime;

	void Start ()
	{
		_text.text = "Time Limit:" + _battleTime.CurrentTime.ToString("F0");
	}
	
	void Update ()
	{
		_text.text = "Time Limit:" + _battleTime.CurrentTime.ToString("F0");
	}
}
