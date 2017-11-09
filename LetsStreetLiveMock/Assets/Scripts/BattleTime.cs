using UnityEngine;

/// <summary>
/// BattleTime クラス
/// 製作者：実川
/// </summary>
public class BattleTime : MonoBehaviour
{
	public float CurrentTime
	{
		get { return _currentTime; }
	}

	[SerializeField]
	private Battle _battle;

	[SerializeField]
	private float _limitTime;

	[SerializeField]
	private float _currentTime = 0.0f;

	private void Start()
	{
		_currentTime = _limitTime;
	}

	void Update ()
	{
		_currentTime -= Time.deltaTime;

		if (_currentTime > 0.0f) return;

		_currentTime = 0.0f;
		_battle.TransScene();
	}
}
