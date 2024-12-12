using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private LineRenderer _hookLine;
    [SerializeField] private DistanceJoint2D _hookDistanceJoint;
    [SerializeField] private Rigidbody2D _hookRigidbody;

    [Header("Proprety")]
    [SerializeField] private float _moveForwardOnHookSpeed;
    [SerializeField] private float _moveForwardOnHookDuration;
    [SerializeField] AnimationCurve _moveForwardCurvefunction;
    [Space(5)]
    // On Player Dehook when he not finish the moveForwardAnimation
    [SerializeField] float _forceApplyOnDehook;

    private InputSystem_Actions _inpPlayer;
    private HookPoint _hookPointGrab;

    // Use to controle the animation curve to the move forward animation
    private float _dynamicMoveForwardDuration;
    private float _startHookTime;
    private float _hookLenght;

    private void Start()
    {
        // Disable hook proprety
        _hookLine.enabled = false;
        _hookDistanceJoint.enabled = false;

        // Enable player inputs (Pc - mobile)
        _inpPlayer = new InputSystem_Actions();
        _inpPlayer.Player.Hook.started += ProcessTouchStart;
        _inpPlayer.Player.Hook.canceled += ProcessTouchCancel;

        _inpPlayer.Enable();
    }

    private void ProcessTouchStart(InputAction.CallbackContext context)
    {
        // Get the nereast hook point and assign it
        _hookPointGrab = HookPointManager.Instance.GetNearestHookPoint(transform.position);
        _hookPointGrab.OnHooked();

        // Set proprety for animation move forward
        _startHookTime = Time.time;
        _hookLenght = Vector2.Distance(transform.position, _hookPointGrab.transform.position);
        _dynamicMoveForwardDuration = 0;

        // Enable hook and his visual 
        _hookLine.enabled = true;
        _hookDistanceJoint.enabled = true;
        _hookDistanceJoint.connectedBody = _hookPointGrab.rb;
    }
    private void ProcessTouchCancel(InputAction.CallbackContext context)
    {

        // Disable hook and his visual
        _hookLine.enabled = false;
        _hookDistanceJoint.enabled = false;

        // If the player dont finish the animation move forward, apply a force to continue his trajectory
        if (_dynamicMoveForwardDuration < _moveForwardOnHookDuration)
        {
            _hookRigidbody.AddForce((_hookPointGrab.transform.position - transform.position).normalized * _forceApplyOnDehook);
        }

        // Deassign the hookPoint
        _hookPointGrab.OnDehooked();
        _hookPointGrab = null;
    }

    private void Update()
    {
        // On hook, set always set the line position
        if (_hookLine.enabled) {

            // Control only the dynamic animation move forward at the beggining of hooking
            if (_dynamicMoveForwardDuration < _moveForwardOnHookDuration) {

                float distanceCovered = (Time.time - _startHookTime) * _moveForwardOnHookSpeed;
                float fractionOfJourney = distanceCovered / _hookLenght;

                // Ensure that the fraction does not exceed 1 (when reaching the target)
                fractionOfJourney = Mathf.Clamp01(fractionOfJourney);

                // Evaluate the custom animation curve to get an easing value
                float curveValue = _moveForwardCurvefunction.Evaluate(fractionOfJourney);

                // Apply ease-in-out function to the fraction of journey
                fractionOfJourney = Mathf.SmoothStep(0f, 1f, fractionOfJourney);

                // Move the player to the direction
                transform.position += (_hookPointGrab.transform.position - transform.position).normalized * _moveForwardOnHookSpeed * curveValue * Time.deltaTime;

                _dynamicMoveForwardDuration += Time.deltaTime;
            }

            //Vector2 force = _hookDirection * _initialForce * forceMultiplier;

            // Apply the force to the Rigidbody2D (in the direction of the hook)
            //_rb.AddForce(force, ForceMode2D.Force);

            // Set lineRenderer start and end position
            _hookLine.SetPosition(0, transform.position);
            _hookLine.SetPosition(1, _hookPointGrab.transform.position);
        }
    }
}
