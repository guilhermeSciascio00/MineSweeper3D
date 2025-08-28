using Unity.Cinemachine;
using UnityEngine;

public class ThirdPersonCamera : MonoBehaviour
{
    [Header("Camera Properties")]
    [SerializeField] private float _minZoomDistance = 2f;
    [SerializeField] private float _maxZoomDistance = 8f;
    [SerializeField] private float _zoomScrollSpeed = 4f;
    [SerializeField] private float _zoomLerpSpeed = 10f;

    [Header("References")]
    [SerializeField] private GameInputManager _inputManager;
    private CinemachineCamera _cinemachineCam;
    private CinemachineOrbitalFollow _cinemachineOrbitFollow;

    //scroll variables
    private Vector2 _scrollDelta;
    private Vector2 _scrollPosition;

    //lerp variables
    private float _targetZoom;
    private float _currentZoom;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _cinemachineCam = GetComponent<CinemachineCamera>();
        _cinemachineOrbitFollow = GetComponent<CinemachineOrbitalFollow>();

        Cursor.lockState = CursorLockMode.Locked;
        _currentZoom = _cinemachineOrbitFollow.Radius;
        _targetZoom = _currentZoom;
    }

    // Update is called once per frame
    void Update()
    {
        UpdateMouseZoom();
    }

    private void UpdateMouseZoom()
    {

        _scrollDelta = _inputManager.GetCameraZoom();

        if(_scrollDelta.y != 0)
        {
            if(_cinemachineOrbitFollow != null)
            {
                //Clamping the target zoom, helping it stays inside the threshold. 
                _targetZoom = Mathf.Clamp(_cinemachineOrbitFollow.Radius - _scrollDelta.y * _zoomScrollSpeed, _minZoomDistance, _maxZoomDistance);

                //It's important to reset the value so the _scrollDelta doesn't increase more than necessary.
                _scrollDelta = Vector2.zero;
            }
        }

        //Lerping between the current orbital radius to the _targetZoom
        _currentZoom = Mathf.Lerp(_currentZoom, _targetZoom, Time.deltaTime * _zoomLerpSpeed);

        _cinemachineOrbitFollow.Radius = _currentZoom;
    }
}
