using TMPro;
using UnityEngine;

public class GameManager : SingletonMonoBehavior<GameManager>
{
    [SerializeField] private int score = 0;
    [SerializeField] private TextMeshProUGUI scoreText;

    protected override void Awake()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        scoreText = GetComponent<TextMeshProUGUI>();
    }

    public void IncreaseScore()
    {
        score++;
        scoreText.text = $"Score: {score}";
    }
}
