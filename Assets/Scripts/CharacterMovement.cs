using UnityEngine;

public class CharacterMovement : MonoBehaviour
{
    [Header("Player Atrributes")]
    [SerializeField] private float _movementSpeed;
    [SerializeField] private float _jumpForce;
    [SerializeField] private float _rotationSpeed;
    //necessary for triggering the tile correctly
    private bool _hasJumped =false;

    [Header("ChrMovement Class Reference")]
    [SerializeField] private GameInputManager _inputManagerRef;
    [SerializeField] private Transform _cameraTransform;

    private Rigidbody _rb;
    private CapsuleCollider _capsuleCollider;

    private bool _isGameFinished = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _rb = GetComponent<Rigidbody>();
        _capsuleCollider = GetComponent<CapsuleCollider>();
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

    private void FixedUpdate()
    {
        //MovePlayer();
        PlayerMovementAligned();
        if (IsOnGround() && _inputManagerRef.IsJumping())
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

        //Testing phase, the player moves aligned with the camera Z and X axis.

        Vector3 input = _inputManagerRef.GetDirectionValue();
        Vector3 move = (right * input.x + forward * input.y).normalized * _movementSpeed;

        _rb.linearVelocity = new Vector3(move.x, _rb.linearVelocity.y, move.z);

    }

    private void PlayerJump()
    {
        _rb.AddForce(Vector3.up * _jumpForce, ForceMode.Impulse);
        _hasJumped = true;
    }

    private bool IsOnGround()
    {
        const string FIELD_LAYER = "Field";

        //Returns the center position of the capsule, the _rb.rotation will only be applied if the object is rotated, if not it's just _rb.position + 0,0,0 since _capsuleCollider.center is 0,0,0
        Vector3 capsuleWorldCenter = _rb.position + _rb.rotation * _capsuleCollider.center;

        //_capsuleHeight is 2 so 2/2 is 1 - r(radius) that is 0.5 we get 0.5
        //_capsuleWorldCenter +(-) V3.UP helps us getting to the bottom and top spheres of the capsuleCollider.
        Vector3 p1 = capsuleWorldCenter + Vector3.up * (_capsuleCollider.height / 2 - _capsuleCollider.radius);

        Vector3 p2 = capsuleWorldCenter - Vector3.up * (_capsuleCollider.height / 2 - _capsuleCollider.radius);

        //bool isOnGround = Physics.CheckCapsule(p1, p2, _capsuleCollider.radius + 0.02f, LayerMask.GetMask(FIELD_LAYER));

        Collider[] hits = Physics.OverlapCapsule(p1, p2, _capsuleCollider.radius, LayerMask.GetMask(FIELD_LAYER));
        
        if(hits.Length > 0 && hits[0].GetComponent<Tile>() != null && _hasJumped)
        {
            //Debug.Log(hits[0].name);
            //Call the event
            EventManager.TileJumped(hits[0].GetComponent<Tile>());
            _hasJumped = false;
        }

        return hits.Length > 0;
    }

}
