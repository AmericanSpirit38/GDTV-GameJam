using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class NewBehaviourScript : MonoBehaviour
{
    public Rigidbody2D rb;
    public float moveSpeed;
    public float jumpForce;
    [SerializeField] private float jumpBufferCounter;

    private bool isWallJumping;
    private float wallJumpingDirection;
    private float wallJumpingTime = 0.2f;
    private float wallJumpingCounter;
    private float wallJumpingDuration = 0.4f;
    private Vector2 wallJumpingPower = new Vector2(8f, 16f);

    [SerializeField] private bool isGrounded;
    [SerializeField] private bool deactivateGravity;
    public Transform[] childrenToDetach;

    [SerializeField] private Vector2 velocity;
    [SerializeField] private Vector2 inputVelocity;
    [SerializeField] private Vector2 savedVelocity; // toto pouzijem na defaultne nastavenie dalsej velocity ku ktorej potom pripocitam movement

    [SerializeField] private bool isOnWall;
    public float xScale = 1f;
    public float yScale = 1f;
    [SerializeField] private bool canSetRotNScaleDefault;
    public bool shouldFlip;
    public Transform topPoint;
    public Transform leftPoint;
    public Transform rightPoint;
    public Transform bottomPoint;
    public float groundCheckRadius;
    public LayerMask whatIsGround;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //------------------GRAVITY---------------------------------------------------------------
        deactivateGravity = Physics2D.OverlapCircle(bottomPoint.position, groundCheckRadius, whatIsGround);
        if (deactivateGravity)
        {
            rb.gravityScale = 0;
        }
        else
        {
            rb.gravityScale = 2.7f;
            if (canSetRotNScaleDefault)
            {
                transform.localScale = new Vector3(transform.localScale.x, yScale, 0);
                transform.rotation = Quaternion.identity;
            }
        }

        //------------------MOVEMENT---------------------------------------------------------------
        float input = Input.GetAxis("Horizontal"); // -1 (left) to +1 (right)
        Vector2 moveDirection = (Vector2)transform.right * input;
        if (transform.eulerAngles.z == 90 || transform.eulerAngles.z == 270)
        {
            //Debug.Log("isOnWall to true");
            isOnWall = true;
        }
        else
        {
            isOnWall = false;
        }
        if (isOnWall)
        {
            inputVelocity = moveDirection * moveSpeed;
        }
        else
        {
            inputVelocity = new Vector2(moveDirection.x * moveSpeed, rb.velocity.y);
        }
        rb.velocity = inputVelocity;

        if (transform.eulerAngles.z == 90 || transform.eulerAngles.z == 270)
        {
            if (rb.velocity.x < 0)
            {
                transform.localScale = new Vector3(xScale, transform.localScale.y, 0);
            }
            if (rb.velocity.x > 0)
            {
                transform.localScale = new Vector3(-xScale, transform.localScale.y, 0);
            }
        }
        else
        {
            if (rb.velocity.x > 0)
            {
                transform.localScale = new Vector3(xScale, transform.localScale.y, 0);
            }
            if (rb.velocity.x < 0)
            {
                transform.localScale = new Vector3(-xScale, transform.localScale.y, 0);
            }
        }

        //------------------JUMPING---------------------------------------------------------------
        if (Input.GetButtonDown("Jump"))
        {
            jumpBufferCounter = 0.15f;
        }
        else
        {
            jumpBufferCounter -= Time.deltaTime;
        }
        if (isGrounded == true && jumpBufferCounter > 0)
        {
            Jump();

        }
    }
  
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("ground"))
        {
            isGrounded = true;

            Vector2 normal = collision.contacts[0].normal;
            Vector2 roundedNormal = new Vector2(Mathf.Round(normal.x), Mathf.Round(normal.y));
            if (roundedNormal == Vector2.down)
            {
                transform.localScale = new Vector3(transform.localScale.x, -yScale, 0);
            }
            if (roundedNormal == Vector2.left)
            {
                transform.rotation = Quaternion.Euler(0, 0, 90);
                canSetRotNScaleDefault = false;
            }
            if (roundedNormal == Vector2.right)
            {
                transform.rotation = Quaternion.Euler(0, 0, 270);
                transform.localScale = new Vector3(-xScale, transform.localScale.y, 0);
                canSetRotNScaleDefault = false;

            }
            if (roundedNormal == Vector2.up)
            {
                transform.rotation = Quaternion.Euler(0, 0, 0);
                transform.localScale = new Vector3(-xScale, transform.localScale.y, 0);
                canSetRotNScaleDefault = false;

            }


        }
    }
    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("ground"))
        {
            isGrounded = true;
        }
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("ground"))
        {
            isGrounded = false;
            canSetRotNScaleDefault = true;

        }
    }
    private void Jump()
    {
        Debug.Log("jump");
        if (!isOnWall) 
        {
            Vector2 v = rb.velocity;
            v.y = 0f;
            v.y += jumpForce;
            rb.velocity = v;
        }
        // handle wall jump
    }
    public void DetachSpecificChildren()
    {
        foreach (Transform child in childrenToDetach)
        {
            if (child != null && child.parent == transform)
            {
                child.SetParent(null);
            }
        }
    }
    public void AttachSpecificChildren()
    {
        foreach (Transform child in childrenToDetach)
        {
            if (child != null)
            {
                child.SetParent(transform);
            }
        }
    }
}
