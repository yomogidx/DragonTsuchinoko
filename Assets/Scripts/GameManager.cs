using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using unityroom.Api;

//--------------------------------------------------------------------
//  GameManagerクラス
//  ゲームの全体制御処理
//--------------------------------------------------------------------
public class GameManager : MonoBehaviour
{
    //--------------------------------------------------------------------
    //  シングルトン
    //--------------------------------------------------------------------
    public static GameManager Instance { get; private set; }

    //--------------------------------------------------------------------
    //  フィールド
    //--------------------------------------------------------------------
    [Header("ゲーム制御用のフラグ")]
    [Tooltip("ゲーム実行中かを表すフラグ")]
    private bool GamePlayflag;
    [Tooltip("リザルト画面の表示フラグ")]
    private bool GameResultflag;

    [Header("UIオブジェクト")]
    [Tooltip("リザルト画面のUIオブジェクト")]
    public GameObject ResultWindow;
    [Tooltip("タイトル画面のUIオブジェクト")]
    public GameObject TitleWindow;
    [Tooltip("ゲーム画面のUIオブジェクト")]
    public GameObject GameWindow;
    [Tooltip("リザルト画面のスコア表示")]
    public TextMeshProUGUI ResultScoreText;

    [Header("ゲームオブジェクト")]
    [Tooltip("プレイヤーのゲームオブジェクト")]
    public GameObject Player;
    [Tooltip("スコアのゲームオブジェクト")]
    public GameObject Score;

    [Header("効果音")]
    [Tooltip("効果音（ゲームスタート）")]
    public AudioSource AudioGameStart;
    [Tooltip("効果音（ゲームストップ）")]
    public AudioSource AudioGameStop;

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
        // ゲーム終了時に結果画面を表示
        if (GameResultflag == true)
        {
            ResultWindow.SetActive(true);
        }
    }

    //--------------------------------------------------------------------
    //  初期化処理（ゲームオブジェクトがアクティブになったとき）
    //--------------------------------------------------------------------
    void Awake()
    {
        // シングルトンの初期化
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject); // 重複を防ぐ
        }
        else
        {
            Instance = this;
            GamePlayflag = false;
            GameResultflag = false;
            //ResultWindow = GameObject.Find("ResultWindow");
            //DontDestroyOnLoad(gameObject); // シーンをまたいでも保持したい場合
        }
    }
    //--------------------------------------------------------------------
    //  ゲームスタート
    //--------------------------------------------------------------------
    public void GameStart()
    {
        TitleWindow.SetActive(false);
        GameWindow.SetActive(true);
        GamePlayflag = true;
        GameResultflag = false;

        PlayerController controller = Player.GetComponent<PlayerController>();
        controller.GameStart();

        AudioGameStart.PlayOneShot(AudioGameStart.clip);
    }

    //--------------------------------------------------------------------
    //  ゲームリスタート
    //--------------------------------------------------------------------
    public void GameRestart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    //--------------------------------------------------------------------
    //  ゲームストップ
    //--------------------------------------------------------------------
    public void GameStop()
    {
        AudioGameStop.PlayOneShot(AudioGameStop.clip);
        SetGameResultflag(true);
        GameWindow.SetActive(false);

        // スコアの設定
        GameScore controller = Score.GetComponent<GameScore>();
        int score = controller.GetScore();
        ResultScoreText.text = "Score:" + ((int)score).ToString();

        // unityroomにスコアの送信
        UnityroomApiClient.Instance.SendScore(1, score, ScoreboardWriteMode.HighScoreDesc);
    }

    //--------------------------------------------------------------------
    //  getter
    //--------------------------------------------------------------------
    public bool GetGamePlayflag() { return GamePlayflag; }
    public bool GetGameResultflag() { return GameResultflag; }
    //--------------------------------------------------------------------
    //  setter
    //--------------------------------------------------------------------
    public void SetGamePlayflag(bool flag) { GamePlayflag = flag; }
    public void SetGameResultflag(bool flag) { GameResultflag = flag; }
}
