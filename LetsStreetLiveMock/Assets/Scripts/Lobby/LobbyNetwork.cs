using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// ロビーでの通信処理
/// </summary>
public class LobbyNetwork : Photon.MonoBehaviour
{
	private static readonly Dictionary<State, string> _messageMap = new Dictionary<State, string>
	{
		{ State.Start, "" },
		{ State.ConnectingLobby, "ネットワークに接続中です" },
		{ State.CreatingRoom, "ルームを作成しています" },
		{ State.JoingRoom, "ルームに入室しています" },
		{ State.WaitMember, "メンバーが揃うまで待機します(デバッグ時は開始できます)" },
		{ State.Ready, "メンバーが揃いました、ボタンを押してゲームを開始してください" }
	};

	private enum State
	{
		Start = 0,
		ConnectingLobby,
		CreatingRoom,
		JoingRoom,
		WaitMember,
		Ready,
		Max
	}

	private State _currentState = State.Start;

	private void Start () 
	{
		if (!PhotonNetwork.connected)
		{
			_currentState = State.ConnectingLobby;
			PhotonNetwork.offlineMode = false;
			// マスターサーバーへ接続  
			PhotonNetwork.ConnectUsingSettings("v0.1");
		}
	}

	/// <summary>  
	/// マスターサーバーのロビー入室時  
	/// </summary>  
	private void OnJoinedLobby()
	{
		if (PhotonNetwork.inRoom)
			return;

		if(PhotonNetwork.countOfRooms == 0)
		{
			// ルーム作成
			RoomOptions roomOptions = new RoomOptions();
			roomOptions.IsVisible = true;
			roomOptions.IsOpen = true;
			roomOptions.MaxPlayers = Define.PLAYER_NUM_MAX;
			roomOptions.CustomRoomProperties = new ExitGames.Client.Photon.Hashtable() { { "CustomProperties", "カスタムプロパティ" } };
			roomOptions.CustomRoomPropertiesForLobby = new string[] { "CustomProperties" };
			// ルームの作成
			PhotonNetwork.CreateRoom("Battle Room", roomOptions, new TypedLobby());
			_currentState = State.CreatingRoom;
		}
		else
		{
			// ルーム入室
			PhotonNetwork.JoinRoom("Battle Room");
			_currentState = State.JoingRoom;
		}
	}

	/// <summary>  
	/// ルーム参加時  
	/// </summary>  
	private void OnJoinedRoom()
	{
		_currentState = State.WaitMember;
		Debug.Log("ルームに入室しました あなたはplayer" + PhotonNetwork.player.ID.ToString());
	}

	/// <summary>  
	/// 他ユーザーがルームに接続した時  
	/// </summary>   
	private void OnPhotonPlayerConnected(PhotonPlayer newPlayer)
	{
		// 入室ログ表示  
		Debug.Log("player" + newPlayer.ID.ToString() + "が入室しました");

		if(PhotonNetwork.countOfPlayers == Define.PLAYER_NUM_MAX)
		{
			_currentState = State.Ready;
		}
	}

	/// <summary>  
	/// 他のユーザーのルーム退室時  
	/// </summary>  
	private void OnPhotonPlayerDisconnected(PhotonPlayer leavePlayer)
	{
		Debug.Log("player" + leavePlayer.ID.ToString() + "が退室しました");

		if (PhotonNetwork.countOfPlayers != Define.PLAYER_NUM_MAX)
		{
			_currentState = State.WaitMember;
		}
	}

	/// <summary>
	/// アプリケーション終了時実行イベント
	/// </summary>
	void OnApplicationQuit()
	{
		Debug.Log("ルームから退出しました");
		// ルーム退室  
		PhotonNetwork.LeaveRoom();
		// ネットワーク切断
		PhotonNetwork.Disconnect();
	}

	private void OnGUI()
	{
		var boxSize = new Vector2(400.0f, 120.0f);
		var rect = new Rect(new Vector2(Screen.width * 0.5f - boxSize.x * 0.5f, Screen.height * 0.8f - boxSize.y * 0.5f), boxSize);
		string message = 
			_messageMap[_currentState] + "\n" +
			"接続人数：" + PhotonNetwork.countOfPlayers.ToString() + "人";
		// UI表示
		GUI.Box(rect, message);
	}
}