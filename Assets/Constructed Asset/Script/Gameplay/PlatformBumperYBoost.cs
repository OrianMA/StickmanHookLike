using UnityEngine;

public class PlatformBumperYBoost : MonoBehaviour
{
    [SerializeField] private float _yVelocityForce;
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            PlayerMovement player = collision.gameObject.GetComponent<PlayerMovement>();

            player.PlayerRigidbody.linearVelocity = new Vector3(player.PlayerRigidbody.linearVelocity.x, _yVelocityForce);
        }
    }
}
