using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    [Header("プレイヤー設定")]
    public int playerNumber = 1; // 1 = 1P, 2 = 2P

    [Header("ブロック設定")]
    public GameObject blockPrefab;
    public Transform spawnPoint;
    private GameObject currentBlock;
    private Rigidbody rb;
    private bool canMove = true;

    [Header("移動関連")]
    public float moveSpeed = 5f;
    public float moveLimitX = 5f;

    void Start()
    {
        SpawnNewBlock();
    }

    void Update()
    {
        if (currentBlock == null) return;

        if (canMove)
        {
            HandleMovement();

            if (GetDropInput())
            {
                DropBlock();
            }
        }
    }

    // --- ブロック生成 ---
    public void SpawnNewBlock() // ★public に変更（Block から呼ばれるので）
    {
        currentBlock = Instantiate(blockPrefab, spawnPoint.position, Quaternion.identity);

        // Rigidbodyを取得
        rb = currentBlock.GetComponent<Rigidbody>();
        rb.useGravity = false;

        // Blockスクリプトを取得してプレイヤー番号をセット
        Block block = currentBlock.GetComponent<Block>();
        block.playerNumber = playerNumber;

        canMove = true;
    }

    void HandleMovement()
    {
        float horizontal = GetHorizontalInput();
        Vector3 pos = currentBlock.transform.position;
        pos.x += horizontal * moveSpeed * Time.deltaTime;
        pos.x = Mathf.Clamp(pos.x, spawnPoint.position.x - moveLimitX, spawnPoint.position.x + moveLimitX);
        currentBlock.transform.position = pos;
    }

    void DropBlock()
    {
        canMove = false;
        rb.useGravity = true;

        // Blockに「落下開始」を通知（自分を渡す）
        Block block = currentBlock.GetComponent<Block>();
        block.OnDropped(this);

        currentBlock = null;
    }

    float GetHorizontalInput()
    {
        if (playerNumber == 1)
            return Input.GetKey(KeyCode.A) ? -1 : Input.GetKey(KeyCode.D) ? 1 : 0; //1P移動操作
        else
            return Input.GetKey(KeyCode.LeftArrow) ? -1 : Input.GetKey(KeyCode.RightArrow) ? 1 : 0; //2P移動操作
    }

    bool GetDropInput()
    {
        if (playerNumber == 1)
            return Input.GetKeyDown(KeyCode.S); //1P落下操作
        else
            return Input.GetKeyDown(KeyCode.DownArrow); //2P落下操作
    }
}
