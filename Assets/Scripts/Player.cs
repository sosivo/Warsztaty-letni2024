using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Player : MonoBehaviour
{
    public Rigidbody2D Rb;
    private Rigidbody2D _heldRb;

    private Collider2D col;
    [SerializeField] private float groundCheckDistance = 0.1f;

    private readonly RaycastHit2D[] groundHitCheckResults = new RaycastHit2D[10];

    [SerializeField] private float movementSpeed = 4f;
    [SerializeField] private float jumpForce = 8f;

    [SerializeField] private float horizontalVelocityLimit = 5f;
    [SerializeField] private float horizontalDrag = 40f;

    [SerializeField] private float jumpHeldGravityScale = 0.8f;
    [SerializeField] private float jumpReleasedGravityScale = 1.6f;

    [SerializeField] private float jumpBuffer = 0.2f;
    private bool hasJumped;
    private float lastJumpTime;
    
    // Start is called before the first frame update
    private void Start()
    {
        //Fool proof for the sake of flex xd
        if (Rb == null)
        {
            if (TryGetComponent(out Rigidbody2D rb))
            {
                Rb = rb;
            }
            else
            {
                Rb = transform.AddComponent<Rigidbody2D>();
            }
        }

        col = GetComponent<Collider2D>();
    }

    // Update is called once per frame
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.W))
            lastJumpTime = Time.time;
            
        if (CanJump())
        {
            if (IsGrounded())
            {
                Rb.velocity = new Vector2(Rb.velocity.x, 0);
                Rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
                hasJumped = true;
            }
        }
            

        if (Rb.velocity.y < -Mathf.Epsilon) hasJumped = false;

        Rb.gravityScale = (Input.GetKey(KeyCode.W), Rb.velocity.y > Mathf.Epsilon) switch
        {
            (true, true) => jumpHeldGravityScale,
            (false, true) => jumpReleasedGravityScale,
            _ => 1
        };

        if (Input.GetKey(KeyCode.A))
            Rb.AddForce(Vector2.left * movementSpeed, ForceMode2D.Force);
        if (Input.GetKey(KeyCode.D))
            Rb.AddForce(Vector2.right * movementSpeed, ForceMode2D.Force);

        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (_heldRb == null)
                Interact();
            else
                DropItem();

        }

        // CameraMovement();
    }
    private void FixedUpdate()
    {
        Rb.velocity = new Vector2(Mathf.Clamp(Rb.velocity.x, -horizontalVelocityLimit, horizontalVelocityLimit) * horizontalDrag * Time.fixedDeltaTime,
            Rb.velocity.y);
        
        HeldItemPhysics();
    }

    private bool CanJump() => Input.GetKey(KeyCode.W) && Time.time - lastJumpTime < jumpBuffer && !hasJumped;

    private void Interact()
    {
        RaycastHit2D[] hits = Physics2D.BoxCastAll(transform.position, new Vector2(2f, 2f), 0f, Vector2.zero,0f);
        foreach(RaycastHit2D hit in hits)
        { 
            if(hit.transform.TryGetComponent(out PlayerInteractable playerInteractable))
            {
                playerInteractable.OnInteraction(this);
            }
        }
    }
    public void PickUp(Rigidbody2D pickedRb)
    {
        if (_heldRb != null)
        {
            if (((Vector2)(_heldRb.transform.position - transform.position)).magnitude < ((Vector2)(pickedRb.transform.position - transform.position)).magnitude)
                return;
            DropItem();
        }
        _heldRb = pickedRb;
        Rb.mass += _heldRb.mass;
        _heldRb.simulated = false;
    }

    private void DropItem()
    {
        Rb.mass -= _heldRb.mass;
        _heldRb.simulated = true;
        _heldRb.velocity = Vector3.zero;
        _heldRb = null;
    }

    private void HeldItemPhysics()
    {
        if (_heldRb == null)
            return;
        _heldRb.transform.position = transform.position + Vector3.up * 1.5f;
    }

    private bool IsGrounded() => col.Cast(Vector2.down, groundHitCheckResults, groundCheckDistance) > 0;
}
