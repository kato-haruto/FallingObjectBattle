using UnityEngine;

public class Block : MonoBehaviour
{
    public int playerNumber; // プレイヤー識別
    private bool scored = false; // スコア処理済みフラグ
    private Rigidbody rb;

    private float dropTime;        // 落下開始時刻
    private bool isDropped = false; // 落下開始したかどうか

    [Header("設置判定設定")]
    public float stableVelocity = 0.1f; // 速度がこの値以下で停止とみなす
    public float maxWaitTime = 3f;      // 強制的に設置とする時間
    public float minDropTime = 0.3f;    // 落下直後に設置判定しない猶予時間
    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }
    private void Update()
    {
        // --- フィールド外 ---
        if (transform.position.y < -5f)
        {
            if (scored)
            {
                GameManager.Instance.RemoveScore(playerNumber);
                scored = false;
            }
            Destroy(gameObject);
            return;
        }
        // --- 設置判定 ---
        if (isDropped && !scored)
        {
            float elapsed = Time.time - dropTime;
            // 最低猶予時間を超えて、速度が小さい場合
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
    public void OnDropped() // PlayerManager から呼ばれる（落下開始時）
    {
        isDropped = true;
        dropTime = Time.time;
    }
    private void ScoreAndFix()
    {
        GameManager.Instance.AddScore(playerNumber);
        scored = true;
    }
}