using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class PlayerController : MonoBehaviour
{
    [Header("Horizontal Movement")]
    public float moveSpeed;
    public Vector2 direction;
    private bool facingRight = true;
    

    [Header("Components")]
    public Text score;
    public Text liveText;
    private Rigidbody2D rd2d;
    public GameObject winText;
    public GameObject loseText;
    private int scoreValue = 0;
    public int lives = 3;
    public LayerMask groundLayer;
    public Animator animator;
    public GameObject blackSquare;
    public AudioClip levelUp;
    public AudioClip backgroundMusic;
    public AudioSource musicSource;
    public GameObject level_1;
    public GameObject level_2;
    public GameObject spawnPoint;
       
    
    
    [Header("Vertical Movement")]
    public float jumpForce;
    public float jumpDelay = 0.25f;
    private float jumpTimer;



    [Header("Physics")]
    public float maxSpeed = 7f;
    public float linearDrag = 4f;
    public float gravity = 0.4f;
    public float fallMultiplier = 5f;

    [Header("Collision")]
    public bool onGround = false;
    public float groundLength = 0.6f;
    public Vector3 colliderOffset;
    

    // Start is called before the first frame update
    void Start()
    {
        rd2d = GetComponent<Rigidbody2D>();
        score.text = scoreValue.ToString();
        liveText.text = lives.ToString();
        winText.SetActive(false);
        loseText.SetActive(false);
        musicSource.clip = backgroundMusic;
        musicSource.loop = true;
        musicSource.Play();
        spawnPoint.transform.position = new Vector3(0, 0, 0);
        level_2.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        onGround = Physics2D.Raycast(transform.position + colliderOffset, Vector2.down, groundLength, groundLayer) || Physics2D.Raycast(transform.position - colliderOffset, Vector2.down, groundLength, groundLayer);
  
        if (Input.GetButtonDown("Jump") || Input.GetKeyDown(KeyCode.W))
        {
            jumpTimer = Time.time + jumpDelay;
        }

        direction = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));


        

        if (Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }

        if (transform.position.y <= -50)
        {
            transform.position = spawnPoint.transform.position;
        }
    }

    void moveCharacter(float horizontal)
    {
        rd2d.AddForce(Vector2.right * horizontal * moveSpeed);
        if ((horizontal > 0 && !facingRight) || (horizontal < 0 && facingRight))
        {
            Flip();
        } 
        if (Mathf.Abs(rd2d.velocity.x) > maxSpeed)
        {
            rd2d.velocity = new Vector2((Mathf.Sign(rd2d.velocity.x)) * (maxSpeed),rd2d.velocity.y);
        }
        //Animating
        //y direction
        if ((rd2d.velocity.y) > 0 && !onGround)
        {
            animator.SetBool("isRising", true);
            animator.SetBool("isFalling", false);
            animator.SetBool("isIdle", false);
            animator.SetBool("isWalking", false);
            animator.SetBool("isRunning", false);
        }
        if ((rd2d.velocity.y) < 0 && !onGround)
        {
            animator.SetBool("isRising", false);
            animator.SetBool("isFalling", true);
            animator.SetBool("isIdle", false);
            animator.SetBool("isWalking", false);
            animator.SetBool("isRunning", false);
        }
        if ((rd2d.velocity.y) == 0 && onGround)
        {
            animator.SetBool("isRising", false);
            animator.SetBool("isFalling", false);
            animator.SetBool("isIdle", true);
            animator.SetBool("isWalking", false);
            animator.SetBool("isRunning", false);
        }
        //x direction
        if (Mathf.Abs(rd2d.velocity.x) > 0 && Mathf.Abs(rd2d.velocity.x) < 1 && onGround)
        {
            animator.SetBool("isWalking", true);
            animator.SetBool("isIdle", false);
            animator.SetBool("isRunning", false);
        }
        if (Mathf.Abs(rd2d.velocity.x) >= 1 && onGround)
        {
            animator.SetBool("isRunning", true);
            animator.SetBool("isWalking", false);
            animator.SetBool("isIdle", false);
        }
        if (Mathf.Abs(rd2d.velocity.x) == 0 && onGround)
        {
            animator.SetBool("isRunning", false);
            animator.SetBool("isWalking", false);
            animator.SetBool("isIdle", true);
        }

        

    }

    void characterJump()
    {
        rd2d.velocity = new Vector2(rd2d.velocity.x, 0);
        rd2d.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        jumpTimer = 0;
    }

    void modifyPhysics()
    {
        bool changingDirections = (direction.x > 0 && rd2d.velocity.x < 0) || (direction.x < 0 && rd2d.velocity.x > 0);

        if (onGround)
        {
            if (Mathf.Abs(direction.x) < 0.4f || changingDirections)
            {
                rd2d.drag = linearDrag;
            }
            else
            {
                rd2d.drag = 0f;
            } 
        }
        else
        {
            rd2d.gravityScale = gravity;
            rd2d.drag = linearDrag * 0.15f;
            if (rd2d.velocity.y < 0)
            {
                rd2d.gravityScale = gravity * fallMultiplier;
            }
            else if (rd2d.velocity.y > 0 && (!Input.GetButton("Jump") || !Input.GetKey(KeyCode.W)))
            {
                rd2d.gravityScale = gravity * (fallMultiplier / 2);
            }
        }
        
    }

    void FixedUpdate()
    {
        moveCharacter(direction.x);
        if (jumpTimer > Time.time && onGround)
        {
            characterJump();
        }
        modifyPhysics();
    }

    void Flip()
    {
        facingRight = !facingRight;
        transform.rotation = Quaternion.Euler(0, facingRight ? 0: 180, 0);                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                               
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("Trigger Event");
       if (collision.tag == "Coin")
        {
            scoreValue += 1;
            score.text = scoreValue.ToString();
            Destroy(collision.gameObject);
            if (scoreValue == 4)
            {
                musicSource.Stop();
                musicSource.clip = levelUp;
                musicSource.loop = false;
                musicSource.Play();
                level_2.SetActive(true);
                transform.position = new Vector3(77, -3, 0);
                spawnPoint.transform.position = new Vector3(77, -3, 0);
                musicSource.Stop();
                musicSource.clip = backgroundMusic;
                musicSource.loop = true;
                musicSource.Play();
                lives = 3;
                liveText.text = lives.ToString();     
                level_1.SetActive(false);
                
                
            }

            if (scoreValue == 8)
            {
                winText.SetActive(true);
                musicSource.Stop();
                musicSource.clip = levelUp;
                musicSource.loop = false;
                musicSource.Play();
            }
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            lives += -1;
            liveText.text = lives.ToString();
            Destroy(collision.gameObject);
            if (lives == 0)
            {
                loseText.SetActive(true);
                gameObject.SetActive(false);
            }

        }
    }

    private void onDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position + colliderOffset, transform.position + colliderOffset + Vector3.down * groundLength);
        Gizmos.DrawLine(transform.position - colliderOffset, transform.position - colliderOffset + Vector3.down * groundLength);
    }

    
}
