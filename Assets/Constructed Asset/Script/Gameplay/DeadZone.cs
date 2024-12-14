using UnityEngine;

public class DeadZone : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            PlayerMovement playerMovement = collision.gameObject.GetComponent<PlayerMovement>();
            
            playerMovement.transform.position = Vector3.zero;
            playerMovement.ResetVelocity();
            playerMovement.Timer.ResetTime();
        }
    }
}
