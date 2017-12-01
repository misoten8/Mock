using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// PlayerGenerator クラス
/// プレイヤーの生成を行う
/// </summary>
public class PlayerGenerator : Photon.MonoBehaviour 
{
	/// <summary>
	///　初期化時に渡すキャッシュクラス
	/// </summary>
	public struct PlayerCaches
	{
		public MobManager mobManager;
		public PlayerManager playerManager;
		public int instanceID;

		public PlayerCaches(MobManager MobManager, PlayerManager PlayerManager, int InstanceID)
		{
			mobManager = MobManager;
			playerManager = PlayerManager;
			instanceID = InstanceID;
		}
	}
}