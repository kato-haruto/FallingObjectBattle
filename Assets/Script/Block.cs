using UnityEngine;

public class Block : MonoBehaviour
{
    public int playerNumber; // �ǂ̃v���C���[�̃u���b�N�����ʗp
    private bool scored = false; // �X�R�A�ς݃t���O
    private Rigidbody rb;
    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }
    private void Update()
    {
        // �t�B�[���h�O�ɗ��������ꍇ
        if (transform.position.y < -3f && scored)
        {
            GameManager.Instance.RemoveScore(playerNumber); // ���_
            scored = false;
            Destroy(gameObject);
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (!scored)
        {
            GameManager.Instance.AddScore(playerNumber); // �ݒu�ŉ��_
            scored = true;
        }
    }
}