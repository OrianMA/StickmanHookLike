using BaseTemplate.Behaviours;
using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoSingleton<PlayerManager>
{
    private List<PlayerMovement> _allPlayer = new();

    // For manager all player (if there are more than one player)
    public void AddPlayer(PlayerMovement player)
    {
        _allPlayer.Add(player);
    }

    public void SetBeginAnimation(BeginCinematicTrigger trigger)
    {
        foreach (PlayerMovement player in _allPlayer)
        {
            player.BeginCinematic = trigger;
        }
    }

    // Enable all the player
    public void StartGame()
    {
        foreach (PlayerMovement player in _allPlayer)
        {
            player.StartGame();
        }
    }
}
