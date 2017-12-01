using System.Collections;
using UnityEngine;
/// <summary>
/// ゲームシーンでオブジェクト生成クラスのインスタンス同期を行うクラス
/// </summary>
public class GameSceneGenerator : Photon.MonoBehaviour 
{
	void Start ()
	{
		PhotonNetwork.SetPlayerCustomProperties(Define.defaultPropaties);
		ExitGames.Client.Photon.Hashtable hashtable = PhotonNetwork.player.CustomProperties;
	}

	private IEnumerator DelayInstance()
	{
		yield return StartCoroutine(Wait());
		
		
	}

	private IEnumerator Wait()
	{
		yield return null;
	}
}