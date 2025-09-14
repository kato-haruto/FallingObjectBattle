using UnityEngine;

public class Block : MonoBehaviour
{
    public int playerNumber; // プレイヤー識別
    private bool scored = false;      // スコア処理済みフラグ
    private bool processed = false;   // 設置/落下処理済みフラグ
    private bool nextBlockSpawned = false; // ★追加：次のブロック生成済みフラグ

    private Rigidbody rb;
    private PlayerManager owner;      // 自分を落としたプレイヤー

    private float dropTime;        // 落下開始時刻
    private bool isDropped = false; // 落下開始したかどうか

    [Header("設置判定設定")]
    public float stableVelocity = 0.1f; // 停止判定の速度
    public float maxWaitTime = 3f;      // 強制的に設置とする時間
    public float minDropTime = 0.3f;    // 落下直後に設置判定しない猶予時間

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        if (processed) return; // --- 既に設置/落下処理済みなら無視 ---

        // --- フィールド外に落下 ---
        if (transform.position.y < -5f)
        {
            if (scored) // 設置後に落ちた場合は減点
            {
                GameManager.Instance.RemoveScore(playerNumber);
                scored = false;
            }
            ProcessedEnd(false); // 落下処理（ブロック削除あり）
            return;
        }

        // --- 設置判定 ---
        if (isDropped && !scored)
        {
            float elapsed = Time.time - dropTime;
            // 一定時間経過 & 停止
            if (elapsed >= minDropTime && rb.velocity.magnitude < stableVelocity)
            {
                ScoreAndFix();
            }
            // 強制設置
            else if (elapsed >= maxWaitTime)
            {
                ScoreAndFix();
            }
        }
    }

    public void OnDropped(PlayerManager player) // PlayerManager から呼ばれる（落下開始時）
    {
        isDropped = true;
        dropTime = Time.time;
        owner = player; // 自分を管理しているプレイヤーを記録
    }

    private void ScoreAndFix()
    {
        if (processed) return; // 二重防止
        GameManager.Instance.AddScore(playerNumber);
        scored = true;

        ProcessedEnd(true); // 設置処理（ブロック残す）
    }

    /// <summary>
    /// 設置 or 落下で終了処理
    /// </summary>
    /// <param name="keepBlock">trueならブロックを残す / falseなら削除</param>
    private void ProcessedEnd(bool keepBlock)
    {
        if (processed) return;
        processed = true;

        // --- 次のブロック生成は一度だけ ---
        if (!nextBlockSpawned && owner != null)
        {
            owner.SpawnNewBlock();
            nextBlockSpawned = true;
        }

        if (!keepBlock)
        {
            Destroy(gameObject, 0.1f); // 落下時のみ削除
        }
        // keepBlock==true の場合は何もせず残す（物理挙動も保持）
    }
}
