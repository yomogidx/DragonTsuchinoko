using UnityEngine;
using System.Collections;
using System.Collections.Generic;

//--------------------------------------------------------------------
//  EnemyManagerクラス
//  敵(ほこり)の制御処理
//--------------------------------------------------------------------
public class EnemyManager : MonoBehaviour
{
    //--------------------------------------------------------------------
    //  フィールド
    //--------------------------------------------------------------------
    [Header("ゲームオブジェクト")]
    [Tooltip("敵オブジェクト")]
    public GameObject Enemy;
    [Tooltip("敵の生成位置を表すオブジェクト")]
    public GameObject EnemyPlace;

    [Header("制御用")]
    [Tooltip("前回敵の生成してからの経過時間")]
    float SpawnTimer;
    [Tooltip("ゲーム全体の経過時間")]
    float TotalTimer;
    [Tooltip("生成時間間隔（前回敵を生成してから生成するまでの秒数）")]
    float SpawnRange;

    //--------------------------------------------------------------------
    //  初期化処理
    //--------------------------------------------------------------------
    void Start()
    {
        // 生成時間間隔を初期化
        SpawnTimer = 0.0f;
        // 生成時間間隔
        SpawnRange = 3.0f;
    }

    //--------------------------------------------------------------------
    //  更新処理
    //--------------------------------------------------------------------
    void Update()
    {
        //----- ゲームプレイ中でなければ更新しない
        if (GameManager.Instance.GetGamePlayflag() == false) { return; }
        if (GameManager.Instance.GetGameResultflag() == true) { return; }

        //----- 更新
        SpawnTimer += Time.deltaTime;
        TotalTimer += Time.deltaTime;

        //----- 敵の生成
        if (SpawnTimer > SpawnRange) 
        {
            Spawn();
        }

        //----- 敵の生成速度をゲームの経過時間に合わせて変更
        if (TotalTimer >= 55) {
            SpawnRange = 0.1f;
        }
        else if (TotalTimer >= 50) {
            SpawnRange = 0.5f;
        }
        else if (TotalTimer >= 40) {
            SpawnRange = 1.0f;
        }
        else if (TotalTimer >= 30) {
            SpawnRange = 1.5f;
        }
        else if (TotalTimer >= 20) {
            SpawnRange = 2.0f;
        }
        else if (TotalTimer >= 10) {
            SpawnRange = 2.5f;
        }
    }

    //--------------------------------------------------------------------
    //  Enemyの生成
    //--------------------------------------------------------------------
    void Spawn()
    {
        // EnemyPlaceの範囲内でランダムな位置を取得
        BoxCollider box = EnemyPlace.GetComponent<BoxCollider>();
        Vector3 center = box.center + EnemyPlace.transform.position;
        Vector3 size = EnemyPlace.transform.localScale;
        float x = Random.Range(center.x - size.x / 2, center.x + size.x / 2);
        float y = Random.Range(center.y - size.y / 2, center.y + size.y / 2);
        float z = Random.Range(center.z - size.z / 2, center.z + size.z / 2);
        Vector3 spawnPosition = new Vector3(x, y, z);

        // Enemyを生成
        Instantiate(Enemy, spawnPosition, Quaternion.AngleAxis(90, Vector3.up));

        // 生成時間間隔を初期化
        SpawnTimer = 0;
    }
}
