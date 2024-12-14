using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    [SerializeField] private List<GameObject> _levels; 

    void Start()
    {
        Instantiate(_levels[PlayerPrefs.GetInt("LevelIndex") % (_levels.Count)]);
    }
}
