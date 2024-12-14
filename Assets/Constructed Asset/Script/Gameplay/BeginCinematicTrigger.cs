using UnityEngine;

public class BeginCinematicTrigger : MonoBehaviour
{
    private void Start()
    {
        PlayerManager.Instance.SetBeginAnimation(this);
    }

    public void StartGame()
    {
        PlayerManager.Instance.StartGame();
        Destroy(gameObject);
    }

    public void SkipCinematic()
    {
        StartGame();
    }

}
