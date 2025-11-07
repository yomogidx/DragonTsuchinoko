using UnityEngine;

//--------------------------------------------------------------------
//  CameraControllerクラス
//  カメラの挙動を制御
//--------------------------------------------------------------------
public class CameraController : MonoBehaviour
{
    //--------------------------------------------------------------------
    //  フィールド
    //--------------------------------------------------------------------
    [Tooltip("カメラの目標座標（現在位置から滑らかに移動する）")]
    public Vector3 targetPosition = new Vector3(21.77f, 14.45f, 0.28f);
    [Tooltip("カメラの目標角度（現在角度から滑らかに回転する）")]
    public Vector3 targetEulerAngles = new Vector3(38.125f, -90f, 0f);
    [Tooltip("カメラの目標角度")]
    private Quaternion targetRotation;
    [Tooltip("カメラの移動速度")]
    public float moveSpeed = 1.0f;

    //--------------------------------------------------------------------
    //  初期化処理
    //--------------------------------------------------------------------
    void Start()
    {
        // 初期位置の設定
        targetRotation = Quaternion.Euler(targetEulerAngles);
    }

    //--------------------------------------------------------------------
    //  更新処理
    //--------------------------------------------------------------------
    void Update()
    {
        //----- ゲームプレイ中ならカメラ移動
        if (GameManager.Instance.GetGamePlayflag())
        {
            //----- カメラを滑らかに更新
            // 表示位置
            transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime * moveSpeed);
            // 回転
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * moveSpeed);
        }
    }
}
