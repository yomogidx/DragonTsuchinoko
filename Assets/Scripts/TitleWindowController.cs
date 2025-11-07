using UnityEngine;

//--------------------------------------------------------------------
//  TitleWindowControllerクラス
//  タイトル画面の制御
//--------------------------------------------------------------------
public class TitleWindowController : MonoBehaviour
{
    //--------------------------------------------------------------------
    // フィールド
    //--------------------------------------------------------------------
    [Header("上下移動用")]
    [Tooltip("タイトルロゴ")]
    public RectTransform logoImage;
    [Tooltip("上下の動く速さ")]
    public float floatSpeed = 1.0f;
    [Tooltip("上下の振れ幅")]
    public float floatHeight = 20.0f;
    [Tooltip("タイトルロゴの初期位置")]
    private Vector2 startPos;

    //--------------------------------------------------------------------
    // 初期化処理
    //--------------------------------------------------------------------
    void Start()
    {
        // タイトルロゴの初期位置を設定
        startPos = logoImage.anchoredPosition;
    }

    //--------------------------------------------------------------------
    // 更新処理
    //--------------------------------------------------------------------
    void Update()
    {
        // タイトルロゴを上下に浮遊
        float newY = startPos.y + Mathf.Sin(Time.time * floatSpeed) * floatHeight;
        logoImage.anchoredPosition = new Vector2(startPos.x, newY);
    }
}
