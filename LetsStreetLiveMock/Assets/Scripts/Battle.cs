using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Battle クラス
/// 製作者：実川
/// </summary>
public class Battle : MonoBehaviour
{
	private void OnGUI()
	{
		GUI.Label(new Rect(new Vector2(0, 0), new Vector2(300, 200)) ,"Battle Scene");
	}

	public void TransScene()
	{
		SceneManager.LoadScene("Result");
	}
}
