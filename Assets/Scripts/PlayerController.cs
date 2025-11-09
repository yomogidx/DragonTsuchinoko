using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.EnhancedTouch;
using Touch = UnityEngine.InputSystem.EnhancedTouch.Touch;

//--------------------------------------------------------------------
//  PlayerControllerクラス
//  プレイヤー（つちのこ）の制御処理
//--------------------------------------------------------------------
public class PlayerController : MonoBehaviour
{
    //--------------------------------------------------------------------
    // フィールド
    //--------------------------------------------------------------------
    [Tooltip("プレイヤーの移動速度")]
    public float PlayerSpeed;   
    [Tooltip("目標の角度")]
    private Quaternion targetRotation;
    [Tooltip("回転速度（度/秒）")]
    public float rotationSpeed = 1000;
    [Tooltip("アニメーション")]
    public Animator PlayerAnimator;
    [Tooltip("ファイアー（つちのこが吐く炎）")]
    public GameObject FirePrefab;
    [Tooltip("プレイヤーのリジッドボディ")]
    private Rigidbody rb;
    [Tooltip("毎フレームごとの移動済管理フラグ")]
    private bool MoveFrameFlag;
    [Tooltip("カメラオブジェクト")]
    public GameObject Gamera;

    //--------------------------------------------------------------------
    // 初期化処理
    //--------------------------------------------------------------------
    void Start()
    {
        // 初期回転を設定
        targetRotation = transform.rotation;
        // プレイヤーのリジッドボディ
        rb = GetComponent<Rigidbody>();
    }

    //--------------------------------------------------------------------
    // 更新処理
    //--------------------------------------------------------------------
    void Update()
    {
        //----- ゲームプレイ中でなければ更新しない
        if (GameManager.Instance.GetGamePlayflag() == false) { return; }

        //----- フレームごとの処理
        MoveFrameFlag = false;

        //----- キャラクターの更新
        UpdateCharacter();
    
        //----- 入力操作
        // キーボード入力操作
        KeyInputprocess();
        // マウス入力操作
        MouseInputprocess();
    }
    //--------------------------------------------------------------------
    //  キャラクターの更新
    //--------------------------------------------------------------------
    void UpdateCharacter()
    {

    }

    //--------------------------------------------------------------------
    //  ゲームスタート
    //--------------------------------------------------------------------
    public void GameStart()
    {
        FirePrefab.SetActive(true);
    }

    //--------------------------------------------------------------------
    //  キーボード入力操作
    //--------------------------------------------------------------------
    void KeyInputprocess()
    {
        // 現在のキーボード情報
        var current = Keyboard.current;
        // キーボード接続チェック
        if (current == null) { return; }
        // キャラクターの移動
        var rot = Vector3.zero;
        var dz = 1;
        var dx = 1;
        int[] degrees = {315,   0,  45,
                         270,  -1,  90,
                         225, 180, 135};
        //----- キー入力
        if (current[Key.W].isPressed) { dx += -1; }
        if (current[Key.A].isPressed) { dz += 1; }
        if (current[Key.S].isPressed) { dx += 1; }
        if (current[Key.D].isPressed) { dz += -1; }
        int degree = degrees[dz * 3 + dx];
        MoveDegree(degree);
    }

    //--------------------------------------------------------------------
    //  移動
    //--------------------------------------------------------------------
    void MoveDegree(int degree)
    {
        if (degree == -1) { return; }
        if (MoveFrameFlag) { return; }
    
        //----- 移動
        var speed = Vector3.zero;
        speed.z = PlayerSpeed * Time.deltaTime;
        transform.Translate(speed);
        //Vector3 newPosition = new Vector3(rb.position.x, rb.position.y, rb.position.z + speed.z);
        //rb.MovePosition(newPosition);
        //Vector3 localSpeed = Vector3.forward * PlayerSpeed * Time.fixedDeltaTime;
        //Vector3 worldSpeed = transform.TransformDirection(localSpeed);
        //Vector3 newPosition = rb.position + worldSpeed;
        //rb.MovePosition(newPosition);

        //----- 回転（目標角度を設定）
        targetRotation = Quaternion.Euler(0, degree, 0);

        //----- ゆっくり回転
        transform.rotation = Quaternion.RotateTowards(
            transform.rotation,
            targetRotation,
            rotationSpeed * Time.deltaTime
        );

        //----- 移動済フラグの更新
        MoveFrameFlag = true;
    }

    //--------------------------------------------------------------------
    //  マウス入力操作
    //--------------------------------------------------------------------
    void MouseInputprocess()
    {
        Vector2 position = Vector2.zero;

        // スマホのタッチ座標取得
        if (Touch.activeTouches.Count > 0)
        {
            foreach (var touch in Touch.activeTouches)
            {
                position = touch.screenPosition;
            }
        }
        // マウスのタッチ座標取得
        else if (Mouse.current != null && Mouse.current.leftButton.isPressed)
        {
            position = Mouse.current.position.ReadValue();
        }

        // 移動
        if (position != Vector2.zero)
        {
            Vector3 screenPosition = Camera.main.WorldToScreenPoint(transform.position);
            //Vector2 screenCenter = new Vector2(Screen.width / 2f, Screen.height / 2f);
            Vector2 screenCenter = new Vector2(screenPosition.x, screenPosition.y);
            Vector2 direction = (position - screenCenter).normalized;
            int degree = GetDirection8(direction);
            MoveDegree(degree);
        }
    }

    //--------------------------------------------------------------------
    // 初期化処理（タッチ）
    //--------------------------------------------------------------------
    void OnEnable()
    {
        EnhancedTouchSupport.Enable();
        //TouchSimulation.Enable();
        //UnityEngine.InputSystem.EnhancedTouch.Touch.onFingerDown += OnFingerDown;
    }

    //--------------------------------------------------------------------
    // 終了処理（タッチ）
    //--------------------------------------------------------------------
    void OnDisable()
    {
        //UnityEngine.InputSystem.EnhancedTouch.Touch.onFingerDown -= OnFingerDown;
        //TouchSimulation.Disable();
        EnhancedTouchSupport.Disable();
    }

    //--------------------------------------------------------------------
    // タッチ操作
    //--------------------------------------------------------------------
    void OnFingerDown(Finger finger)
    {
        Vector2 screenCenter = new Vector2(Screen.width / 2f, Screen.height / 2f);
        Vector2 touchPos = finger.screenPosition;
        Vector2 direction = (touchPos - screenCenter).normalized;

        int degree = GetDirection8(direction);
        MoveDegree(degree);
    }

    //--------------------------------------------------------------------
    // タッチ方向の取得
    //--------------------------------------------------------------------
    int GetDirection8(Vector2 dir)
    {
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        angle = (angle + 360f) % 360f;
        return (int)angle * -1;
        /*
        if (angle >= 337.5f || angle < 22.5f) return 90;   //"右";
        if (angle >= 22.5f && angle < 67.5f) return 45;    //"右上";
        if (angle >= 67.5f && angle < 112.5f) return 0;    //"上";
        if (angle >= 112.5f && angle < 157.5f) return 315; //"左上";
        if (angle >= 157.5f && angle < 202.5f) return 270; //"左";
        if (angle >= 202.5f && angle < 247.5f) return 225; //"左下";
        if (angle >= 247.5f && angle < 292.5f) return 180; //"下";
        if (angle >= 292.5f && angle < 337.5f) return 135; //"右下";
        return -1; // 該当なし
        */
        //int[] degrees = {315,   0,  45,
        //                 270,  -1,  90,
        //                 225, 180, 135};
    }
}
