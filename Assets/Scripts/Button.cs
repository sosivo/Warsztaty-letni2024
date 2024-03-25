using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Button : MonoBehaviour
{
    [SerializeField]
    List<ObjectInteractable> KeyActivatedObjects;
    [SerializeField]
    Rigidbody2D _movingPartRb;
    [SerializeField]
    Transform _buttonCenter;
    [SerializeField]
    float _requierdMass;
    [SerializeField]
    float _desactivatedOffset;
    bool _turnedOn = false;
    // Start is called before the first frame update
    void Start()
    {
        if(_movingPartRb == null)
            _movingPartRb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Vector2 toCenterVector = (Vector2)(_buttonCenter.position - _movingPartRb.transform.position);
        Vector2 toOffsetVector = (Vector2)(_buttonCenter.position + (_desactivatedOffset *_buttonCenter.up) - _movingPartRb.transform.position);
        _movingPartRb.transform.position = _buttonCenter.position + Vector3.Dot(_buttonCenter.up, -toCenterVector) * _buttonCenter.up;
        _movingPartRb.AddForce(toOffsetVector * _requierdMass * toOffsetVector.magnitude/_desactivatedOffset,ForceMode2D.Impulse);
        if (_turnedOn && Vector2.Dot((Vector2)_buttonCenter.up, toCenterVector) < 0f)
            TurnOff();
        if (!_turnedOn && Vector2.Dot((Vector2)_buttonCenter.up, toCenterVector) > 0f)
            TurnOn();


    }
    void TurnOn()
    {
        _turnedOn = true;
        foreach(var obj in KeyActivatedObjects)
        {
            obj.OnInteraction();
        }
    }
    void TurnOff()
    {
        _turnedOn = false;
        foreach (var obj in KeyActivatedObjects)
        {
            obj.OffInteraction();
        }
    }
    private void OnDrawGizmosSelected()
    {
        foreach (var obj in KeyActivatedObjects)
        {
            Gizmos.color = Color.cyan;
            if (obj != null)
                Gizmos.DrawLine(transform.position, obj.transform.position);
        }
    }
}
