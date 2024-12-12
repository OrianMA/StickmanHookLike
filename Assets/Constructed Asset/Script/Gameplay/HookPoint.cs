using UnityEngine;

public class HookPoint : MonoBehaviour
{
    public Rigidbody2D rb;

    [SerializeField] private SpriteRenderer _NoHookedRenderer;
    [SerializeField] private SpriteRenderer _HookedRenderer;

    void Start()
    {
        HookPointManager.Instance.AddHookPoint(this);
        ChangeSprite(false);
    }

    public void OnHooked()
    {
        ChangeSprite(true);
    }

    public void OnDehooked()
    {
        ChangeSprite(false);
    }

    // Change sprite dynamicly
    private void ChangeSprite(bool isHooked)
    {
        _HookedRenderer.gameObject.SetActive(isHooked);
        _NoHookedRenderer.gameObject.SetActive(!isHooked);
    }
}
