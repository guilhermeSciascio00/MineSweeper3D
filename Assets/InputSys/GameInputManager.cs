using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameInputManager : MonoBehaviour 
{
    //PlayerActions
    private InputSystemAction _inputSysAction;
    private InputAction _movementAction;
    private InputAction _jumpAction;
    private InputAction _gamePause;

    //PlayerCameraActions

    private InputAction _cameraZoom;

    private void Awake()
    {
        _inputSysAction = new InputSystemAction();
        _movementAction = _inputSysAction.Player.Movement;
        _jumpAction = _inputSysAction.Player.Jump;
        _cameraZoom = _inputSysAction.Camera.CameraZoom;
        _gamePause = _inputSysAction.GameMenu.GamePause;

       
    }

    private void EventManager_OnGameOver()
    {
        Debug.Log("HEEY");
        DisableAllControls();
    }

    private void EventManager_OnGameWon()
    {
        DisableAllControls();
    }

    private void OnEnable()
    {

        EventManager.OnGameWon += EventManager_OnGameWon;
        EventManager.OnGameOver += EventManager_OnGameOver;

        EnableAllControls();
    }

    private void OnDisable()
    {

        EventManager.OnGameWon -= EventManager_OnGameWon;
        EventManager.OnGameOver -= EventManager_OnGameOver;

        DisableAllControls();
    }

    public Vector3 GetDirectionValue() => _movementAction.ReadValue<Vector2>();

    public bool IsJumping() => _jumpAction.IsPressed();

    public Vector2 GetCameraZoom() => _cameraZoom.ReadValue<Vector2>();

    public bool IsPauseButtonPressed() => _gamePause.WasPressedThisFrame();

    private void EnableAllControls()
    {
        _movementAction.Enable();
        _jumpAction.Enable();
        _cameraZoom.Enable();
        _gamePause.Enable();
    }

    private void DisableAllControls()
    {
        _movementAction?.Disable();
        _jumpAction?.Disable();
        _cameraZoom?.Disable();
        _gamePause?.Disable();
    }

}

