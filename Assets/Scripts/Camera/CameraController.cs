using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [field: SerializeField] public Transform TrackedObject { get; private set; }
    [field: SerializeField] public float CameraDynamic { get; private set; } = 4f;

    private float initialZ;
    
    // Start is called before the first frame update
    void Start()
    {
        initialZ = transform.position.z;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Vector3.Lerp(transform.position, TrackedObject.position, Time.deltaTime * CameraDynamic);
        transform.position = new Vector3(transform.position.x, transform.position.y, initialZ);
    }
}
