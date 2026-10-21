using TMPro;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance { get; private set; }

    [SerializeField] private TMP_Text scoreText;
    private int score;

    public int CurrentScore => score;

    private void Awake()
    {
        Instance = this;

        if (scoreText == null)
        {
            scoreText = GetComponent<TMP_Text>();
        }

        if (scoreText == null)
        {
            GameObject scoreObject = GameObject.Find("score");
            if (scoreObject != null)
            {
                scoreText = scoreObject.GetComponent<TMP_Text>();
            }
        }

        if (scoreText == null)
        {
            Debug.LogError(
                "Assign a TextMeshPro component to Score Manager or add one to an active GameObject named 'score'.",
                this
            );
            return;
        }

        UpdateDisplay();
    }

    public void AddPoint()
    {
        score++;
        UpdateDisplay();
    }

    private void UpdateDisplay()
    {
        if (scoreText == null)
        {
            return;
        }

        scoreText.text = score.ToString();
    }
}