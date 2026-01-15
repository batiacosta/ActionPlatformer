using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInput : MonoBehaviour
{
    public FrameInput FrameInput { get; private set; }
    public static event Action OnShot;
    private PlayerInputActions _playerInputActions;
    private InputAction _jump;
    private InputAction _move;
    private InputAction _shoot;

    private void Awake()
    {
        _playerInputActions = new PlayerInputActions();
        _jump = _playerInputActions.Player.Jump;
        _move = _playerInputActions.Player.Move;
        _shoot = _playerInputActions.Player.Shoot;
    }

    private void OnEnable()
    {
        _playerInputActions.Enable();
        _shoot.performed += Shoot;
    }

    private void OnDisable()
    {
        _shoot.performed += Shoot;
        _playerInputActions.Disable();
    }

    private void Shoot(InputAction.CallbackContext obj)
    {
        OnShot?.Invoke();
    }

    private void Update()
    {
        FrameInput = GatherInput();
    }

    private FrameInput GatherInput()
    {
        return new FrameInput
        {
            Jump = _jump.WasPressedThisDynamicUpdate(),
            Move = _move.ReadValue<Vector2>()
        };
    }
}

public struct FrameInput
{
    public Vector2 Move;
    public bool Jump;
}
