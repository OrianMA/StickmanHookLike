using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;
using DG.Tweening;
using System.Collections;
public class PlayerMovement : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private LineRenderer _hookLine;
    [SerializeField] private DistanceJoint2D _hookDistanceJoint;
    [SerializeField] private CinemachineCamera _playerCam;
    public PlayerUIManager PlayerUIManager;
    public BeginCinematicTrigger BeginCinematic;
    public Rigidbody2D PlayerRigidbody;
    public PlayerTimer Timer;

    [Header("Proprety")]
    [SerializeField] Vector3 _hookVisualOffset;
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
    private HookPoint _nearestHookPoint;

    // Use to controle the animation curve to the move forward animation
    private float _dynamicMoveForwardDuration;
    private float _startHookTime;
    private float _hookLenght;

    private bool _isFirstHooking;
    private bool _canPlayerMove;

    private void Start()
    {
        PlayerManager.Instance.AddPlayer(this);

        // Enable player inputs (Pc - mobile)
        _inpPlayer = new InputSystem_Actions();
        _inpPlayer.Player.Hook.started += ProcessTouchStart;
        _inpPlayer.Player.Hook.canceled += ProcessTouchCancel;

        _inpPlayer.Enable();
    }

    public void StartGame()
    {
        // Disable hook proprety
        _hookLine.enabled = false;
        _hookDistanceJoint.enabled = false;
        _isFirstHooking = true;

        // Can player move ? Now yes
        _canPlayerMove = true;

        // Text tap to skip cinematic disable
        PlayerUIManager.SkipToStartText.SetActive(false);
    }

    // Reset his speed
    public void ResetVelocity() => PlayerRigidbody.linearVelocity = Vector3.zero;

    private void ProcessTouchStart(InputAction.CallbackContext context)
    {
        if (!_canPlayerMove)
        {
            if (BeginCinematic)
            {
                BeginCinematic.SkipCinematic();
            }
            return;
        }

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

        if (PlayerRigidbody.linearVelocity.x <= _maxXVelocityToImpulse)
        {
            PlayerRigidbody.AddForceX(_xImpulsionOnStart);
        }

        if (_isFirstHooking)
        {
            Timer.SetActiveTimer(true);
        }
    }
    private void ProcessTouchCancel(InputAction.CallbackContext context)
    {
        if (!_hookPointGrab)
            return;

        // Disable hook and his visual
        _hookLine.enabled = false;
        _hookDistanceJoint.enabled = false;

        // If the player dont finish the animation move forward, apply a force to continue his trajectory
        if (_dynamicMoveForwardDuration < _moveForwardOnHookDuration)
        {
            PlayerRigidbody.AddForce((_hookPointGrab.transform.position - transform.position).normalized * _forceApplyOnDehook);
        }

        // Deassign the hookPoint
        _hookPointGrab.OnDehooked();
        _hookPointGrab = null;
    }

    private void Update()
    {
        if (!_canPlayerMove)
            return;

        // On hook, set always set the line position
        if (_hookLine.enabled) {

            // Control only the dynamic animation move forward at the beggining of hooking
            if (_dynamicMoveForwardDuration < _moveForwardOnHookDuration && PlayerRigidbody.linearVelocity.magnitude <= _magnitudeMaxToMoveForward) {

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
            if (PlayerRigidbody.linearVelocity.y < 0f) {

                PlayerRigidbody.linearVelocity += Vector2.down * _gravityForce * Time.deltaTime;
            }

            // Set lineRenderer start and end position
            _hookLine.SetPosition(0, transform.position + _hookVisualOffset);
            _hookLine.SetPosition(1, _hookPointGrab.transform.position);
        } 
        else
        {
            // Show what is the nearestHookPoint in game
            HookPoint newHookPoint = HookPointManager.Instance.GetNearestHookPoint(transform.position);
            if (!_nearestHookPoint || _nearestHookPoint != newHookPoint)
            {
                if (_nearestHookPoint != null) {
                    _nearestHookPoint.SetSelectHookPoint(false);
                }
                _nearestHookPoint = newHookPoint;
                _nearestHookPoint.SetSelectHookPoint(true);
            }
            
        }

        // Zoom and dezoom on the player with his speed
        _playerCam.Lens.OrthographicSize = Mathf.Lerp(_playerCam.Lens.OrthographicSize,
                                                      Mathf.Clamp(_cameraOrthographicSizeMinMax.x-1 + PlayerRigidbody.linearVelocity.magnitude / _divideMagnitude, _cameraOrthographicSizeMinMax.x, _cameraOrthographicSizeMinMax.y),
                                                      _cameraOrthographicSizeSpeed * Time.deltaTime);
    }

    public void OnEndAnimation()
    {
        // DOTWEEN ANIMATION END

        // Disable movement and gravity
        PlayerRigidbody.gravityScale = 0;
        _canPlayerMove = false;
        _inpPlayer.Disable();
        // Zoom on the player with dotween animation
        _playerCam.Lens.OrthographicSize = 4;
        Vector2 targetVelocity = PlayerRigidbody.linearVelocity.normalized;

        DOTween.Sequence(DOVirtual.Float(_playerCam.Lens.OrthographicSize, 4, .5f, (float t) =>
        {
            _playerCam.Lens.OrthographicSize = t;
            PlayerRigidbody.linearVelocity = Vector2.Lerp(PlayerRigidbody.linearVelocity, targetVelocity, 5 * Time.deltaTime);
            
        }).SetEase(Ease.Linear)).OnComplete(() => PlayerUIManager.ShowOnFinishCanvas());
    }
}
