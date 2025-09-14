using UnityEngine;

public class Block : MonoBehaviour
{
    public int playerNumber; // �v���C���[����
    private bool scored = false;      // �X�R�A�����ς݃t���O
    private bool processed = false;   // �ݒu/���������ς݃t���O
    private bool nextBlockSpawned = false; // ���ǉ��F���̃u���b�N�����ς݃t���O

    private Rigidbody rb;
    private PlayerManager owner;      // �����𗎂Ƃ����v���C���[

    private float dropTime;        // �����J�n����
    private bool isDropped = false; // �����J�n�������ǂ���

    [Header("�ݒu����ݒ�")]
    public float stableVelocity = 0.1f; // ��~����̑��x
    public float maxWaitTime = 3f;      // �����I�ɐݒu�Ƃ��鎞��
    public float minDropTime = 0.3f;    // ��������ɐݒu���肵�Ȃ��P�\����

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        if (processed) return; // --- ���ɐݒu/���������ς݂Ȃ疳�� ---

        // --- �t�B�[���h�O�ɗ��� ---
        if (transform.position.y < -5f)
        {
            if (scored) // �ݒu��ɗ������ꍇ�͌��_
            {
                GameManager.Instance.RemoveScore(playerNumber);
                scored = false;
            }
            ProcessedEnd(false); // ���������i�u���b�N�폜����j
            return;
        }

        // --- �ݒu���� ---
        if (isDropped && !scored)
        {
            float elapsed = Time.time - dropTime;
            // ��莞�Ԍo�� & ��~
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

    public void OnDropped(PlayerManager player) // PlayerManager ����Ă΂��i�����J�n���j
    {
        isDropped = true;
        dropTime = Time.time;
        owner = player; // �������Ǘ����Ă���v���C���[���L�^
    }

    private void ScoreAndFix()
    {
        if (processed) return; // ��d�h�~
        GameManager.Instance.AddScore(playerNumber);
        scored = true;

        ProcessedEnd(true); // �ݒu�����i�u���b�N�c���j
    }

    /// <summary>
    /// �ݒu or �����ŏI������
    /// </summary>
    /// <param name="keepBlock">true�Ȃ�u���b�N���c�� / false�Ȃ�폜</param>
    private void ProcessedEnd(bool keepBlock)
    {
        if (processed) return;
        processed = true;

        // --- ���̃u���b�N�����͈�x���� ---
        if (!nextBlockSpawned && owner != null)
        {
            owner.SpawnNewBlock();
            nextBlockSpawned = true;
        }

        if (!keepBlock)
        {
            Destroy(gameObject, 0.1f); // �������̂ݍ폜
        }
        // keepBlock==true �̏ꍇ�͉��������c���i�����������ێ��j
    }
}
