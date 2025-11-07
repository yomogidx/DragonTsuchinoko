using UnityEngine;
using UnityEngine.UI;
using TMPro;

//--------------------------------------------------------------------
//  GameTimerクラス
//  タイマー表示の更新処理
//--------------------------------------------------------------------
public class GameTimer : MonoBehaviour
{
    //--------------------------------------------------------------------
    //  フィールド
    //--------------------------------------------------------------------
    [Tooltip("タイマー")]
    public float Timer = 60;
    public TextMeshProUGUI TimerText;
    public GameObject ResultWindow;
    public GameObject FirePrefab;  // ファイアー

    //--------------------------------------------------------------------
    //  初期化処理
    //--------------------------------------------------------------------
    void Start()
    {
        
    }

    //--------------------------------------------------------------------
    //  更新処理
    //--------------------------------------------------------------------
    void Update()
    {
        //----- ゲームプレイ中でなければ更新しない
        if (GameManager.Instance.GetGamePlayflag() == false) { return; }
        if (GameManager.Instance.GetGameResultflag() == true) { return; }

        if (Timer > 0)
        {
            Timer -= Time.deltaTime;
        }
        else
        {
            //----- ゲーム終了
            Timer = 0;
            //ResultWindow.SetActive(true);
            FirePrefab.SetActive(false);
            GameManager.Instance.GameStop();
        }
        TimerText.text = ((int)Timer).ToString();
    }
}
