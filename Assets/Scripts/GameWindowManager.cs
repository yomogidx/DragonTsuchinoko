using UnityEngine;

//--------------------------------------------------------------------
//  GameWindowManagerクラス
//  ゲームUI画面の制御
//--------------------------------------------------------------------
public class GameWindowManager : MonoBehaviour
{
    //--------------------------------------------------------------------
    //  フィールド
    //--------------------------------------------------------------------
    [Tooltip("「スタート！」ロゴ")]
    public RectTransform logoImage;
    [Tooltip("「スタート！」ロゴの移動速度")]
    public float scrollSpeed = -500f;
    [Tooltip("「スタート！」ロゴのスクロール最大位置")]
    public float targetX = -1000f;
    [Tooltip("「スタート！」ロゴのスクロール有無")]
    private bool isScrolling = true;

    //--------------------------------------------------------------------
    //  初期化処理
    //--------------------------------------------------------------------
    void Start()
    {
        // 初期位置を画面右外に設定
        logoImage.anchoredPosition = new Vector2(1000f, logoImage.anchoredPosition.y);
    }

    //--------------------------------------------------------------------
    //  更新処理
    //--------------------------------------------------------------------
    void Update()
    {
        if (isScrolling)
        {
            // 徐々に左へ移動
            float newX = Mathf.MoveTowards(logoImage.anchoredPosition.x, targetX, scrollSpeed * Time.deltaTime);
            logoImage.anchoredPosition = new Vector2(newX, logoImage.anchoredPosition.y);

            // 目標位置に到達したら止める
            if (Mathf.Approximately(newX, targetX))
            {
                isScrolling = false;
            }
        }
    }
}
