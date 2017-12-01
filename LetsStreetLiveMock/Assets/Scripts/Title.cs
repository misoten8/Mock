using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Title クラス
/// 製作者：実川
/// </summary>
public class Title : MonoBehaviour
{
	void Start()
	{

	}

	void Update()
	{

	}

	private void OnGUI()
	{
		GUI.Label(new Rect(new Vector2(0, 0), new Vector2(300, 200)), "Title Scene");
	}

	public void TransScene()
	{
		SceneManager.LoadScene("Lobby");
	}
}
