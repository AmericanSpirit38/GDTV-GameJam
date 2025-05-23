using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class NewBehaviourScript : MonoBehaviour
{
    public Transform[] childrenToDetach;
    [Header("movement")]
    public Rigidbody2D rb;
    public float moveSpeed;
    public float jumpForce;
    [SerializeField] private Vector2 velocity;
    [SerializeField] private Vector2 inputVelocity;
    [SerializeField] private float jumpBufferCounter;

    [Header("touching")]
    public Transform topPoint;
    public Transform leftPoint;
    public Transform rightPoint;
    public Transform bottomPoint;
    public bool isTouchingRight;
    public bool isTouchingTop;
    public bool isTouchingLeft;
    public bool isTouchingBottom;
    [SerializeField] private bool isOnWall;
    [SerializeField] private bool isGrounded;
    public float groundCheckRadius;
    public LayerMask whatIsGround;



    public float xScale = 1f;
    public float yScale = 1f;
    [SerializeField] private bool canSetRotNScaleDefault;
    public bool shouldFlip;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //------------------GRAVITY---------------------------------------------------------------
        isTouchingBottom = Physics2D.OverlapCircle(bottomPoint.position, groundCheckRadius, whatIsGround);
        isTouchingTop = Physics2D.OverlapCircle(topPoint.position, groundCheckRadius, whatIsGround);
        isTouchingRight = Physics2D.OverlapCircle(rightPoint.position, groundCheckRadius, whatIsGround);
        isTouchingLeft = Physics2D.OverlapCircle(leftPoint.position, groundCheckRadius, whatIsGround);
        isGrounded = isTouchingBottom == true || isTouchingTop == true || isTouchingRight == true || isTouchingLeft == true;
        if (isGrounded)
        {
            rb.gravityScale = 0;
        }
        else
        {
            rb.gravityScale = 2.7f;
            transform.localScale = new Vector3(transform.localScale.x, yScale, 0);
            transform.rotation = Quaternion.identity;
            
        }

        //------------------MOVEMENT---------------------------------------------------------------
        float input = Input.GetAxis("Horizontal"); // -1 (left) to +1 (right)
        Vector2 moveDirection = (Vector2)transform.right * input;
        if (isTouchingBottom)
        {
            transform.localScale = new Vector3(transform.localScale.x, yScale, 0);
        }
        if (isTouchingLeft)
        {
            transform.rotation = Quaternion.Euler(0, 0, 90);
            isOnWall = true;
            canSetRotNScaleDefault = false;
        }
        if (isTouchingRight)
        {
            transform.rotation = Quaternion.Euler(0, 0, 270);
            transform.localScale = new Vector3(-xScale, transform.localScale.y, 0);
            isOnWall = true;
            canSetRotNScaleDefault = false;

        }
        if (isTouchingTop)
        {
            transform.rotation = Quaternion.Euler(0, 0, 0);
            transform.localScale = new Vector3(-xScale, transform.localScale.y, 0);
            canSetRotNScaleDefault = false;

        }
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
