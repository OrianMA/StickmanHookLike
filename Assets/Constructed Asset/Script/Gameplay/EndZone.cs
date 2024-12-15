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

            PlayerManager.Instance.SavePlayerDatas(playerMovement);
        }
    }
}
