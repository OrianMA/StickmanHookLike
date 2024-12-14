using TMPro;
using UnityEngine;

public class MenuUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _levelText;

    private void Start()
    {
        _levelText.text = PlayerPrefs.GetInt("LevelIndex").ToString();
    }

    public void StartGame()
    {

    }
}
