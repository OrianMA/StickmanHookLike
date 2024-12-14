using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _levelText;

    private void Start()
    {
        _levelText.text = PlayerPrefs.GetInt("LevelIndex").ToString();
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
}
