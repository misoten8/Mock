using UnityEngine;
using WiimoteApi;

/// <summary>
/// Player クラス
/// 製作者：実川
/// </summary>
public class Player : MonoBehaviour
{
    //=======================================
    //構造体
    //=======================================
    public enum PLAYERMODE
    {
        NORMAL,
        MOVE,
        STAMINAOUT,
        BATTLE,
        END
    };

    public Define.PlayerType Type
	{
		get { return _type; }
	}

	public Color PlayerColor
	{
		get { return _playerColor; }
	}

	[SerializeField]
	private Color _playerColor;

	[SerializeField]
	private Define.PlayerType _type;

	[SerializeField]
	private Rigidbody _rb;

	[SerializeField]
	private float _power;

	[SerializeField]
	private float _rotatePower;

	[SerializeField]
	private Animator _animator;

	[SerializeField]
	private Dance _dance;

	[SerializeField]
	private PlayerManager _playerManager;

	// wiiリモコン
	private Wiimote _wm;
	private int _wmNum;

    //移動ゲージ
    private int gauge;

    //バトル
    public int battlegauge;

    //ゲージ減少する時間
    private const int REDUCTION_TIME = 1;//モードを変える時間
    private float changetime;   //動きを更新する時刻

    //バトルモード
    private const int BATTLE_TIME = 5;//モードを変える時間

    //移動モード
    private const int START_TIME = 10;
    private const int END_TIME = -1;//モードを変える時間

    //プレイヤーモード
    private PLAYERMODE mode;

    void Start()
	{
		// wiiリモコン初期化処理
		WiimoteManager.FindWiimotes();
		_wmNum = (int)_type - 1;
		if (WiimoteManager.HasWiimote(_wmNum))
		{
            _wm = WiimoteManager.Wiimotes[_wmNum];
            _wm.InitWiiMotionPlus();
			int i = _wmNum + 1;
			_wm.SendPlayerLED(i == 1, i == 2, i == 3, i == 4);
		}

        //ゲージ初期化
        gauge = 0;
        battlegauge = 0;
        //プレイヤーモード初期化
        mode = PLAYERMODE.NORMAL;
	}

	void Update()
	{
        //=======================================================
        //待機モード（Wiiリモコンを振って数値を10にする）
        //=======================================================
        if (mode == PLAYERMODE.NORMAL)
        {
            if (Input.GetKeyDown("return") || WiimoteManager.GetSwing(_wmNum))
            {
                gauge++;
                if (gauge >= START_TIME)
                {
                    mode = PLAYERMODE.MOVE;
                }
            }
        }

        if (mode == PLAYERMODE.MOVE )
        {
            //=============================================================================
            //ムーブモード(数値が０の時移動できなくする（０になるまで移動できる）
            //=============================================================================
            if (Input.GetKey("up") || WiimoteManager.GetButton(_wmNum, ButtonData.WMBUTTON_RIGHT))
                _rb.AddForce(transform.forward * _power);
            if (Input.GetKey("left") || WiimoteManager.GetButton(_wmNum, ButtonData.WMBUTTON_UP))
                transform.Rotate(Vector3.up, -_rotatePower);
            if (Input.GetKey("right") || WiimoteManager.GetButton(_wmNum, ButtonData.WMBUTTON_DOWN))
                transform.Rotate(Vector3.up, _rotatePower);
            if (Input.GetKey("down") || WiimoteManager.GetButton(_wmNum, ButtonData.WMBUTTON_LEFT))
                _rb.AddForce(-transform.forward * _power);

            //=====================================
            //時間経過で数値が０になるようにする
            //=====================================
            if (changetime < Time.time)
            {
                gauge--;                                  //ゲージ減少
                changetime = Time.time + REDUCTION_TIME;  //次の更新時刻を決める
                if (gauge <= END_TIME)
                {
                    mode = PLAYERMODE.NORMAL;
                }

            }
        }
        //=============================================================================
        //バトルモード
        //=============================================================================
        if (mode == PLAYERMODE.BATTLE )
        {
                if (Input.GetKeyDown("return") || WiimoteManager.GetSwing(_wmNum))
                {
                    battlegauge++;
                }

        }


    }
    private void OnGUI()
    {
        if (_wmNum == 0)
        {
            GUI.Label(new Rect(new Vector2(0, 12), new Vector2(300, 200)), "SUTAMINA : " + gauge.ToString());
            GUI.Label(new Rect(new Vector2(0, 24), new Vector2(300, 200)), "MODE     : " + mode.ToString());


        }
    }

    void OnApplicationQuit()
    {
		// 一括で実行させる
        if (WiimoteManager.Wiimotes.Count > 0)
        {
            _wm = WiimoteManager.Wiimotes[0];
            WiimoteManager.Cleanup(_wm);
            _wm = null;
            WiimoteManager.Wiimotes.Clear();
        }
    }

    public PLAYERMODE GetPlayerMode()
    {
        return mode;
    }
    public void SetPlayerMode(PLAYERMODE setmode)
    {
        mode = setmode;
    }
    //バトルが終わった時にする処理
    public void OnBattleEnd(bool iswin)
    {
        Debug.Log("onbattleend");
        //勝ってた場合
        if (iswin == true)
        {
            
            mode = PLAYERMODE.MOVE;
            return;
        }
        else
        {
            mode = PLAYERMODE.MOVE;
            gauge = 0;
        }
    }


}
