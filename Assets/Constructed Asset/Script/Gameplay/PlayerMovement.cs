using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    public InputSystem_Actions inpPlayer;

    private void Start()
    {
        inpPlayer = new InputSystem_Actions();
        inpPlayer.Enable();
        inpPlayer.Player.Hook.started += ProcessTouchStart;
        inpPlayer.Player.Hook.canceled += ProcessTouchCancel;

        inpPlayer.Enable();
    }

    private void ProcessTouchStart(InputAction.CallbackContext context)
    {

    }
    private void ProcessTouchCancel(InputAction.CallbackContext context)
    {

    }
}
