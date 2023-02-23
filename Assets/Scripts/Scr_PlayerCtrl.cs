using System.Collections;
using System.Collections.Generic;
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
    private bool isGrounded;
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

    public GameObject attackArrow;
    public float meleeRange;
    public float CurrMeleeCD = 0;
    public float meleeCD = 0.2f;
    public float meleeDmg = 10f;

    //animation
    Animator playerAnimator;
    private string currentState;
    public void Awake()
    {
        rb = gameObject.GetComponent<Rigidbody2D>();
        attackArrow.GetComponent<scr_attackArrow>().enemyTag = EnemyTag;
        playertransform = gameObject.transform;
        playerAnimator = gameObject.GetComponent<Animator>();
        playerAnimator.Play("Idle");
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
        if(moveDir == 0)
        {
            animationSwitch("Idle");
        }
        else if(isGrounded)
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
}

