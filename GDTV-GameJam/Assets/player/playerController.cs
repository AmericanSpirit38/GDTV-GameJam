using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour
{
    public Rigidbody2D rb;
    public float moveSpeed;
    public float jumpForce;
    [SerializeField] private float jumpBufferCounter;

    [SerializeField] private bool isGrounded;
    [SerializeField] private bool deactivateGravity;
    public Transform[] childrenToDetach;

    public float xScale = 1.0492f;
    public float yScale = 0.6278f;

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
        //------------------MOVEMENT---------------------------------------------------------------
        deactivateGravity = Physics2D.OverlapCircle(bottomPoint.position, groundCheckRadius, whatIsGround);
        if (deactivateGravity)
        {
            rb.gravityScale = 0;
        }
        else
        {
            rb.gravityScale = 2.7f;
        }
        transform.localScale = new Vector3(transform.localScale.x, -yScale, 0);
        transform.localScale = new Vector3(transform.localScale.x, yScale, 0);


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
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        }

        if (Input.GetButtonUp("Jump") && rb.velocity.y > 0)
        {
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * 0.5f);

        }

        rb.velocity = new Vector2(Input.GetAxisRaw("Horizontal") * moveSpeed, rb.velocity.y);
        if (rb.velocity.x > 0)
        {
            transform.localScale = new Vector3(xScale, transform.localScale.y, 0);
        } 
        if (rb.velocity.x < 0)
        {
            transform.localScale = new Vector3(-xScale, transform.localScale.y, 0);
        }
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

    private void OnCollisionEnter2D(Collision2D collision)
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
        }
    }

}
