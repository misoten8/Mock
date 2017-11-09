using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Battle クラス
/// 製作者：実川
/// </summary>
public class Battle : MonoBehaviour
{
	[SerializeField]
	private Score _score;

	private void Start()
	{
		AudioManager.PlayBGM("DJ Striden - Lights [Dream Trance]");
	}

	private void OnGUI()
	{
		GUI.Label(new Rect(new Vector2(0, 0), new Vector2(300, 200)) ,"Battle Scene");
	}

	public void TransScene()
	{
		ResultScore.scoreArray[(int)Define.PlayerType.First] = _score.GetScore(Define.PlayerType.First);
		ResultScore.scoreArray[(int)Define.PlayerType.Second] = _score.GetScore(Define.PlayerType.Second);
		ResultScore.scoreArray[(int)Define.PlayerType.Third] = _score.GetScore(Define.PlayerType.Third);
		ResultScore.scoreArray[(int)Define.PlayerType.Force] = _score.GetScore(Define.PlayerType.Force);

		SceneManager.LoadScene("Result");
	}
}
