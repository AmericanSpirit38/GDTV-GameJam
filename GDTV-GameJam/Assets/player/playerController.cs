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

    [SerializeField]private bool isOnWall;
    public float xScale = 1f;
    public float yScale = 1f;
    private bool canSetRotNScaleDefault;
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
        Vector2 velocity = Vector2.zero;
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
            if (transform.eulerAngles.z == 90)
            {
                velocity += new Vector2(-jumpForce, jumpForce);
                Debug.Log("jumped from right wall");
            }
        }
       //if (Input.GetButtonUp("Jump") && rb.velocity.y > 0)
        //{
         //   rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * 0.5f);

        //}

        //------------------MOVEMENT---------------------------------------------------------------
        float input = Input.GetAxis("Horizontal"); // -1 (left) to +1 (right)
        Vector2 moveDirection = (Vector2)transform.right * input;
        if (transform.eulerAngles.z == 90 || transform.eulerAngles.z == -90)
        {
            isOnWall = true;
        }
        else
        {
            isOnWall = false;
        }
        if (isOnWall)
        {
            velocity += moveDirection * moveSpeed;
        }
        else
        {
            velocity += new Vector2(moveDirection.x * moveSpeed, rb.velocity.y);
        }
        
        rb.velocity = velocity;
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

            Vector2 normal = collision.contacts[0].normal;
            Vector2 roundedNormal = new Vector2(Mathf.Round(normal.x), Mathf.Round(normal.y));
            if (roundedNormal == Vector2.down) 
            {
                transform.localScale = new Vector3(transform.localScale.x, -yScale, 0);
            }
            if (roundedNormal == Vector2.left)
            {
                transform.rotation = Quaternion.Euler(0,0,90);
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

}
