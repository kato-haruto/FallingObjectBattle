using UnityEngine;

public class Block : MonoBehaviour
{
    public int playerNumber; // �v���C���[����
    private bool scored = false;        // �X�R�A�����ς݃t���O
    private bool nextBlockSpawned = false; // ���̃u���b�N�����ς݃t���O

    private Rigidbody rb;
    private PlayerManager owner;      // �����𗎂Ƃ����v���C���[

    private float dropTime;        // �����J�n����
    private bool isDropped = false; // �����J�n�������ǂ���
    private bool finalized = false; // ���ǉ�: ���̃u���b�N�̏I�����������S�ɏI�������

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
        if (finalized) return; // ���S�ɏI�������珈�����Ȃ�

        // --- �t�B�[���h�O�ɗ��� ---
        if (transform.position.y < -5f)
        {
            if (scored) // ���ݒu��ɗ����� �� ���_
            {
                GameManager.Instance.RemoveScore(playerNumber);
                scored = false;
            }

            EndBlock(false); // �u���b�N�폜
            return;
        }

        // --- �ݒu���� ---
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

    public void OnDropped(PlayerManager player) // PlayerManager ����Ă΂��i�����J�n���j
    {
        isDropped = true;
        dropTime = Time.time;
        owner = player;
    }

    private void ScoreAndFix()
    {
        if (scored) return; // ��d�h�~

        GameManager.Instance.AddScore(playerNumber); // ���_
        scored = true;

        EndBlock(true); // �ݒu����
    }

    /// <summary>
    /// �ݒu or �����ŏI������
    /// </summary>
    /// <param name="keepBlock">true�Ȃ�u���b�N���c�� / false�Ȃ�폜</param>
    private void EndBlock(bool keepBlock)
    {
        // --- ���̃u���b�N�����͈�x���� ---
        if (!nextBlockSpawned && owner != null)
        {
            owner.SpawnNewBlock();
            nextBlockSpawned = true;
        }

        if (!keepBlock)
        {
            Destroy(gameObject, 0.1f); // �������̂ݍ폜
            finalized = true; // ���S�I��
        }
        // keepBlock==true �̏ꍇ�͎c�� �� �܂������`�F�b�N�͓���
    }
}
