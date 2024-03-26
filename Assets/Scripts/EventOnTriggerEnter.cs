using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EventOnTriggerEnter : MonoBehaviour
{
    [SerializeField]
    UnityEvent TriggerEnterEvent = new UnityEvent();
    private void OnTriggerEnter2D(Collider2D collision)
    {
        TriggerEnterEvent.Invoke();
    }
}
