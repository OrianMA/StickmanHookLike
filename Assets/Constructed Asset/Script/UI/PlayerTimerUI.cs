using TMPro;
using UnityEngine;

public class PlayerTimerUI : MonoBehaviour
{
    public TextMeshProUGUI PlayerTimerText;
    [SerializeField] PlayerTimer _playerTimer;
    [SerializeField] float _milisecondSize;

    private void Update()
    {

        int minutes = Mathf.FloorToInt(_playerTimer.GetTime() / 60);
        int seconds = Mathf.FloorToInt(_playerTimer.GetTime() % 60);
        int milliseconds = Mathf.FloorToInt((_playerTimer.GetTime() * 1000) % 1000);

        //_playerTimerText.text = $"{minutes:00}:{seconds:00}:<size={_milisecondSize}%>{milliseconds:000}</size>";
        PlayerTimerText.text = $"{minutes:00}:{seconds:00}:<size={_milisecondSize}%><mspace=0.8em>{milliseconds:000}</mspace></size>";
    }
}
