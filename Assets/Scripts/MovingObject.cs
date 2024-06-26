using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingObject : ObjectInteractable
{
    [SerializeField]
    float _moveSpeed = 1f;
    [SerializeField]
    float _acceleration = 10f;
    [SerializeField]
    Rigidbody2D _rb;
    Vector2 _origin,_destination;
    public Vector2 DestinationDelta;
    bool _originFlag = true;
    // Start is called before the first frame update
    void Start()
    {
        _origin = transform.position;
        _destination = transform.position + (Vector3)DestinationDelta;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        float closeToZero = _moveSpeed * Time.fixedDeltaTime > Physics2D.gravity.magnitude * Time.fixedDeltaTime * Time.fixedDeltaTime ?
            _moveSpeed * Time.fixedDeltaTime : Physics2D.gravity.magnitude * Time.fixedDeltaTime * Time.fixedDeltaTime;
        closeToZero += 0.05f;
        Vector2 desiredRelocation = Vector2.zero;

        if (_originFlag)
        {
            desiredRelocation = (_origin - (Vector2)transform.position);
        }
        else
        {
            desiredRelocation = (_destination - (Vector2)transform.position);
        }

        if (desiredRelocation.magnitude > closeToZero)
        {
            _rb.velocity = Vector2.Lerp(_rb.velocity, desiredRelocation.normalized * _moveSpeed,_acceleration*Time.fixedDeltaTime);
        }
        else
        {
            _rb.velocity = Vector2.Lerp(_rb.velocity, desiredRelocation, _acceleration * Time.deltaTime);
            //transform.position = (Vector2)transform.position + desiredRelocation;
        }
    }
    public override void OnInteraction()
    {
        _originFlag = false;
    }
    public override void OffInteraction()
    {
        _originFlag = true;
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Vector2 boxSize = Vector2.zero;
        if(TryGetComponent(out BoxCollider2D collider))
        {
            boxSize = collider.transform.localScale;
        }
        if (!Application.isPlaying)
        {
            Gizmos.DrawLine(transform.position, transform.position + (Vector3)DestinationDelta);
            Gizmos.DrawWireCube(transform.position, boxSize);
            Gizmos.DrawWireCube(transform.position + (Vector3)DestinationDelta, boxSize);
        }
        else
        {
            Gizmos.DrawLine(_origin, _destination);
            Gizmos.DrawWireCube(_origin, boxSize);
            Gizmos.DrawWireCube(_destination, boxSize);
        }
    }
}
