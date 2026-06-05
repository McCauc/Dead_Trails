using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class StartMenu : MonoBehaviour
{
    [Header("Panels")]
    [SerializeField] private GameObject mainMenuPanel;
    [SerializeField] private GameObject highScorePanel;

    [Header("UI Elements")]
    [SerializeField] private TMP_Text highScoreTextDisplay;

    void Start()
    {
        if (HighScoreManager.Instance != null && HighScoreManager.Instance.showHighScoresOnLoad)
        {
            HighScoreManager.Instance.showHighScoresOnLoad = false; // Immediately consume and reset flag
            ShowHighScores();
        }
        else
        {
            if (mainMenuPanel != null) mainMenuPanel.SetActive(true);
            if (highScorePanel != null) highScorePanel.SetActive(false);
        }
    }

    public void StartGame()
    {
        SceneManager.LoadScene(1); 
    }

    public void ShowHighScores()
    {
        if (mainMenuPanel != null) mainMenuPanel.SetActive(false);
        if (highScorePanel != null) highScorePanel.SetActive(true);

        if (HighScoreManager.Instance != null && highScoreTextDisplay != null)
        {
            List<HighScoreEntry> topScores = HighScoreManager.Instance.GetTopScores();
            string displayString = "TOP 10 HIGH SCORES\n\n";
            
            for (int i = 0; i < 10; i++)
            {
                if (i < topScores.Count)
                {
                    displayString += $"{i + 1}. {topScores[i].name} - {topScores[i].score}\n";
                }
                else
                {
                    displayString += $"{i + 1}. --- - 0\n";
                }
            }
            highScoreTextDisplay.text = displayString;
        }
    }

    public void CloseHighScores()
    {
        if (highScorePanel != null) highScorePanel.SetActive(false);
        if (mainMenuPanel != null) mainMenuPanel.SetActive(true);
    }

    public void ExitGame()
    {
        Debug.Log("Exit Game button pressed!");
        Application.Quit();
    }
}