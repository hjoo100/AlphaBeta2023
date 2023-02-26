using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Scr_PlayerCtrl : MonoBehaviour
{
    public GameObject PlayerObj;
    public KeyCode M_left, M_right, M_jump;
    

    private Rigidbody2D rb;
    private Transform playertransform;
    private float moveDir;
    private bool toRight = true;
    private bool isJumping = false;
    
    private bool isInMeleeRange;
    private float jumpCount;

    public float PlayerSpd = 0.5f;
    public float jumpForce = 5f;
    public Transform cellingCheck;
    public Transform groundCheck;
    public LayerMask GroundObj;
    public string EnemyTag;
    public float jumpCheckRaidus;
    public float MaxJumpNum;

    public float hitpoints = 100;
    public float maxHp = 100;
    public bool isDead = false;
    public GameObject attackArrow;
    public float meleeRange;
    public float CurrMeleeCD = 0;
    public float meleeCD = 0.2f;
    public float meleeDmg = 10f;

    public bool isWalking = false, isAir = false, isAttacking = false;
    public bool isGrounded = false;
    //animation
    Animator playerAnimator;
    private string currentState;

    scr_GManager Gamemanager;
    public void Awake()
    {
        rb = gameObject.GetComponent<Rigidbody2D>();
        attackArrow.GetComponent<scr_attackArrow>().enemyTag = EnemyTag;
        playertransform = gameObject.transform;
        playerAnimator = gameObject.GetComponent<Animator>();
        playerAnimator.Play("Idle");
        hitpoints = maxHp;
        Gamemanager = GameObject.FindGameObjectWithTag("GManager").GetComponent<scr_GManager>();
    }
    // Start is called before the first frame update
    void Start()
    {
        jumpCount = MaxJumpNum;
    }

    // Update is called once per frame
    void Update()
    {

        ReceiveInputFunc();
        attack();

        if(isDead == true)
        {
            print("Player Died!");
            Gamemanager.LoseFunc();
            Destroy(gameObject);
            
        }
    }

    //fixed update for movement
    private void FixedUpdate()
    {
        //check for ground 
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, jumpCheckRaidus, GroundObj);
        if(isGrounded)
        {
            jumpCount = MaxJumpNum;
        }

        charaMoveFunc();

        if(CurrMeleeCD > 0)
        {
            CurrMeleeCD -= Time.deltaTime;
            if(CurrMeleeCD <0)
            {
                CurrMeleeCD = 0;
            }
        }

        
    }

    void ReceiveInputFunc()
    {
        moveDir = Input.GetAxis("Horizontal");

        //rb.velocity = new Vector2(PlayerSpd * moveDir, rb.velocity.y);
        if(moveDir >0 && toRight == false)
        {
            flipChara();
        }
        else if(moveDir <0 && toRight == true)
        {
            flipChara();
        }

        if(Input.GetButtonDown("Jump") && jumpCount >0)
        {
            isJumping = true;

        }
    }

    void charaMoveFunc()
    {
        rb.velocity = new Vector2(PlayerSpd * moveDir, rb.velocity.y);
        if(isJumping && jumpCount >0)
        {
            rb.AddForce(new Vector2(0f, jumpForce));
            print("jumped");
            animationSwitch("Jump");
            jumpCount--;
        }
        isJumping = false;
        if(moveDir == 0 && isGrounded && !isAttacking)
        {
            animationSwitch("Idle");
        }
        else if(isGrounded && !isAttacking)
        {
            animationSwitch("Walk");
        }
    }

    void flipChara()
    {
        toRight = !toRight;

        playertransform.localScale = new Vector2(-playertransform.localScale.x,playertransform.localScale.y);
    }

    void attack()
    {
        //isInMeleeRange = Physics2D.OverlapCircle(attackArrow.position, meleeRange, EnemyLayer);
        if(Input.GetKeyDown(KeyCode.Z) && CurrMeleeCD == 0)
        {
            print("Melee Pressed");
            //do melee attack
            attackArrow.GetComponent<scr_attackArrow>().attackEnemyInRange(meleeDmg);
            playerAnimator.Play("Attack");
            isAttacking = true;
            Invoke(nameof(resetAttack), 0.27f);
            CurrMeleeCD = meleeCD;
        }
    }



    //animation

    void animationSwitch(string animState)
    {
        if(currentState == animState)
        {
            return;
        }

        //play animation
        playerAnimator.Play(animState);
    }

    public void takeDmg(float dmg)
    {
        hitpoints -= dmg;
        if(hitpoints < 0)
        {
            isDead = true;
        }
    }

    void resetAttack()
    {
        isAttacking = false;
    }
}

