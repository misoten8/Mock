using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// LobbySceneManager クラス
/// 製作者：実川
/// </summary>
public class LobbySceneManager : MonoBehaviour 
{
	void Update () 
	{
		if(Input.GetKeyDown("return"))
		{
			if (PhotonNetwork.inRoom)
			{
				SceneManager.LoadScene("Battle");
				return;
			}
			Debug.LogWarning("まだゲーム開始の準備ができていません");
		}
	}
}