using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [field: SerializeField] public Transform TrackedObject { get; set; }
    [field: SerializeField] public float CameraDynamic { get; private set; } = 4f;

    private float initialZ;
    private Vector2 targetPosition;
    
    // Start is called before the first frame update
    void Start()
    {
        initialZ = transform.position.z;
    }

    // Update is called once per frame
    void Update()
    {
        if (TrackedObject is not null)
            targetPosition = TrackedObject.transform.position;
        
        transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime * CameraDynamic);
        transform.position = new Vector3(transform.position.x, transform.position.y, initialZ);
    }
}
