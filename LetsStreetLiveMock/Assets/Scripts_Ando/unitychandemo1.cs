using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class unitychandemo1 : MonoBehaviour
{
    //=========================================
    //構造体
    //=========================================
    public enum Mode
    {
        NORMAL     = 0,//通常状態
        DANCEINTRO = 1,
        DANCE      = 2, //ダンスモード
        END        = 3
    };
    //=========================================
    //グローバル変数
    //=========================================
    private Animator animator;
    private bool Active;//updateの有効化on/off
    private Mode g_Mode;

    //=========================================
    //関数名 Start
    //引き数
    //戻り値
    //=========================================
    void Start()
    {
        g_Mode     = Mode.NORMAL;
        Active     = true;
        animator   = GetComponent<Animator>();
    
    }

    //=========================================
    //関数名 Update
    //引き数
    //戻り値
    //=========================================
    void Update()
    {
        if (Active == true && Input.GetKey("up") || Active == true && Input.GetKey("down"))
        {
            animator.SetBool("isrunning", true);
        }
        else
        {
            animator.SetBool("isrunning", false);
        }

    }
    //================================================
    //関数名:SetDancing
    //引き数:なし
    //戻り値:なし
    //================================================
    //通常状態ならダンス開始、ダンス中ならダンス終了
    //================================================
    public void SetDancing()
    {
        if (!animator.GetBool("isdancing"))
        {
            animator.SetBool("isdancing", true);
            Debug.Log("ダンス開始");
            g_Mode = Mode.DANCEINTRO;
            return;
        }
        else
        {
            animator.SetBool("isdancing", false);
            Debug.Log("ダンス終了");
            g_Mode = Mode.NORMAL;
            return;
        }
    }

    //================================================
    //関数名:GetMode
    //引き数:なし
    //戻り値:現在のモード状態
    //================================================
    //プレイヤが通常状態かダンス状態か取得
    //================================================
    public Mode GetMode()
    {
        return g_Mode;
    }
    //================================================
    //関数名:SetMode
    //引き数:セットしたいモード
    //戻り値:なし
    //================================================
    //プレイヤが通常状態かダンス状態か取得
    //================================================
    public void SetMode(Mode mode)
    {
        g_Mode = mode;
    }
}