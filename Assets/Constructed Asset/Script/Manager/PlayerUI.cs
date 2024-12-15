using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerUI : MonoBehaviour
{
    [Header("References")]
    [SerializeField] List<CanvasGroup> _canvasGroups;
    [SerializeField] CanvasGroup _inGameCanvas;
    [SerializeField] CanvasGroup _pauseCanvas;
    [SerializeField] CanvasGroup _onFinishCanvas;
    [SerializeField] PlayerTimer _timer;

    [Header("In Game Canvas")]
    public GameObject SkipToStartText;
    public GameObject PauseButton;

    [Header("Pause Canvas")]
    [SerializeField] TextMeshProUGUI _timerPauseText;


    [Header("On Finish Game Canvas")]
    [SerializeField] TextMeshProUGUI _finalTimerText;


    private Vector3 _saveVelocityToPause;
    // On player end parcour, show onFinishPanel and set timer
    public void ShowOnFinishCanvas()
    {
        _inGameCanvas.interactable = false;
        _inGameCanvas.alpha = 0;

        _onFinishCanvas.interactable = true;
        _onFinishCanvas.alpha = 1;

        OpenCanvasGroup(_onFinishCanvas);

        _finalTimerText.text = _timer.PlayerTimerText.text;
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

    public void OpenCanvasGroup(CanvasGroup group)
    {
        foreach (CanvasGroup child in _canvasGroups)
        {
            child.alpha = 0;
            child.interactable = false;
            child.blocksRaycasts = false;
        }

        group.alpha = 1;
        group.interactable = true;
        group.blocksRaycasts = true;
    }

    public void SetActivePause(bool value)
    {
        PlayerMovement player = transform.parent.GetComponent<PlayerMovement>();
        
        if (value)
        {
            // Open canvas pause and set proprety
            OpenCanvasGroup(_pauseCanvas);
            _timerPauseText.text = _timer.PlayerTimerText.text;

            // Desactive playter input and block his speed
            player.SetActiveInput(false);
            _saveVelocityToPause = player.PlayerRigidbody.linearVelocity;
            player.PlayerRigidbody.bodyType = RigidbodyType2D.Static;
        } else
        {
            OpenCanvasGroup(_inGameCanvas);
            
            transform.parent.GetComponent<PlayerMovement>().SetActiveInput(true);

            player.PlayerRigidbody.bodyType = RigidbodyType2D.Dynamic;
            player.PlayerRigidbody.linearVelocity = _saveVelocityToPause;
        }
    }

}
