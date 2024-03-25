using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingObject : ObjectInteractable
{
    [SerializeField]
    float _moveSpeed = 1f;
    [SerializeField]
    Rigidbody2D _rb;
    Vector2 _origin;
    public Vector2 Destination;
    bool _originFlag = true;
    // Start is called before the first frame update
    void Start()
    {
        _origin = transform.position;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        float closeToZero = _moveSpeed * Time.fixedDeltaTime > Physics2D.gravity.magnitude * Time.fixedDeltaTime * Time.fixedDeltaTime ?
            _moveSpeed * Time.fixedDeltaTime : Physics2D.gravity.magnitude * Time.fixedDeltaTime * Time.fixedDeltaTime;
        closeToZero += 0.05f;
        const float acceleration = 10f;
        Vector2 desiredRelocation = Vector2.zero;

        if (_originFlag)
        {
            desiredRelocation = (_origin - (Vector2)transform.position);
        }
        else
        {
            desiredRelocation = (Destination - (Vector2)transform.position);
        }

        if (desiredRelocation.magnitude > closeToZero)
        {
            _rb.velocity = Vector2.Lerp(_rb.velocity, desiredRelocation.normalized * _moveSpeed,acceleration*Time.fixedDeltaTime);
        }
        else
        {
            _rb.velocity = Vector2.Lerp(_rb.velocity, desiredRelocation, acceleration * Time.deltaTime);
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
            Gizmos.DrawLine(transform.position, Destination);
            Gizmos.DrawWireCube(transform.position, boxSize);
        }
        else
        {
            Gizmos.DrawLine(_origin, Destination);
            Gizmos.DrawWireCube(_origin, boxSize);
        }
        Gizmos.DrawWireCube(Destination, boxSize);
    }
}
