using UnityEngine;

public class CharacterMovement : MonoBehaviour
{
    [Header("Player Atrributes")]
    [SerializeField] private float _movementSpeed;
    [SerializeField] private float _jumpForce;
    [SerializeField] private float _rotationSpeed;

    [Header("ChrMovement Class Reference")]
    [SerializeField] private GameInputManager _inputManagerRef;
    [SerializeField] private Transform _cameraTransform;

    private Rigidbody _rb;
    private CapsuleCollider _capsuleCollider;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _rb = GetComponent<Rigidbody>();
        _capsuleCollider = GetComponent<CapsuleCollider>();
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

    //private void MovePlayer()
    //{

    //    _rb.linearVelocity = new Vector3(_inputManagerRef.GetDirectionValue().x * _movementSpeed, _rb.linearVelocity.y, _inputManagerRef.GetDirectionValue().y * _movementSpeed);
    //}

    //This method move the player based in the direction which the camera is facing.
    private void PlayerMovementAligned()
    {
        Vector3 forward = _cameraTransform.forward;
        Vector3 right = _cameraTransform.right;

        forward.y = 0f;
        right.y = 0f;

        //Testing phase, the player moves aligned with the camera Z and X axis.
        _rb.linearVelocity = new Vector3(right.x * _inputManagerRef.GetDirectionValue().x * _movementSpeed, _rb.linearVelocity.y, forward.z * _inputManagerRef.GetDirectionValue().y * _movementSpeed);

    }

    private void PlayerJump()
    {
        _rb.AddForce(Vector3.up * _jumpForce, ForceMode.Impulse);
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

        bool isOnGround = Physics.CheckCapsule(p1, p2, _capsuleCollider.radius + 0.02f, LayerMask.GetMask(FIELD_LAYER));

        return isOnGround;
    }

}
