using UnityEngine;

public class GroundDetection : MonoBehaviour
{
    Rigidbody _rb;
    CapsuleCollider _capsuleCollider;

    private void Start()
    {
        _rb = GetComponent<Rigidbody>();
        _capsuleCollider = GetComponent<CapsuleCollider>();
    }


    public bool IsOnGround(out Tile tileJumped)
    {
        const string FIELD_LAYER = "Field";

        //Returns the center position of the capsule, the _rb.rotation will only be applied if the object is rotated, if not it's just _rb.position + 0,0,0 since _capsuleCollider.center is 0,0,0
        Vector3 capsuleWorldCenter = _rb.position + _rb.rotation * _capsuleCollider.center;

        //_capsuleHeight is 2 so 2/2 is 1 - r(radius) that is 0.5 we get 0.5
        //_capsuleWorldCenter +(-) V3.UP helps us getting to the bottom and top spheres of the capsuleCollider.
        Vector3 p1 = capsuleWorldCenter + Vector3.up * (_capsuleCollider.height / 2 - _capsuleCollider.radius);

        Vector3 p2 = capsuleWorldCenter - Vector3.up * (_capsuleCollider.height / 2 - _capsuleCollider.radius);

        Collider[] hits = Physics.OverlapCapsule(p1, p2, _capsuleCollider.radius, LayerMask.GetMask(FIELD_LAYER));

        //if (hits.Length > 0 && hits[0].GetComponent<Tile>() != null)
        //{
        //    //Debug.Log(hits[0].name);
        //    //Call the event
            
        //}
        if(hits.Length > 0)
        {
            tileJumped = hits[0]?.GetComponent<Tile>();
            return true;
        }

        tileJumped = null;
        return false;
    }
}
