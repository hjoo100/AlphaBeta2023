using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class scr_rangeEnemyMove : MonoBehaviour
{

    const string LEFT = "left";
    const string RIGHT = "right";
    string facingDir = RIGHT;

    private Rigidbody2D enemyRB;
    private scr_enemyBase enemyBase;

    [SerializeField]
    private float moveSpd = 5f;

    private GameObject Player;

    [SerializeField]
    private bool isAwake = false, isDead = false, isinAir = false;
    private Scr_PauseManager pauseManager;

    public bool moveToPlayer = false;

    private int moveDirection;

    private Vector3 baseScale;

    [SerializeField]
    Transform castPos,castPosHead;

    bool alerted = false;

    float baseCastDist = 0.6f;

    [SerializeField]
    private LayerMask groundAndOneWayPlatformMask;


    public Animator animator;

    void Awake()
    {
        pauseManager = FindObjectOfType<Scr_PauseManager>();
        baseScale = transform.localScale;
    }

    // Start is called before the first frame update
    void Start()
    {
        enemyRB = GetComponent<Rigidbody2D>();
        enemyBase = GetComponent<scr_enemyBase>();
        Player = GameObject.FindGameObjectWithTag("Player");
        moveDirection = (Random.Range(0, 2) == 0) ? -1 : 1;
        if(moveDirection == -1)
        {
            facingDir = LEFT;
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (enemyBase.isDead || gameObject.GetComponent<scr_movingRangedEnemy>().canAttack == false)
        {
            return;
        }

        if (pauseManager.IsPaused())
        {
            return;
        }

        if (!isAwake || isDead || isinAir)
        {
            return;
        }

        if (Player != null && !GetComponent<scr_movingRangedEnemy>().IsAttacking())
        {
            
            float distance = Vector2.Distance(transform.position, Player.transform.position);
            if (distance < enemyBase.detectDist || alerted)
            {
                moveToPlayer = true;
                alerted = true;
            }
            else
            {
                moveToPlayer = false;
            }
            if (moveToPlayer)
            {
                float x = Player.transform.position.x - transform.position.x;
                Vector2 moveDirection = new Vector2(x, 0).normalized;
                enemyRB.velocity = moveDirection * moveSpd;

                // Change facing direction based on the player's position
                if (x > 0 && facingDir == LEFT)
                {
                    changeFaceDir(RIGHT);
                }
                else if (x < 0 && facingDir == RIGHT)
                {
                    changeFaceDir(LEFT);
                }

                animator.Play("WoodBowManMove");
            }
            else
            {
                enemyRB.velocity = new Vector2(moveDirection * moveSpd, 0);

                if ((isHittingWall() || isNearEdge()) && isAwake && !isDead && !isinAir)
                {
                    if(isHittingWall())
                    {
                       // Debug.Log("Hitting wall");
                    }
                    if(isNearEdge())
                    {
                       // Debug.Log("Nearing Edge");
                    }
                    if (facingDir == LEFT)
                    {
                        changeFaceDir(RIGHT);
                        moveDirection = 1; // Change moveDirection to match the new facing direction
                    }
                    else if (facingDir == RIGHT)
                    {
                        changeFaceDir(LEFT);
                        moveDirection = -1; 
                    }
                }

                animator.Play("WoodBowManMove");
            }
        }
        else
        {
            enemyRB.velocity = Vector2.zero;
            
        }

    }
    public void setAwake(bool awake)
    {
        isAwake = awake;
    }

    public void setDead(bool dead)
    {
        isDead = dead;

        
    }

    public void setInAir(bool inAir)
    {
        isinAir = inAir;
    }

    public void setMoveSpd(float speed)
    {
        moveSpd = speed;
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

        // Use groundAndOneWayPlatformMask in the Linecast method
        if (Physics2D.Linecast(castPos.position, targetPos, groundAndOneWayPlatformMask) || Physics2D.Linecast(castPosHead.position, targetPosHead, groundAndOneWayPlatformMask))
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

        //determine target destination based on cast distance
        Vector3 targetPos = castPos.position;
        targetPos.y -= castDist;

        Debug.DrawLine(castPos.position, targetPos, Color.cyan);

        // Use groundAndOneWayPlatformMask in the Linecast method
        if (Physics2D.Linecast(castPos.position, targetPos, groundAndOneWayPlatformMask))
        {
            val = false;
        }
        else
        {
            val = true;
        }

        return val;
    }
}
