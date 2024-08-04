using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public Text scoreText;
    public Text timerText;

    public void UpdateScore(int score)
    {
        scoreText.text = "Score: " + score;
    }

    public void UpdateTimer(float time)
    {
        timerText.text = "Time: " + Mathf.Ceil(time);
    }

    public void ShowEndGameScreen(int finalScore)
    {
        // Show end game UI
    }
}
