using BaseTemplate.Behaviours;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

public class PlayerManager : MonoSingleton<PlayerManager>
{
    private List<PlayerMovement> _allPlayer = new();
    private List<Transform> _allPlayerPos = new();

    // For manager all player (if there are more than one player)
    public void AddPlayer(PlayerMovement player)
    {
        _allPlayer.Add(player);
        _allPlayerPos.Add(player.transform);
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

    public List<Transform> GetPlayerPos() => _allPlayerPos;

    public void SavePlayerDatas(PlayerMovement player)
    {
        // If Player make the most advanced level for him, increment his max level available
        if (PlayerPrefs.GetInt("MaxLevelIndex") == PlayerPrefs.GetInt("LevelIndex"))
        {
            PlayerPrefs.SetInt("MaxLevelIndex", PlayerPrefs.GetInt("MaxLevelIndex") + 1);
            PlayerPrefs.SetString($"Level{PlayerPrefs.GetInt("LevelIndex")}", player.Timer.PlayerTimerText.text);
        } else
        {
            // Clean the strings to remove TextMeshPro tags
            string cleanTimer1 = CleanTimerString(player.Timer.PlayerTimerText.text);
            string cleanTimer2 = CleanTimerString(PlayerPrefs.GetString($"Level{PlayerPrefs.GetInt("LevelIndex")}"));

            // Parse the cleaned strings
            TimeSpan currentTime = ParseCustomTimeFormat(cleanTimer1);
            TimeSpan bestTime = ParseCustomTimeFormat(cleanTimer2);

            if (currentTime < bestTime)
                PlayerPrefs.SetString($"Level{PlayerPrefs.GetInt("LevelIndex")}", player.Timer.PlayerTimerText.text);
        }
    }

    private string CleanTimerString(string timer)
    {
        // Use Regex to remove all TextMeshPro tags
        return Regex.Replace(timer, "<.*?>", string.Empty);
    }

    private TimeSpan ParseCustomTimeFormat(string timer)
    {
        // Split the string into parts (minutes:seconds:milliseconds)
        string[] parts = timer.Split(':');
        int minutes = int.Parse(parts[0]);
        int seconds = int.Parse(parts[1]);
        int milliseconds = int.Parse(parts[2]);

        // Create a TimeSpan from the parsed values
        return new TimeSpan(0, 0, minutes, seconds, milliseconds);
    }
}
