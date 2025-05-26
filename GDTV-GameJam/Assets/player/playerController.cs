using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class playerController : MonoBehaviour
{
    public static playerController instance;
    private void Awake()
    {
        instance = this;
    }
    public Animator anim;
    [Header("movement")]
    public Rigidbody2D rb;
    public float moveSpeed;
    public float jumpForce;
    [SerializeField] private Vector2 velocity;
    [SerializeField] private Vector2 inputVelocity;
    [SerializeField] private Vector2 LastInputVelocity;
    [SerializeField] private float jumpBufferCounter;
    [SerializeField] private float wallJumpLockTime = 0.4f;
    private float wallJumpLockCounter = 0f;

    [Header("touching")]
    public Transform topPoint;
    public Transform topPoint2;
    public Transform rightPoint;
    public Transform bottomPoint;
    public Transform bottomPoint2;
    public bool isTouchingRight;
    public bool isTouchingTop;
    public bool isTouchingBottom;
    public Collider2D target;
    [SerializeField] private bool isOnWall;
    [SerializeField] private bool isGrounded;
    public float groundCheckRadius;
    public LayerMask whatIsGround;
    public enum wallTypes { top, bottom, left, right, none }; //default je 
    public wallTypes currentWall;

    public float xScale = 1.5f;
    public float yScale = 1.5f;


    // Start is called before the first frame update
    void Start()
    {
        currentWall = wallTypes.bottom;
    }

    // Update is called once per frame
    void Update()
    {
        if (rb.velocity.x != 0 && isGrounded)
        {
            anim.SetBool("isWalking", true);
        }
        else
        {
            anim.SetBool("isWalking", false);
        }
        //------------------GRAVITY---------------------------------------------------------------
        isTouchingBottom = Physics2D.OverlapCircle(bottomPoint.position, groundCheckRadius, whatIsGround) || Physics2D.OverlapCircle(bottomPoint2.position, groundCheckRadius, whatIsGround);
        isTouchingTop = Physics2D.OverlapCircle(topPoint.position, groundCheckRadius, whatIsGround) || Physics2D.OverlapCircle(topPoint2.position, groundCheckRadius, whatIsGround);
        isTouchingRight = Physics2D.OverlapCircle(rightPoint.position, groundCheckRadius, whatIsGround);
        target = Physics2D.OverlapCircle(bottomPoint.position, groundCheckRadius, whatIsGround);
        if (target == null)
        {
            target = Physics2D.OverlapCircle(bottomPoint2.position, groundCheckRadius, whatIsGround);
        }
        isGrounded = isTouchingBottom == true || isTouchingTop == true || isTouchingRight == true;
        if(isTouchingBottom == false && isTouchingTop == false && isTouchingRight == false)
        {
            isOnWall = false;
            isGrounded = false;
        }
        if (isTouchingBottom && transform.eulerAngles.z == 0)
        {
            isOnWall = false;
            currentWall = wallTypes.bottom;
        }
        if (isGrounded)
        {
            rb.gravityScale = 0;
        }
        else //rese
        {
            currentWall = wallTypes.none;
            rb.gravityScale = 2.7f;
            transform.localScale = new Vector3(transform.localScale.x, yScale, 0);
            transform.rotation = Quaternion.identity;
        }

        //------------------ROTATION AND WALLS---------------------------------------------------------------
        if (isTouchingRight)
        {
            if (currentWall == wallTypes.bottom || currentWall == wallTypes.none|| currentWall == wallTypes.top)
            {
                if (rb.velocity.x > 0)
                {
                    transform.rotation = Quaternion.Euler(0, 0, 90);
                    isOnWall = true;
                    currentWall = wallTypes.right;
                    Debug.Log("to right");
                }
                else if (rb.velocity.x < 0)
                {
                    transform.rotation = Quaternion.Euler(0, 0, 270);
                    transform.localScale = new Vector3(-xScale, transform.localScale.y, 0);
                    isOnWall = true;
                    currentWall = wallTypes.left;
                    Debug.Log("to left");
                }
            }
            else if (currentWall == wallTypes.left || currentWall == wallTypes.right)
            {
                if (rb.velocity.y > 0)
                {
                    currentWall = wallTypes.top;
                    transform.rotation = Quaternion.Euler(0, 0, 180);
                    transform.localScale = new Vector2(-xScale, yScale);
                    Debug.Log("to to top");
                }
                else
                {
                    currentWall = wallTypes.bottom;
                    transform.rotation = Quaternion.Euler(0, 0, 0);
                    isOnWall = false;
                    Debug.Log("to bottom");
                }
            }
        }
        else if (isTouchingTop)
        {
            currentWall = wallTypes.top;
            transform.rotation = Quaternion.Euler(0, 0, 180);
            transform.localScale = new Vector2(-xScale, yScale);
            isOnWall = true;
        }



            //------------------MOVEMENT---------------------------------------------------------------

            float input = Input.GetAxis("Horizontal"); // -1 (left) to +1 (right)
        Vector2 moveDirection = (Vector2)transform.right * input;
        if (isGrounded)
        {
            if(target != null && target.gameObject.layer == 7)
            {
                moveDirection *= -1;
            }
            if(target != null && target.gameObject.layer != 7 && transform.eulerAngles.z == 180)
            {
                moveDirection *= -1;
            }
        }
        if (isOnWall)
        {
            inputVelocity = moveDirection * moveSpeed;
        }
        else
        {
            inputVelocity = new Vector2(moveDirection.x * moveSpeed, rb.velocity.y);
        }

        if (wallJumpLockCounter <= 0)
        {
            rb.velocity = inputVelocity;
        }
        else
        {
            wallJumpLockCounter -= Time.deltaTime;
        }
        LastInputVelocity = inputVelocity;

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
            if (currentWall == wallTypes.top)
            {
                if (rb.velocity.x > 0)
                {
                    transform.localScale = new Vector3(-xScale, transform.localScale.y, 0);
                }
                if (rb.velocity.x < 0)
                {
                    transform.localScale = new Vector3(xScale, transform.localScale.y, 0);
                }
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
        if (jumpBufferCounter > 0)
        {
            if (isGrounded && !isOnWall)
            {
                Jump();
            }
            else if (isOnWall)
            {
                WallJump();
            }
        }
    }

    private void Jump()
    {
        
        jumpBufferCounter = 0f;
        Vector2 v = rb.velocity;
        v.y = 0f;
        v.y += jumpForce;
        rb.velocity = v;
        SoundEffectSource.Instance.PlaySoundEffect(0);
    }
    private void WallJump()
    {
        SoundEffectSource.Instance.PlaySoundEffect(0);
        wallJumpLockCounter = wallJumpLockTime;
        if (currentWall == wallTypes.top)
        {
            wallJumpLockCounter = 0;
        }
        isOnWall = false;
        transform.rotation = Quaternion.identity;
        jumpBufferCounter = 0f;

        // Reset current velocity
        rb.velocity = Vector2.zero;

        // Determine jump direction based on wall
        Vector2 jumpDir = Vector2.zero;

        switch (currentWall)
        {
            case wallTypes.right:
                jumpDir = Vector2.left * 5 + Vector2.up * 10;
                transform.localScale = new Vector3(xScale, transform.localScale.y, 0); // Face left
                break;
            case wallTypes.left:
                jumpDir = Vector2.right * 5 + Vector2.up * 10;
                transform.localScale = new Vector3(-xScale, transform.localScale.y, 0); // Face right
                break;
            default:
                return; // not on a valid wall side
        }
        Debug.Log("wallJump");

        // Apply wall jump force
        rb.AddForce(jumpDir.normalized * jumpForce, ForceMode2D.Impulse);
    }
}
