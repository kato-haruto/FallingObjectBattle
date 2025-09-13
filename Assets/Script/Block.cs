using UnityEngine;

public class Block : MonoBehaviour
{
    public int playerNumber; // �v���C���[����
    private bool scored = false; // �X�R�A�����ς݃t���O
    private Rigidbody rb;

    private float dropTime;        // �����J�n����
    private bool isDropped = false; // �����J�n�������ǂ���

    [Header("�ݒu����ݒ�")]
    public float stableVelocity = 0.1f; // ���x�����̒l�ȉ��Œ�~�Ƃ݂Ȃ�
    public float maxWaitTime = 3f;      // �����I�ɐݒu�Ƃ��鎞��
    public float minDropTime = 0.3f;    // ��������ɐݒu���肵�Ȃ��P�\����
    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }
    private void Update()
    {
        // --- �t�B�[���h�O ---
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
        // --- �ݒu���� ---
        if (isDropped && !scored)
        {
            float elapsed = Time.time - dropTime;
            // �Œ�P�\���Ԃ𒴂��āA���x���������ꍇ
            if (elapsed >= minDropTime && rb.velocity.magnitude < stableVelocity)
            {
                ScoreAndFix();
            }
            // �����ݒu
            else if (elapsed >= maxWaitTime)
            {
                ScoreAndFix();
            }
        }
    }
    public void OnDropped() // PlayerManager ����Ă΂��i�����J�n���j
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