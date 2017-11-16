using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class cameramanager : MonoBehaviour {
    //=======================================
    //構造体
    //=======================================
    enum CAMERATYPE
    {
        PLAYER_CAMERA,
        DANCE_START,
        DANCE_0,
        DANCE_1,
        DANCE_2,
        DANCE_3;
    };
    //=======================================
    //グローバル変数
    //=======================================
    private unitychandemo1   player;
    private playercamera playercamera;
    [SerializeField] private DanceCamera[]  dancecamera =  new DanceCamera[4]; 
    public const int PRIORITY_HIGH      = 15; // 優先度 高
    public const int PRIORITY_LOW       = PRIORITY_HIGH - 1; // HIGHより低ければなんでもOK
    private int CAMERA_MAX  = 4; //カメラの最大数
    private int CHANGE_TIME = 4;//モードを変える時間
    private float cnt;
    private float changetime;   //動きを更新する時刻
    private CinemachineBrain brain;
    //=======================================
    //関数名 Start
    //引き数
    //戻り値
    //=======================================
    void Start ()
    {
        player         = GameObject.Find("Player").GetComponent<unitychandemo1>();
        playercamera   = GameObject.Find("CM Player").GetComponent<playercamera>();
        dancecamera[0] = GameObject.Find("CM DanceCamera1").GetComponent<DanceCamera>();
        dancecamera[1] = GameObject.Find("CM DanceCamera2").GetComponent<DanceCamera>();
        dancecamera[2] = GameObject.Find("CM DanceCamera3").GetComponent<DanceCamera>();
        dancecamera[3] = GameObject.Find("CM DanceCamera4").GetComponent<DanceCamera>();
    }
    //=======================================
    //関数名 Update
    //引き数
    //戻り値
    //=======================================
    void Update()
    {

        unitychandemo1.Mode mode = player.GetMode();
        switch (mode)
        {
            case unitychandemo1.Mode.NORMAL:
                Setblend(1);
                playercamera.SetPriority(PRIORITY_HIGH);
                dancecamera[0].SetPriority(PRIORITY_LOW);
                break;
            case unitychandemo1.Mode.DANCEINTRO:
                SetCameraPriority(0);
                playercamera.SetPriority(PRIORITY_LOW);
                //一定時間経過でランダム
                if (changetime < Time.time)
                {
                    player.SetMode(unitychandemo1.Mode.DANCE);
                }
                changetime = Time.time + CHANGE_TIME;  //次の更新時刻を決める
                break;
            case unitychandemo1.Mode.DANCE:
                if (changetime < Time.time)
                {
                    Setblend(0);
                    int random = Random.Range(0, 4);
                    Debug.Log(random);
                    SetCameraPriority(random);
                    changetime = Time.time + CHANGE_TIME;  //次の更新時刻を決める
                }
                break;
        }
    }
    //=======================================
    //関数名 Update
    //引き数
    //戻り値
    //=======================================
    void Setblend(int num)
    {
        brain = FindObjectOfType<CinemachineBrain>();
        brain.m_DefaultBlend.m_Time = num; // 0 Time equals a cut
    }
    //=======================================
    //関数名 SetCameraPriority
    //引き数
    //戻り値
    //=======================================
    void SetCameraPriority(int num)
    {
        for (int i = 0; i < CAMERA_MAX; i++)
        {
            dancecamera[i].SetPriority(PRIORITY_LOW);
        }
        dancecamera[num].SetPriority(PRIORITY_HIGH);
    }
}
