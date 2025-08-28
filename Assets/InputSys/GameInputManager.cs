using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameInputManager : MonoBehaviour 
{
    //PlayerActions
    private InputSystemAction _inputSysAction;
    private InputAction _movementAction;
    private InputAction _jumpAction;

    //PlayerCameraActions

    private InputAction _cameraZoom;

    private void Awake()
    {
        _inputSysAction = new InputSystemAction();
        _movementAction = _inputSysAction.Player.Movement;
        _jumpAction = _inputSysAction.Player.Jump;
        _cameraZoom = _inputSysAction.Camera.CameraZoom;
    }

    private void OnEnable()
    {
        _movementAction.Enable();
        _jumpAction.Enable();
        _cameraZoom.Enable();
    }

    private void OnDisable()
    {
        _movementAction?.Disable();
        _jumpAction?.Disable();
        _cameraZoom?.Disable();
    }

    public Vector3 GetDirectionValue() => _movementAction.ReadValue<Vector2>();

    public bool IsJumping() => _jumpAction.IsPressed();

    public Vector2 GetCameraZoom() => _cameraZoom.ReadValue<Vector2>();

}

