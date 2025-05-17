using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour
{
    public Rigidbody2D rb;
    public float moveSpeed;
    public float jumpForce;
    [SerializeField]private float jumpBufferCounter;

    [SerializeField]private bool isGrounded;
    [SerializeField]private bool deactivateGravity;

    public Transform gravityDeactivationPoint;
    public Transform groundCheckPoint;
    public float groundCheckRadius;
    public LayerMask whatIsGround;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //------------------MOVEMENT---------------------------------------------------------------
        isGrounded = Physics2D.OverlapCircle(groundCheckPoint.position, groundCheckRadius, whatIsGround);
        deactivateGravity = Physics2D.OverlapCircle(gravityDeactivationPoint.position, groundCheckRadius, whatIsGround);
        if (deactivateGravity) 
        {
            rb.gravityScale = 0;
        }
        else
        {
            rb.gravityScale = 2.3f;
        }
        if (Input.GetButtonDown("Jump"))
        {
            jumpBufferCounter = 0.15f;
        }
        else
        {
            jumpBufferCounter -= Time.deltaTime;
        }
        if (isGrounded == true && jumpBufferCounter>0)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        }

        //if (Input.GetButtonUp("Jump") && rb.velocity.y > 0)
        //{
         //   rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * 0.5f);

        //}
        rb.velocity = new Vector2(Input.GetAxisRaw("Horizontal")* moveSpeed, rb.velocity.y);
    }
}
