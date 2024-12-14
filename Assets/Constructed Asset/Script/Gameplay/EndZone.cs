using UnityEngine;

public class EndZone : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Detect player
        if (collision.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            PlayerMovement playerMovement = collision.gameObject.GetComponent<PlayerMovement>();
            playerMovement.Timer.SetActiveTimer(false);
            
            // End Animation
            playerMovement.OnEndAnimation();


            // If Player make the most advanced level for him, increment his max level available
            if (PlayerPrefs.GetInt("MaxLevelIndex") == PlayerPrefs.GetInt("LevelIndex"))
            {
                PlayerPrefs.SetInt("MaxLevelIndex", PlayerPrefs.GetInt("MaxLevelIndex") + 1);
            }
        }
    }
}
