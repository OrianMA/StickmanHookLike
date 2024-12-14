using System.Collections.Generic;
using UnityEngine;

public class DeadZone : MonoBehaviour
{
    [SerializeField] float _YdistanceToTriggerDeadZone;
    private List<Transform> _playerPos = new();

    private void Start()
    {
        if (_YdistanceToTriggerDeadZone != 0)
        {
            _playerPos = PlayerManager.Instance.GetPlayerPos();
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            PlayerMovement playerMovement = collision.gameObject.GetComponent<PlayerMovement>();

            ResetPlayerPos(playerMovement);
        }
    }


    private void Update()
    {
        if (_YdistanceToTriggerDeadZone == 0)
            return;

        foreach (Transform pos in _playerPos)
        {
            if (pos.position.y <= _YdistanceToTriggerDeadZone)
            {
                ResetPlayerPos(pos.GetComponent<PlayerMovement>());
            }
        }
    }

    private void ResetPlayerPos(PlayerMovement playerMovement)
    {
        playerMovement.transform.position = Vector3.zero;
        playerMovement.ResetVelocity();
        playerMovement.Timer.ResetTime();
    }
}
