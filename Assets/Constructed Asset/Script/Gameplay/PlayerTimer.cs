using UnityEngine;

public class PlayerTimer : MonoBehaviour
{
    private float _timePass;
    private bool _isTimerStart;

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
    }

    public float GetTime() => _timePass;
}
