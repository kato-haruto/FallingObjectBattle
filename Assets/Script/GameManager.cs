using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    [Header("UI関連")]
    public TextMeshProUGUI RemainingTimeText; // "TIME"の表示
    public TextMeshProUGUI timerText;    // 残り時間表示
    public TextMeshProUGUI p1ScoreText;  // P1スコア表示
    public TextMeshProUGUI p2ScoreText;  // P2スコア表示
    public GameObject resultPanel;       // 結果表示パネル
    public TextMeshProUGUI resultText;   // 結果テキスト
    [Header("ゲーム設定")]
    public float gameTime = 60f; // 制限時間（秒）

    private float remainingTime;
    private int p1Score = 0;
    private int p2Score = 0;
    private bool gameEnded = false;
    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }
    void Start()
    {
        remainingTime = gameTime;
        UpdateUI();
        resultPanel.SetActive(false); // ゲーム開始時は非表示
    }
    void Update()
    {
        if (gameEnded) return;
        // タイマー処理
        remainingTime -= Time.deltaTime;
        if (remainingTime <= 0f)
        {
            remainingTime = 0f;
            EndGame();
        }
        UpdateUI();
    }
    void UpdateUI()
    {
        RemainingTimeText.text = "TIME";
        timerText.text = Mathf.CeilToInt(remainingTime).ToString();
        p1ScoreText.text = "Score: " + p1Score;
        p2ScoreText.text = "Score: " + p2Score;
    }
    public void AddScoreP1(int amount) // P1がスコアを得たとき
    {
        p1Score += amount;
        UpdateUI();
    }
    public void AddScoreP2(int amount) // P1がスコアを得たとき
    {
        p2Score += amount;
        UpdateUI();
    }
    public void AddScore(int playerID)
    {
        if (playerID == 1) p1Score++;
        else if (playerID == 2) p2Score++;
        UpdateUI();
    }
    public void RemoveScore(int playerID)
    {
        if (playerID == 1) p1Score = Mathf.Max(0, p1Score - 1);
        else if (playerID == 2) p2Score = Mathf.Max(0, p2Score - 1);
        UpdateUI();
    }
    void EndGame()
    {
        gameEnded = true;
        resultPanel.SetActive(true);

        if (p1Score > p2Score)
        {
            resultText.text = "P1 WIN!";
        }
        else if (p2Score > p1Score)
        {
            resultText.text = "P2 WIN!";
        }
        else
        {
            resultText.text = "DRAW!";
        }
    }
}