using UnityEngine;

public class HookPoint : MonoBehaviour
{
    void Start()
    {
        HookPointManager.Instance.AddHookPoint(this);
    }
}
