using TMPro;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private LineRenderer _hookLine;
    [SerializeField] private DistanceJoint2D _hookDistanceJoint;
    [SerializeField] private Rigidbody2D _playerRigidbody;
    [SerializeField] private CinemachineCamera _playerCam;
    [SerializeField] private GameObject _playerBackground;

    [Header("Proprety")]
    [SerializeField] float _hookVisualYOffset;
    [SerializeField] float _maxXVelocityToImpulse;
    [SerializeField] float _xImpulsionOnStart;
    [Space(5)]
    [SerializeField] private float _moveForwardOnHookSpeed;
    [SerializeField] private float _moveForwardOnHookDuration;
    [SerializeField] AnimationCurve _moveForwardCurvefunction;
    [SerializeField] private float _magnitudeMaxToMoveForward;
    [Space(5)]
    // On Player Dehook when he not finish the moveForwardAnimation
    [SerializeField] float _forceApplyOnDehook;

    [SerializeField] float _gravityForce;

    [Header("Camera propreties")]
    [SerializeField] Vector2 _cameraOrthographicSizeMinMax;
    [SerializeField] float _cameraOrthographicSizeSpeed;
    [SerializeField] float _divideMagnitude;

    private InputSystem_Actions _inpPlayer;
    private HookPoint _hookPointGrab;

    // Use to controle the animation curve to the move forward animation
    private float _dynamicMoveForwardDuration;
    private float _startHookTime;
    private float _hookLenght;

    private float _dynamicCameraOrthographicSize; 

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

        if (_playerRigidbody.linearVelocity.x <= _maxXVelocityToImpulse)
        {
            _playerRigidbody.AddForceX(_xImpulsionOnStart);
        }
    }
    private void ProcessTouchCancel(InputAction.CallbackContext context)
    {

        // Disable hook and his visual
        _hookLine.enabled = false;
        _hookDistanceJoint.enabled = false;

        // If the player dont finish the animation move forward, apply a force to continue his trajectory
        if (_dynamicMoveForwardDuration < _moveForwardOnHookDuration)
        {
            _playerRigidbody.AddForce((_hookPointGrab.transform.position - transform.position).normalized * _forceApplyOnDehook);
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
            if (_dynamicMoveForwardDuration < _moveForwardOnHookDuration && _playerRigidbody.linearVelocity.magnitude <= _magnitudeMaxToMoveForward) {

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

            }
                _dynamicMoveForwardDuration += Time.deltaTime;


            // Apply adding force when the player fall with the hook on 
            if (_playerRigidbody.linearVelocity.y < 0f) {
            
                _playerRigidbody.linearVelocity += Vector2.down * _gravityForce * Time.deltaTime;
            }

            // Set lineRenderer start and end position
            _hookLine.SetPosition(0, transform.position + Vector3.up * _hookVisualYOffset);
            _hookLine.SetPosition(1, _hookPointGrab.transform.position);
        }


        _playerCam.Lens.OrthographicSize = Mathf.Lerp(_playerCam.Lens.OrthographicSize,
                                                      Mathf.Clamp(_cameraOrthographicSizeMinMax.x-1 + _playerRigidbody.linearVelocity.magnitude / _divideMagnitude, _cameraOrthographicSizeMinMax.x, _cameraOrthographicSizeMinMax.y),
                                                      _cameraOrthographicSizeSpeed * Time.deltaTime);


        // Get the orthographic size and aspect ratio
        float aspectRatio = (float)Screen.width / Screen.height;

        // Calculate the size of the square
        float height = _playerCam.Lens.OrthographicSize * 2f; // Total height of camera view
        float width = height * aspectRatio;  // Total width of camera view
        float size = Mathf.Min(width, height); // Ensure it fits within both dimensions

        // Adjust the scale of the square
        _playerBackground.transform.localScale = new Vector3(width*1.25f, height*1.25f, 1f);

    }
}
