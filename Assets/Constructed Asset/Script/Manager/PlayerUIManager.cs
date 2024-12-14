using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerUIManager : MonoBehaviour
{
    [Header("In Game Canvas")]
    [SerializeField] CanvasGroup _inGameCanvas;
    public GameObject SkipToStartText;
    [SerializeField] PlayerTimerUI _playerTimerUI;

    [Header("On Finish Game Canvas")]
    [SerializeField] CanvasGroup _onFinishCanvas;
    [SerializeField] TextMeshProUGUI _finalTimerText;


    // On player end parcour, show onFinishPanel and set timer
    public void ShowOnFinishCanvas()
    {
        _inGameCanvas.interactable = false;
        _inGameCanvas.alpha = 0;

        _onFinishCanvas.interactable = true;
        _onFinishCanvas.alpha = 1;

        _finalTimerText.text = _playerTimerUI.PlayerTimerText.text;
    }

    // Reload the scene to restart the level
    public void RestartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void ContinueToNextLevel()
    {
        // Increment level index
        PlayerPrefs.SetInt("LevelIndex", PlayerPrefs.GetInt("LevelIndex") + 1);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void ReturnToMenu()
    {
        SceneManager.LoadScene("MenuScene");
    }
}
