using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class scr_ShieldBossMove : MonoBehaviour
{
    const string LEFT = "left";
    const string RIGHT = "right";

    public bool isPlayerInRange = false;
    public scr_ShieldBossEnemy enemy;
    public float movespeed = 1f;

    public bool inAir = false;

    public bool isAlerted = false;

    Rigidbody2D rb;
    BoxCollider2D boxCollider;

    [SerializeField]
    Transform castPos;
    [SerializeField]
    Transform castPosHead;
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

    Scr_PauseManager pauseManager;

    private void Awake()
    {
        baseScale = transform.localScale;
        facingDir = RIGHT;
        enemy = GetComponent<scr_ShieldBossEnemy>();
        movespeed = enemy.getMoveSpd();
        rb = GetComponent<Rigidbody2D>();
        boxCollider = GetComponent<BoxCollider2D>();
        playerobj = GameObject.FindGameObjectWithTag("Player");

        pauseManager = FindObjectOfType<Scr_PauseManager>();
    }


    private void FixedUpdate()
    {
        if (pauseManager.IsPaused())
        {
            return; // Do not execute the rest of the Update logic if the game is paused
        }

        isGrounded = Physics2D.OverlapCircle(groundCheck.position, airCheckRadius, GroundLayer);

        if (enemy.getIsDead() == false)
        {
            float velocityX = movespeed;

            if ((enemy.getInairBool() == false) && (enemy.getAttackedBool() == false) && isKnockedBack == false)
            {
                if (rb.velocity.x != 0f)
                {
                    if (enemy.getAttackingBool() == false)
                        animator.Play("Walk");

                }
                else
                {
                    if (enemy.getAttackingBool() == false)
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
                if (enemy.getAttackingBool() == false && isAlerted == true)
                {
                    if (facingDir == LEFT)
                    {
                        velocityX = -movespeed;
                    }
                    //enemy patrol move
                    if (isGrounded == true && enemy.getAttackedBool() == false)
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
                        Debug.Log("changing direction because of environment");
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

        Vector3 targetPosHead = castPosHead.position;
        targetPosHead.x += castDist;

        Debug.DrawLine(castPos.position, targetPos, Color.green);
        Debug.DrawLine(castPosHead.position, targetPos, Color.green);

        if (Physics2D.Linecast(castPos.position, targetPos, 1 << LayerMask.NameToLayer("Wall")) || Physics2D.Linecast(castPosHead.position, targetPosHead, 1 << LayerMask.NameToLayer("Wall")))
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

    public void PoweredKnockBack(float forceVal)
    {
        isKnockedBack = true;
        knockbackTime = knockBackMaxTime;
        Transform attackerTrans = playerobj.transform;
        Vector2 knockBackDir = new Vector2(transform.position.x - attackerTrans.transform.position.x, 0);
        rb.velocity = new Vector2(knockBackDir.x, 0.2f) * forceVal;
    }

    public void TargetOnPlayer()
    {
        if (isAlerted)
        {
            if (playerobj.transform.position.x > transform.position.x)
            {
                changeFaceDir(RIGHT);
                //Debug.Log("Facing RIGHT");

            }
            else if (playerobj.transform.position.x < transform.position.x)
            {
                changeFaceDir(LEFT);
                //Debug.Log("Facing LEFT");

            }

        }

    }

    public void Aircheck()
    {

    }
}
