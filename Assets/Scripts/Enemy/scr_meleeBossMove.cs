using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class scr_meleeBossMove : MonoBehaviour
{
    const string LEFT = "left";
    const string RIGHT = "right";

    public bool isPlayerInRange = false;
    public scr_meleeBoss bossEnemy;
    public float movespeed = 1f;

    public bool inAir = false;

    public bool isAlerted = false;

    Rigidbody2D rb;
    BoxCollider2D boxCollider;

    [SerializeField]
    Transform castPos;

    [SerializeField]
    float baseCastDist;

    string facingDir;

    Vector3 baseScale;

    public Animator animator;
    public bool isKnockedBack = false;
    public float knockBackMaxTime = 1.2f;
    public float knockbackTime = 0f;



    public bool isGrounded = false;
    public Transform groundCheck;
    public float airCheckRadius = 0.6f;
    public LayerMask GroundLayer;


    public GameObject playerobj;
    private void Awake()
    {
        baseScale = transform.localScale;
        facingDir = RIGHT;
        bossEnemy = GetComponent<scr_meleeBoss>();
        movespeed = bossEnemy.moveSpd;
        rb = GetComponent<Rigidbody2D>();
        boxCollider = GetComponent<BoxCollider2D>();
        playerobj = GameObject.FindGameObjectWithTag("Player");
    }


    private void FixedUpdate()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, airCheckRadius, GroundLayer);

        if (bossEnemy.isDead == false)
        {
            float velocityX = movespeed;

            if (bossEnemy.isinAir == false && bossEnemy.attacking == false && isKnockedBack == false )
            {
                if (rb.velocity.x != 0f)
                {
                    if (bossEnemy.attacking == false)
                        animator.Play("Walk");
                }
                else
                {
                   
                    
                       animator.Play("Idle");
                    
                        
                }
            }
            if (isKnockedBack)
            {
                knockbackTime -= Time.deltaTime;
                if (knockbackTime <= 0f)
                {
                    knockbackTime = 0f;
                    isKnockedBack = false;
                }
                else
                {
                    animator.Play("Knockedback");
                }
            }
            else

            {
                TargetOnPlayer();
                if (bossEnemy.attacking == false)
                {
                    if (facingDir == LEFT)
                    {
                        velocityX = -movespeed;
                    }
                    //enemy patrol move
                    if (isGrounded == true && bossEnemy.isAttacked == false)
                    {
                        rb.velocity = new Vector2(velocityX, rb.velocity.y);
                    }


                    if ((isHittingWall() || isNearEdge()) && isGrounded == true)
                    {
                        if (facingDir == LEFT)
                        {
                            changeFaceDir(RIGHT);
                        }
                        else if (facingDir == RIGHT)
                        {
                            changeFaceDir(LEFT);
                        }

                    }
                }
               
            }
        }
    }

    void changeFaceDir(string newDir)
    {
        Vector3 newScale = baseScale;
        if (newDir == LEFT)
        {
            newScale.x = -baseScale.x;
        }
        else if (newDir == RIGHT)
        {
            newScale.x = baseScale.x;
        }

        transform.localScale = newScale;

        facingDir = newDir;
    }

    private bool isFaceRight()
    {
        return transform.localScale.x > Mathf.Epsilon;
    }

    bool isHittingWall()
    {
        bool val = false;
        float castDist = baseCastDist;
        //define cast dist for L and R
        if (facingDir == LEFT)
        {
            castDist = -baseCastDist;
        }
        else
        {
            castDist = baseCastDist;
        }

        //determine target destination based on cast distance
        Vector3 targetPos = castPos.position;
        targetPos.x += castDist;

        Debug.DrawLine(castPos.position, targetPos, Color.green);


        if (Physics2D.Linecast(castPos.position, targetPos, 1 << LayerMask.NameToLayer("Wall")))
        {
            val = true;
        }
        else
        {
            val = false;
        }

        return val;
    }

    bool isNearEdge()
    {
        bool val = true;
        float castDist = baseCastDist;
        //define cast dist for L and R


        //determine target destination based on cast distance
        Vector3 targetPos = castPos.position;
        targetPos.y -= castDist;

        Debug.DrawLine(castPos.position, targetPos, Color.cyan);


        if (Physics2D.Linecast(castPos.position, targetPos, 1 << LayerMask.NameToLayer("Ground")))
        {
            val = false;
        }
        else
        {
            val = true;
        }

        return val;
    }

    public void tempFreeze()
    {
        rb.velocity = new Vector2(0, 0);
    }

    public void knockBack()
    {
        isKnockedBack = true;
        knockbackTime = knockBackMaxTime;
        Transform attackerTrans = playerobj.transform;
        Vector2 knockBackDir = new Vector2(transform.position.x - attackerTrans.transform.position.x, 0);
        rb.velocity = new Vector2(knockBackDir.x, 0.2f) * 0.8f;
    }

    public void TargetOnPlayer()
    {
        if (isAlerted)
        {
            if (playerobj.transform.position.x > transform.position.x)
            {
                changeFaceDir(RIGHT);
            }
            else if (playerobj.transform.position.x < transform.position.x)
            {
                changeFaceDir(LEFT);
            }

        }

    }

    public void Aircheck()
    {

    }
}
