using UnityEngine;

public class CharacterMovement : MonoBehaviour
{
    [Header("Player Atrributes")]
    [SerializeField] private float _movementSpeed;
    [SerializeField] private float _jumpForce;
    [SerializeField] private float _rotationSpeed;
    private float _fallVelocity;
    //necessary for triggering the tile correctly
    private bool _hasJumped = false;

    [Header("ChrMovement Class Reference")]
    [SerializeField] private GameInputManager _inputManagerRef;
    [SerializeField] private Transform _cameraTransform;
    private GroundDetection _groundDetection;

    private Rigidbody _rb;
    private Tile _tileJumped;

    private bool _isGameFinished = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _rb = GetComponent<Rigidbody>();
        _groundDetection = GetComponent<GroundDetection>();
    }

    private void OnEnable()
    {
        EventManager.OnGameOver += OnGameOverTriggered;
        EventManager.OnGameWon += OnGameWonTriggered;
    }

    private void OnDisable()
    {
        EventManager.OnGameOver -= OnGameOverTriggered;
        EventManager.OnGameWon -= OnGameWonTriggered;
    }

    private void Update()
    {
        if (_groundDetection.IsOnGround(out _tileJumped) && _fallVelocity < 0f && _hasJumped)
        {
            _hasJumped = false;
            if(_tileJumped != null)
            {
                EventManager.TileJumped(_tileJumped);
            }
        }
    }

    private void FixedUpdate()
    {
        PlayerMovementAligned();

        _fallVelocity = _rb.linearVelocity.y;

        if (_groundDetection.IsOnGround(out _tileJumped) && _inputManagerRef.IsJumping())
        {
            PlayerJump();
        }
    }
    private void OnGameOverTriggered()
    {
        _isGameFinished = true;
    }

    private void OnGameWonTriggered()
    {
        _isGameFinished = true;
    }

    //This method move the player based in the direction which the camera is facing.
    private void PlayerMovementAligned()
    {
        if(_isGameFinished) { return; }

        Vector3 forward = _cameraTransform.forward;
        Vector3 right = _cameraTransform.right;

        forward.y = 0f;
        right.y = 0f;

        forward.Normalize();
        right.Normalize();

        Vector3 input = _inputManagerRef.GetDirectionValue();
        Vector3 move = (right * input.x + forward * input.y).normalized * _movementSpeed;

        _rb.linearVelocity = new Vector3(move.x, _rb.linearVelocity.y , move.z);

    }

    private void PlayerJump()
    {
        _rb.AddForce(Vector3.up * _jumpForce, ForceMode.Impulse);
        _hasJumped = true;
    }
}
