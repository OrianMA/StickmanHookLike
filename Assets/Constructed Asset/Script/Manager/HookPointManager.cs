using BaseTemplate.Behaviours;
using System.Collections.Generic;
using UnityEngine;

public class HookPointManager : MonoSingleton<HookPointManager>
{
    // Hook point on the current level, only available on read
    private List<HookPoint> _hookPoints = new();
    public void AddHookPoint(HookPoint point) => _hookPoints.Add(point);


    public HookPoint GetNearestHookPoint(Vector3 pos)
    {
        // Create list of HookPoint and his distance to pos
        List<KeyValuePair<HookPoint, float>> distance = new();

        // Add the HookPoint and calcule distance to pos
        foreach (var hookPoint in _hookPoints) {
            distance.Add(new KeyValuePair<HookPoint, float>(hookPoint, Vector3.Distance(pos, hookPoint.transform.position)));
        }

        // Sort the list to have the nearest pos in first
        distance.Sort((a, b) => a.Value.CompareTo(b.Value));

        // Return the first so the nearest
        return distance[0].Key;
    }


}
