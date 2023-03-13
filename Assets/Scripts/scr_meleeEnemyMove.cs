using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using UnityEngine;
using UnityEngine.AI;

public class scr_meleeEnemyMove : MonoBehaviour
{
    const string LEFT = "left";
    const string RIGHT = "right";

    public bool isPlayerInRange = false;
    public scr_MeleeEnemy enemy;
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
        enemy = GetComponent<scr_MeleeEnemy>();
        movespeed = enemy.moveSpd;
        rb = GetComponent<Rigidbody2D>();
        boxCollider = GetComponent<BoxCollider2D>();
        playerobj = GameObject.FindGameObjectWithTag("Player");
    }

    
    private void FixedUpdate()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, airCheckRadius, GroundLayer);

        if (enemy.isDead == false)
        {
            float velocityX = movespeed;

            if(enemy.isinAir == false && enemy.isAttacked == false && isKnockedBack == false)
            {
                if (rb.velocity.x != 0f)
                {
                    if(enemy.attacking == false)
                    animator.Play("Walk");
                }else
                {
                    if (enemy.attacking == false)
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

                if (facingDir == LEFT)
                {
                    velocityX = -movespeed;
                }
                //enemy patrol move
                if(isGrounded == true)
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

    void changeFaceDir(string newDir)
    {
        Vector3 newScale = baseScale;
        if(newDir==LEFT)
        {
            newScale.x = -baseScale.x;
        }
        else if(newDir==RIGHT)
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
        if(facingDir == LEFT)
        {
            castDist = -baseCastDist;
        }else
        {
            castDist = baseCastDist;
        }

        //determine target destination based on cast distance
        Vector3 targetPos = castPos.position;
        targetPos.x += castDist;

        Debug.DrawLine(castPos.position, targetPos, Color.green);


        if(Physics2D.Linecast(castPos.position,targetPos,1<<LayerMask.NameToLayer("Wall")))
        {
            val = true;
        }else
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
        rb.velocity = new Vector2(knockBackDir.x, 0.2f) * 1.3f;
    }

    public void TargetOnPlayer()
    {
        if(isAlerted)
        {
            if(playerobj.transform.position.x > transform.position.x)
            {
                changeFaceDir(RIGHT);
            }else if(playerobj.transform.position.x < transform.position.x)
            {
                changeFaceDir(LEFT);
            }
            
        }
        
    }

    public void Aircheck()
    {

    }
    /*
     *  public NavMeshAgent agent;
    public Transform PlayerTransform;
    public LayerMask groundMask, playermask;
    public scr_enemyFOV enemyFOV;
    public bool lastFrameInSight = false;
    public scr_MeleeEnemy meleEnemyParams;

    //state
    public float attackRange;
    public bool isPlayerInSight, isPlayerInAttackRange;


    //to right
    public bool isRight = false;

    public Vector3 previousPos;
    public float curSpeed;

    //var for patrol
    public Vector3 patrolPoint;
    public bool isPatrolPointSet;
    public float patrolPointRange;

    //var for idle
    public float idleTime = 2.5f;
    float currIdleTime = 0f;
    public bool isIdling = false;

    // var for confused
    public float confuseTime = 2.5f;
    public float currConfuseTime = 0f;
    public bool isConfused = false;

    public Animator enemyAnimator;

    private void Awake()
    {
        PlayerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        agent = GetComponent<NavMeshAgent>();
        agent.speed = meleEnemyParams.moveSpd; 

    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        //check if player is in sight or in attackRange
        isPlayerInSight = enemyFOV.canSeePlayer;
        isPlayerInAttackRange = Physics.CheckSphere(transform.position, attackRange, playermask);

        Vector3 curMove = transform.position - previousPos;
        curSpeed = curMove.magnitude / Time.deltaTime;

        if (curMove.x > 0 && isRight == false)
        {
            Vector3 tempScaleVec = new Vector3(-1, 1, 1);
            transform.localScale = tempScaleVec;
            isRight = true;
        }

        if (curMove.x < 0 && isRight == true)
        {
            Vector3 tempScaleVec = new Vector3(1, 1, 1);
            transform.localScale = tempScaleVec;
            isRight = false;
        }

        if (lastFrameInSight == true && isPlayerInSight == false)
        {
            //lost visual of player
            isConfused = true;
            Debug.Log("confused!");
            currConfuseTime = confuseTime;
            Confusing();


        }

        else if (isConfused == true)
        {
            //stay in confused state
            Confusing();
        }

        else
        {
            // if (!isPlayerInSight && !isPlayerInAttackRange)
            if (!isPlayerInSight)
            {
                Patroling();
                //enemyAnimator.SetBool("IsAttacking", false);
            }
            if (isPlayerInSight && !isPlayerInAttackRange)
            {
                currIdleTime = 0;
                Chasing();
                //enemyAnimator.SetBool("IsAttacking", false);
            }
            if (isPlayerInAttackRange && isPlayerInSight)
            {
                Attacking();
            }


        }
        //save in sight bool
        lastFrameInSight = isPlayerInSight;

        previousPos = transform.position;

    }

    private void SearchPatrolPoint()
    {
        //Generate random patrol point in range
        float randomZ = Random.Range(-patrolPointRange, patrolPointRange);
        float randomX = Random.Range(-patrolPointRange, patrolPointRange);

        patrolPoint = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ);

        NavMeshPath patrolPath = new NavMeshPath();
        if (Physics.Raycast(patrolPoint, -transform.up, 2f, groundMask) && (agent.CalculatePath(patrolPoint, patrolPath) && patrolPath.status == NavMeshPathStatus.PathComplete))
        {
            isPatrolPointSet = true;
        }
    }
    public void Confusing()
    {
        
            // enemy in confused state will not move
            // enemy will enter confuse state if lost visual of player
            if (currConfuseTime > 0)
            {
                currConfuseTime -= Time.deltaTime;
                enemyAnimator.SetBool("IsIdling", true);
                enemyAnimator.SetBool("IsWalking", false);

            }
            if (currConfuseTime <= 0)
            {
                currConfuseTime = 0;
                isConfused = false;
                enemyAnimator.SetBool("IsIdling", false);

            }

        
    }

    public void Patroling()
    {
        if (!isPatrolPointSet)
        {
            SearchPatrolPoint();
        }

        if (isPatrolPointSet)
        {
            agent.SetDestination(patrolPoint);
            enemyAnimator.Play("Walking");
        }

        Vector3 distanceToPatrolPoint = transform.position - patrolPoint;

        //arrive patrol point
        if (distanceToPatrolPoint.magnitude < 1f)
        {
            Idling();
            if (isIdling == false)
            {
                isPatrolPointSet = false;
            }
        }
    }

    public void Chasing()
    {
        agent.SetDestination(PlayerTransform.position);
        //enemyAnimator.SetBool("IsWalking", true);
        //enemyAnimator.SetBool("IsIdling", false);
        enemyAnimator.Play("Walking");
    }

    public void Attacking()
    {
        agent.SetDestination(transform.position);
        meleEnemyParams.Attacking();
        //transform.LookAt(PlayerTransform);

    }

    private void Idling()
    {
        //idle after arrived at patrol point 
        if (isIdling == false)
        {
            isIdling = true;
        }
        if (isIdling == true && currIdleTime == 0)
        {
            currIdleTime = idleTime;
        }
        else if (isIdling == true && currIdleTime > 0)
        {
            currIdleTime -= Time.deltaTime;
            //enemyAnimator.SetBool("IsIdling", true);
            enemyAnimator.Play("Idle");
            //enemyAnimator.SetBool("IsWalking", false);
            if (currIdleTime <= 0)
            {
                currIdleTime = 0;
                isIdling = false;
            }
        }
    }

    public GameObject FindClosestObj(string Tag)
    {
        GameObject[] objCollection;
        objCollection = GameObject.FindGameObjectsWithTag(Tag);
        GameObject closest = null;
        float distance = Mathf.Infinity;
        Vector3 position = transform.position;
        foreach (GameObject go in objCollection)
        {
            Vector3 diff = go.transform.position - position;
            float curDistance = diff.sqrMagnitude;
            if (curDistance < distance)
            {
                closest = go;
                distance = curDistance;
            }
        }
        return closest;
    }

    private void OnDrawGizmosSelected()
    {
        //draw attackRange
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }

     */
}
