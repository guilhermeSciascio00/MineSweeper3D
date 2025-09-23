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
    private InputAction _flagTileAction;

    //PlayerCameraActions

    private InputAction _cameraZoom;

    private void Awake()
    {
        _inputSysAction = new InputSystemAction();
        _movementAction = _inputSysAction.Player.Movement;
        _jumpAction = _inputSysAction.Player.Jump;
        _cameraZoom = _inputSysAction.Camera.CameraZoom;
        _gamePause = _inputSysAction.GameMenu.GamePause;
        _flagTileAction = _inputSysAction.Player.FlagTile;
       
    }

    private void EventManager_OnGameOver()
    {
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

    public bool IsFlagButtonPressed => _flagTileAction.WasPerformedThisFrame();

    private void EnableAllControls()
    {
        _movementAction.Enable();
        _jumpAction.Enable();
        _cameraZoom.Enable();
        _gamePause.Enable();
        _flagTileAction.Enable();
    }

    private void DisableAllControls()
    {
        _movementAction?.Disable();
        _jumpAction?.Disable();
        _cameraZoom?.Disable();
        _gamePause?.Disable();
        _flagTileAction?.Disable();
    }

}

