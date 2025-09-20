using UnityEngine;

public class Block : MonoBehaviour
{
    public int playerNumber; // プレイヤー識別
    private bool scored = false;        // スコア処理済みフラグ
    private bool nextBlockSpawned = false; // 次のブロック生成済みフラグ

    private Rigidbody rb;
    private PlayerManager owner;      // 自分を落としたプレイヤー

    private float dropTime;        // 落下開始時刻
    private bool isDropped = false; // 落下開始したかどうか
    private bool finalized = false; // ★追加: このブロックの終了処理が完全に終わったか

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
        if (finalized) return; // 完全に終了したら処理しない

        // --- フィールド外に落下 ---
        if (transform.position.y < -5f)
        {
            if (scored) // ★設置後に落ちた → 減点
            {
                GameManager.Instance.RemoveScore(playerNumber);
                scored = false;
            }

            EndBlock(false); // ブロック削除
            return;
        }

        // --- 設置判定 ---
        if (isDropped && !scored)
        {
            float elapsed = Time.time - dropTime;
            if (elapsed >= minDropTime && rb.velocity.magnitude < stableVelocity)
            {
                ScoreAndFix();
            }
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
        owner = player;
    }

    private void ScoreAndFix()
    {
        if (scored) return; // 二重防止

        GameManager.Instance.AddScore(playerNumber); // 加点
        scored = true;

        EndBlock(true); // 設置扱い
    }

    /// <summary>
    /// 設置 or 落下で終了処理
    /// </summary>
    /// <param name="keepBlock">trueならブロックを残す / falseなら削除</param>
    private void EndBlock(bool keepBlock)
    {
        // --- 次のブロック生成は一度だけ ---
        if (!nextBlockSpawned && owner != null)
        {
            owner.SpawnNewBlock();
            nextBlockSpawned = true;
        }

        if (!keepBlock)
        {
            Destroy(gameObject, 0.1f); // 落下時のみ削除
            finalized = true; // 完全終了
        }
        // keepBlock==true の場合は残す → まだ落下チェックは動く
    }
}
