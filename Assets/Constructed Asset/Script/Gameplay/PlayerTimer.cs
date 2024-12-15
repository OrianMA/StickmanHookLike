using TMPro;
using UnityEngine;

public class PlayerTimer : MonoBehaviour
{
    private float _timePass;
    private bool _isTimerStart;
    public TextMeshProUGUI PlayerTimerText;
    [SerializeField] float _milisecondSizeText;

    public void ResetTime()
    {
        _timePass = 0;
        SetActiveTimer(false);
    }

    public bool SetActiveTimer(bool Value)
    {
        return _isTimerStart = Value;
    }

    private void Update()
    {
        if (_isTimerStart)
        {
            _timePass += Time.deltaTime;
        }

        int minutes = Mathf.FloorToInt(_timePass / 60);
        int seconds = Mathf.FloorToInt(_timePass % 60);
        int milliseconds = Mathf.FloorToInt((_timePass * 1000) % 1000);

        //_playerTimerText.text = $"{minutes:00}:{seconds:00}:<size={_milisecondSize}%>{milliseconds:000}</size>";
        PlayerTimerText.text = $"{minutes:00}:{seconds:00}:<size={_milisecondSizeText}%><mspace=0.8em>{milliseconds:000}</mspace></size>";
    }


}
