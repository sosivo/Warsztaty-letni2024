using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField]
    Rigidbody2D _rb;
    // Start is called before the first frame update
    void Start()
    {
        //Fool proof for the sake of flex xd
        if (_rb == null)
        {
            if (TryGetComponent<Rigidbody2D>(out Rigidbody2D rb))
            {
                _rb = rb;
            }
            else
            {
                _rb = transform.AddComponent<Rigidbody2D>();
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        const float movementScalar = 4f;
        const float jumpScalar = 8f;
        Vector2 desiredVelocity = Vector2.zero;

        //allows MultiJump for testing
        if(Input.GetKeyDown(KeyCode.W))
            desiredVelocity += Vector2.up * jumpScalar;
        if (Input.GetKey(KeyCode.A))
            desiredVelocity += Vector2.left * movementScalar;
        if (Input.GetKey(KeyCode.D))
            desiredVelocity += Vector2.right * movementScalar;
        _rb.velocity = desiredVelocity + new Vector2(0f,_rb.velocity.y);

        if(Input.GetKeyDown(KeyCode.Space))
        {
            Interact();
        }
    }

    void Interact()
    {
        RaycastHit2D[] hits = Physics2D.BoxCastAll(transform.position, new Vector2(2f, 2f), 0f, Vector2.zero,0f);
        foreach(RaycastHit2D hit in hits)
        { 
            if(hit.transform.TryGetComponent<PlayerInteractable>(out PlayerInteractable playerInteractable))
            {
                playerInteractable.OnInteract();
            }
        }
    }
}
