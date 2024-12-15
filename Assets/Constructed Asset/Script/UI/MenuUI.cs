using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuUI : MonoBehaviour
{
    [SerializeField] private List<CanvasGroup> _canvasGroups;
    [SerializeField] private TextMeshProUGUI _levelText;

    [Tooltip("Make the first text, the first button, in order")]
    [SerializeField] private List<TextMeshProUGUI> _bestScoreTexts;

    private void Start()
    {
        for (int i = 0; i < _bestScoreTexts.Count; i++)
        {
            if (i > PlayerPrefs.GetInt("MaxLevelIndex"))
                _bestScoreTexts[i].transform.parent.gameObject.SetActive(false);

            else if (i == PlayerPrefs.GetInt("MaxLevelIndex"))
                _bestScoreTexts[i].text = "00:00:00";

            else
                _bestScoreTexts[i].text = PlayerPrefs.GetString($"Level{i}");
        }

        _levelText.text = PlayerPrefs.GetInt("MaxLevelIndex").ToString();
    }

    public void StartGame(int levelIndex)
    {
        PlayerPrefs.SetInt("LevelIndex", levelIndex);
        SceneManager.LoadScene("MainScene");
    }

    public void ContinueGame()
    {
        PlayerPrefs.SetInt("LevelIndex", PlayerPrefs.GetInt("MaxLevelIndex"));
        SceneManager.LoadScene("MainScene");
    }

    public void OpenCanvasGroup(CanvasGroup group)
    {
        foreach (CanvasGroup child in _canvasGroups) {
            child.alpha = 0;
            child.interactable = false;
            child.blocksRaycasts = false;
        }

        group.alpha = 1;
        group.interactable = true;
        group.blocksRaycasts = true;
    }

    public void QuitGame()
    {
        Application.Quit();

#if UNITY_EDITOR
        EditorApplication.ExitPlaymode();
        #endif
    }
}
