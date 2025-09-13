using UnityEngine;

public class Block : MonoBehaviour
{
    public int playerNumber; // どのプレイヤーのブロックか識別用
    private bool scored = false; // スコア済みフラグ
    private Rigidbody rb;
    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }
    private void Update()
    {
        // フィールド外に落下した場合
        if (transform.position.y < -3f && scored)
        {
            GameManager.Instance.RemoveScore(playerNumber); // 減点
            scored = false;
            Destroy(gameObject);
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (!scored)
        {
            GameManager.Instance.AddScore(playerNumber); // 設置で加点
            scored = true;
        }
    }
}