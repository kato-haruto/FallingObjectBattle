using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    [Header("�v���C���[�ݒ�")]
    public int playerNumber = 1; // 1 = 1P, 2 = 2P
    [Header("�u���b�N�ݒ�")]
    public GameObject blockPrefab;
    public Transform spawnPoint;
    private GameObject currentBlock;
    private Rigidbody rb;
    private bool canMove = true;
    [Header("�ړ��֘A")]
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
    void SpawnNewBlock()
    {
        currentBlock = Instantiate(blockPrefab, spawnPoint.position, Quaternion.identity);
        rb = currentBlock.GetComponent<Rigidbody>();
        rb.useGravity = false;
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
        currentBlock = null;
        Invoke(nameof(SpawnNewBlock), 1f); // �����x��Ď��𐶐�
    }
    // ���͏������v���C���[�ԍ��ŕ�����
    float GetHorizontalInput()
    {
        if (playerNumber == 1) 
            return Input.GetKey(KeyCode.A) ? -1 : Input.GetKey(KeyCode.D) ? 1 : 0; //1P�ړ�����
        else
            return Input.GetKey(KeyCode.LeftArrow) ? -1 : Input.GetKey(KeyCode.RightArrow) ? 1 : 0; //2P�ړ�����
    }
    bool GetDropInput()
    {
        if (playerNumber == 1)
            return Input.GetKeyDown(KeyCode.S); //1P��������
        else
            return Input.GetKeyDown(KeyCode.DownArrow); //2P��������
    }
}