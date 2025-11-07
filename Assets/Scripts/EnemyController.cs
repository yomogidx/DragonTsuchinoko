using UnityEngine;

//--------------------------------------------------------------------
//  EnemyControllerクラス
//  敵(ほこり)の制御処理
//--------------------------------------------------------------------
public class EnemyController : MonoBehaviour
{
    //--------------------------------------------------------------------
    //  フィールド
    //--------------------------------------------------------------------
    [Header("上下移動用（浮遊アニメーション用）")]
    [Tooltip("上下の振れ幅")]
    public float Amplitude = 0.5f;
    [Tooltip("上下の動く速さ")]
    public float Frequency = 1f;
    [Tooltip("初期位置")]
    private Vector3 StartPos;

    [Header("徘徊用")]
    [Tooltip("移動速度")]
    public float MoveSpeed = 20f;
    [Tooltip("方向を変える間隔（秒）")]
    public float ChangeDirectionTime = 2f;
    [Tooltip("移動方向")]
    private Vector3 MoveDirection;
    [Tooltip("移動用タイマー")]
    private float MoveTimer;

    [Header("ステータス")]
    [Tooltip("ヒットポイント（HP）")]
    public int HP = 10;

    [Header("死亡エフェクト")]
    [Tooltip("死亡エフェクトのオブジェクト")]
    public GameObject DeathEffect;

    //--------------------------------------------------------------------
    //  初期処理
    //--------------------------------------------------------------------
    void Start()
    {
        //----- 上下移動用に初期位置の設定
        StartPos = transform.position;
        //----- 移動方向の設定
        ChooseNewDirection();
    }

    //--------------------------------------------------------------------
    //  更新処理
    //--------------------------------------------------------------------
    void Update()
    {
        //----- 上下移動
        Vector3 pos = transform.position;
        float yOffset = Mathf.Sin(Time.time * Frequency) * Amplitude;
        // ※Time.time…ゲーム開始時から現在までの経過時間（秒）
        pos.y = StartPos.y + yOffset;
        transform.position = pos;

        // 一定時間ごとに方向を変える
        MoveTimer += Time.deltaTime;
        if (MoveTimer >= ChangeDirectionTime)
        {
            ChooseNewDirection();
            MoveTimer = 0f;
        }
        // 選ばれた方向に移動
        transform.Translate(MoveDirection * MoveSpeed * Time.deltaTime);
    }

    //--------------------------------------------------------------------
    //  移動方向の設定
    //--------------------------------------------------------------------
    void ChooseNewDirection()
    {
        float angle = Random.Range(0f, 360f);   // 角度をランダムに設定
        float rad = angle * Mathf.Deg2Rad;      // 角度をラジアンに変換
        MoveDirection = new Vector3(Mathf.Cos(rad), 0f, Mathf.Sin(rad)).normalized;  // X軸とZ軸の移動距離を設定
        //Debug.Log(MoveDirection);
    }

    //--------------------------------------------------------------------
    //  パーティクルとの当たり判定
    //--------------------------------------------------------------------
    void OnParticleCollision(GameObject obj) 
    {
        //----- ダメージ処理
        HP--;

        //----- 死亡判定（HPが0以下なら死亡判定）
        if (HP <= 0)
        {
            HP = 0;
            //AudioSource.PlayOneShot(AudioClipDeath);
            // 死亡エフェクトの作成
            Instantiate(DeathEffect, transform.position, Quaternion.identity);
            // 敵オブジェクトの削除
            Destroy(this.gameObject);
            // スコアの加算
            GameScore.Instance.AddScore(100);
        }
    }
}
