using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;

public class DeathMenuController : MonoBehaviour
{
    public static DeathMenuController Instance;

    [Header("UI Screen Canvas Container")]
    [SerializeField] private GameObject deathMenuCanvas;

    [Header("UI Text Display Components")]
    [SerializeField] private TMP_Text currentScoreText;
    [SerializeField] private TMP_InputField nameInputField;

    private int finalScore;

    void Awake()
    {
        Instance = this;
        if (deathMenuCanvas != null)
        {
            deathMenuCanvas.SetActive(false);
        }
    }

    public void TriggerDeathMenu()
    {
        StartCoroutine(WaitAndShowMenuRoutine());
    }

    IEnumerator WaitAndShowMenuRoutine()
    {
        yield return new WaitForSeconds(2.5f);

        if (deathMenuCanvas != null)
        {
            deathMenuCanvas.SetActive(true);
        }

        finalScore = ScoreController.Instance != null ? ScoreController.Instance.score : 0;
        
        if (currentScoreText != null) 
        {
            currentScoreText.text = "Your Score: " + finalScore;
        }

        Time.timeScale = 0f; 
    }

    public void SubmitAndGoToHighScores()
    {
        if (nameInputField == null) return;

        string playerPrefName = nameInputField.text.Trim();
        if (string.IsNullOrEmpty(playerPrefName)) playerPrefName = "Anonymous";

        if (HighScoreManager.Instance != null)
        {
            HighScoreManager.Instance.AddNewScore(playerPrefName, finalScore);
            
            HighScoreManager.Instance.showHighScoresOnLoad = true;
        }

        Time.timeScale = 1f; 
        
        SceneManager.LoadScene(0); 
    }
}